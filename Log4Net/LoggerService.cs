using System;

namespace Utilities.Log4Net
{
    public class LoggerService<T>: ILoggerService where T:class
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(T));

        public void LogError(string message, Exception ex)
        {
            log.Error(message, ex);
        }

        public void LogInfo(string message, Exception ex)
        {
            log.Info(message, ex);
        }

        public void LogWarn(string message, Exception ex)
        {
            log.Warn(message, ex);
        }
    }

}
