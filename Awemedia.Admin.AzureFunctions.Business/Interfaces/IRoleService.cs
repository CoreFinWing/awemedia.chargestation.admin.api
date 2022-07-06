using Awemedia.Admin.AzureFunctions.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Interfaces
{
    public interface IRoleService
    {
        IEnumerable<object> GetAll(bool isActive = true);

        RoleModel GetById(int id);
    }
}
