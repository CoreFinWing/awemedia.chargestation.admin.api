using Awemedia.Admin.AzureFunctions.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Interfaces
{
    public interface IChargeSessionService
    {
        IEnumerable<UserSession> Get(BaseSearchFilter userSessionSearchFilter, out int totalRecords);
        UserSession GetById(Guid Id);
    }
}
