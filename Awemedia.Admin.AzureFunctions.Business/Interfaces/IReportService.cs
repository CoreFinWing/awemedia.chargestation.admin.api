using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Interfaces
{
    public interface IReportService
    {
        void Create();
        void Send();
    }
}
