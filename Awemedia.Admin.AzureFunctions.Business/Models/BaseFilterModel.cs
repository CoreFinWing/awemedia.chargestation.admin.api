using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Models
{
    public class BaseSearchFilter
    {
        public string Start { get; set; }
        public string Size { get; set; }
        public string Order { get; set; }
        public string Dir { get; set; }
        public string Search { get; set; }
    }
}
