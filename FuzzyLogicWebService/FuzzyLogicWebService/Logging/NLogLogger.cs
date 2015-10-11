using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace FuzzyLogicWebService.Logging
{
    public class NLogLogger : ILogger
    {
        private Logger logger;

        public NLogLogger()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        public void Info(string message)
        {
            logger.Info(message);
        }

        public void Warn(string message)
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

        public void Error(Exception x)
        {
            Error(LogExceptionMessage(x));
        }

        public void Error(string message, Exception x)
        {
            Error(message);
            Error(x);
        }

        private string LogExceptionMessage(Exception exc)
        {
            Exception exceptionToBeLogged = exc.InnerException != null ? exc.InnerException : exc;
            string errorMsg = Environment.NewLine + "Message: " + exceptionToBeLogged.Message;
            errorMsg += Environment.NewLine + "Source: " + exceptionToBeLogged.Source;
            errorMsg += Environment.NewLine + "Stack Trace: " + exceptionToBeLogged.StackTrace;
            errorMsg += Environment.NewLine + "TargetSite: " + exceptionToBeLogged.TargetSite;
            return errorMsg;
        }
    }
}
