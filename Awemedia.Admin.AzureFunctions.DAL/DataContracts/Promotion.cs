using System;
using System.Collections.Generic;

namespace Awemedia.Admin.AzureFunctions.DAL.DataContracts
{
    public partial class Promotion
    {
        public int Id { get; set; }
        public string PromotionDesc { get; set; }
        public int? PromotionType { get; set; }
        public int? BranchId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        public Branch Branch { get; set; }
    }
}
