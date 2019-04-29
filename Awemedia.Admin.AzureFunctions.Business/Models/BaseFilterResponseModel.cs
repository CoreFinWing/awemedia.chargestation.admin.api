using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Models
{
    public class ChargeStationSearchFilter
    {
        public string PageNum { get; set; }
        public string ItemsPerPage { get; set; }
        public string SortBy { get; set; }
        public string Desc { get; set; }
        public string SearchText { get; set; }
    }
}
