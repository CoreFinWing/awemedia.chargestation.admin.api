using System;
using System.Collections.Generic;

namespace Awemedia.Admin.AzureFunctions.DAL.DataContracts
{
    public partial class SessionType
    {
        public SessionType()
        {
            UserSession = new HashSet<UserSession>();
        }

        public int Id { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }

        public ICollection<UserSession> UserSession { get; set; }
    }
}
