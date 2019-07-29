using System;
using System.Collections.Generic;

namespace Awemedia.Admin.AzureFunctions.DAL.DataContracts
{
    public partial class SessionStatus
    {
        public SessionStatus()
        {
            UserSession = new HashSet<UserSession>();
        }

        public int Id { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }

        public ICollection<UserSession> UserSession { get; set; }
    }
}
