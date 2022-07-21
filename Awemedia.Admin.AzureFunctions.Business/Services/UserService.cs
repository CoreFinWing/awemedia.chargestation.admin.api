using Awemedia.ADB2C;
using Awemedia.ADB2C.Models;
using Awemedia.Admin.AzureFunctions.Business.Helpers;
using Awemedia.Admin.AzureFunctions.Business.Infrastructure;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.Json;
using User = Awemedia.Admin.AzureFunctions.DAL.DataContracts.User;
using CountryDB = Awemedia.Admin.AzureFunctions.DAL.DataContracts.Country;
using UserModel = Awemedia.Admin.AzureFunctions.Business.Models.UserModel;
using Role = Awemedia.Admin.AzureFunctions.DAL.DataContracts.Role;


namespace Awemedia.Admin.AzureFunctions.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IBaseService<User> _baseService;
        private readonly IBaseService<CountryDB> _countryService;
        private readonly IBaseService<Role> _roleService;
        private readonly IEmailService _emailService;
        private readonly IADB2CService _adb2cService;
        readonly string[] includedProperties = new string[] { "Country" };
        public UserService(IBaseService<User> baseService, IBaseService<CountryDB> countryService, IEmailService emailService, IBaseService<Role> roleService, IADB2CService adb2cService)
        {
            _baseService = baseService;
            _countryService = countryService;
            _emailService = emailService;
            _roleService = roleService;
            _adb2cService = adb2cService;
        }
        public IEnumerable<object> Get(BaseSearchFilter userSearchFilter, out int totalRecords, bool isActive = true)
        {
            IEnumerable<UserModel> _users = null;
            totalRecords = 0;
            string[] navigationalProps = { "Country", "Role" };
            if (!string.IsNullOrEmpty(userSearchFilter.FromDate) && !string.IsNullOrEmpty(userSearchFilter.ToDate))
            {
                DateTime fromDate = DateTime.Now.ToUniversalTime();
                DateTime toDate = DateTime.Now.ToUniversalTime();
                fromDate = Utility.ParseStartAndEndDates(userSearchFilter, ref toDate);
                _users = _baseService.Where(a => a.CreatedDate.Value.Date >= fromDate && a.CreatedDate.Value.Date <= toDate, navigationalProps).Select(t => MappingProfile.MapUserModelObject(t)).ToList();
            }
            else
            {
                _users = _baseService.GetAll(navigationalProps).Select(t => MappingProfile.MapUserModelObject(t)).ToList();
            }
            totalRecords = _users.Count();
           
            if (userSearchFilter != null)
            {
                if (!string.IsNullOrEmpty(userSearchFilter.Search) && !string.IsNullOrEmpty(userSearchFilter.Type))
                {
                    _users = _users.Search(userSearchFilter.Type, userSearchFilter.Search);
                    totalRecords = _users.Count();
                }
                _users = _users.OrderBy(userSearchFilter.Order, userSearchFilter.Dir);
                if (!Convert.ToBoolean(userSearchFilter.Export))
                {
                    _users = _users.Skip((Convert.ToInt32(userSearchFilter.Start) - 1) * Convert.ToInt32(userSearchFilter.Size)).Take(Convert.ToInt32(userSearchFilter.Size));
                }
                else
                {
                    var dataToExport = _users.ToList();
                    return dataToExport.ToList();
                }
            }

            return _users.ToList();
        }


        public int AddUser(UserModel userModel, int id = 0)
        {
            if (!IsUserDuplicate(userModel))
            {
                AweMediaUser awUser = new AweMediaUser();
                MappingProfile.MapAweMediaUserObject(userModel, awUser);
                awUser.CountryName = _countryService.GetById(userModel.CountryId).CountryName;
                awUser.Role = _roleService.GetById(userModel.RoleId).Name;
                var adb2cUser = _adb2cService.CreateUserWithCustomAttribute(JsonSerializer.Serialize(awUser)).Result;
                var user = _baseService.AddOrUpdate(MappingProfile.MapUserObject(userModel, new DAL.DataContracts.User(),adb2cUser.Item2), 0);
                userModel.Id = user.Id;



                EmailModel emailModel = new EmailModel
                {
                    Content = string.Format("Hi {0}, <br/><br/> You account has been created successfully on awemedia. <br/> <br/><br/> Please use the password <b>{1}</b> and your email to login", user.Name, adb2cUser.Item1),
                    MailRecipientsTo = user.Email,
                    Subject = "Your AweMedia account created successfully",
                };
                _emailService.SendEmailAsync(emailModel);
                //send email
                return user.Id;
            }
            return 0;
        }

        public void UpdateUser(UserModel userModel, int id)
        {
            var user = _baseService.GetById(id);
            string[] excludedProps = { "Id", "CreatedDate" };
            if (user != null)
            {
                userModel.Id = id;
                AweMediaUser awUser = new AweMediaUser();
                MappingProfile.MapAweMediaUserObject(userModel, awUser);
                awUser.CountryName = _countryService.GetById(userModel.CountryId).CountryName;
                var userId = _baseService.GetById(userModel.Id).UserId;
                _adb2cService.UpdateUser(JsonSerializer.Serialize(awUser),userId).Wait();
                _baseService.AddOrUpdate(MappingProfile.MapUserObject(userModel, user,""), id, excludedProps);
            }
        }
       
        public UserModel GetById(int id)
        {
            IQueryable<User> users = _baseService.GetAll("Country", "Role").AsQueryable();
            var user = users.Where(u => u.Id == id).FirstOrDefault();
            if (user != null)
            {
                return MappingProfile.MapUserModelObject(user);
            }
            else
            {
                return null;
            }
        }
       
        public bool IsUserDuplicate(UserModel userModel)
        {
            bool isDuplicateUserFound = false;
            if (userModel != null)
            {
                var user = _baseService.Where(a => a.Email.Equals(userModel.Email)).FirstOrDefault();
                if (user != null)
                {
                    isDuplicateUserFound = true;
                }
            }
            return isDuplicateUserFound;
        }
    }
}
