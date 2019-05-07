using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Models
{
    public class BaseChargeOptionsFilterResponse
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
