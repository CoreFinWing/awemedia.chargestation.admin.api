using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Models
{
    public class Promotion
    {
        public int Id { get; set; }
        public string PromotionDesc { get; set; }
        public int? BranchId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? PromotionType { get; set; }
        public string Mobile { get; set; }
        public string BranchName { get; set; }
    }
}
