using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.DAL.DataContracts
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }
    }
}
