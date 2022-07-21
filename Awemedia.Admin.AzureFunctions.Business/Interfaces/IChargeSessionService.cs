using Awemedia.Admin.AzureFunctions.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Interfaces
{
    public interface IChargeSessionService
    {
        IEnumerable<object> Get(BaseSearchFilter userSessionSearchFilter, out int totalRecords,string email);
        UserSession GetById(Guid Id);
        IEnumerable<UserSession> GetSuccessfulSessions();
    }
}
