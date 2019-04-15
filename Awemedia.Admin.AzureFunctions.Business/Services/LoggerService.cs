using Awemedia.Admin.AzureFunctions.Business.Interfaces;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Services
{
    public class LoggerService : ILoggerService
    {
        private static NLog.ILogger logger = LogManager.GetCurrentClassLogger();

        public LoggerService()
        {
        }

        public void Information(string message)
        {
            logger.Info(message);
        }

        public void Warning(string message)
        {
            logger.Warn(message);
        }

        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Error(string message)
        {
            logger.Error(message);
        }
    }
}
