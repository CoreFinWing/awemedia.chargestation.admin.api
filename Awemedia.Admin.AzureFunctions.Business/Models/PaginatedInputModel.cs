using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Models
{
    public class PaginatedInputModel
    {
        public IEnumerable<string> GroupingColumns { get; set; } = null;
    }
}
