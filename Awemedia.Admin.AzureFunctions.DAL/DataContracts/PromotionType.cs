using System;
using System.Collections.Generic;

namespace Awemedia.Admin.AzureFunctions.DAL.DataContracts
{
    public partial class PromotionType
    {
        public PromotionType()
        {
            Promotion = new HashSet<Promotion>();
        }

        public int Id { get; set; }
        public string PromotionType1 { get; set; }

        public ICollection<Promotion> Promotion { get; set; }
    }
}
