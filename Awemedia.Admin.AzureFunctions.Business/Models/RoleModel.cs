using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Models
{
    public class RoleModel : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }
    }
}
