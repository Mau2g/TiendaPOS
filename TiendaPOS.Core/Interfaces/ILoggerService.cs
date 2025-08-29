using System;

namespace TiendaPOS.Core.Interfaces
{
    public interface ILoggerService
    {
        void LogError(string message, Exception? exception = null);
        void LogWarning(string message);
        void LogInfo(string message);
    }
}
