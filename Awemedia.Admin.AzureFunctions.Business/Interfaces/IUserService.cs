using Awemedia.Admin.AzureFunctions.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Interfaces
{
    public interface IUserService
    {
        IEnumerable<object> Get(BaseSearchFilter userSearchFilter, out int totalRecords, bool isActive = true);
        int AddUser(UserModel userModel, int id =0);
        void UpdateUser(UserModel userModel, int id);
        bool IsUserDuplicate(UserModel userModel);
        UserModel GetById(int id);
    }
}
