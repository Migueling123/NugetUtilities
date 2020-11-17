using System;

namespace Utilities.Log4Net
{
    public interface ILoggerService
    {
        
        void LogWarn(string message, Exception ex);
        void LogError(string message, Exception ex);
        void LogInfo(string message, Exception ex);
    }
}
