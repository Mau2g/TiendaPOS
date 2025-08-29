using System;
using System.Windows;
using TiendaPOS.Core.Interfaces;

namespace TiendaPOS.Presentacion.Services
{
    public class LoggerService : ILoggerService
    {
        public void LogError(string message, Exception? exception = null)
        {
            var fullMessage = exception != null ? $"{message}: {exception.Message}" : message;
            MessageBox.Show(fullMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void LogWarning(string message)
        {
            MessageBox.Show(message, "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void LogInfo(string message)
        {
            MessageBox.Show(message, "Información", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
