using Awemedia.Admin.AzureFunctions.Business.Infrastructure;
using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Awemedia.Admin.AzureFunctions.Business.Models;
using Awemedia.Admin.AzureFunctions.DAL.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Awemedia.Admin.AzureFunctions.Business.Services
{
    public class RoleService : IRoleService
    {
        private readonly IBaseService<Role> _baseService;

        public RoleService(IBaseService<Role> baseService)
        {
            _baseService = baseService;
        }

        public IEnumerable<object> GetAll(bool isOwner = false, bool isActive = true)
        {
            if (!isOwner)
                return _baseService.GetAll().Where(t => t.IsActive == isActive && t.Name!="owner").Select(t => MappingProfile.MapRoleModelObject(t)).ToList();
            else
                return _baseService.GetAll().Where(t => t.IsActive == isActive).Select(t => MappingProfile.MapRoleModelObject(t)).ToList();
        }

        public RoleModel GetById(int id)
        {
            IQueryable<Role> roles = _baseService.GetAll().AsQueryable();
            var role = roles.Where(u => u.Id == id).FirstOrDefault();
            if (role != null)
            {
                return MappingProfile.MapRoleModelObject(role);
            }
            else
            {
                return null;
            }
        }
    }
}