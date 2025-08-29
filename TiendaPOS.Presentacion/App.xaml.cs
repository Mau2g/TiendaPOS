using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TiendaPOS.Core.Interfaces;
using TiendaPOS.Presentacion.ViewModels;
using TiendaPOS.Presentacion.Services;

namespace TiendaPOS.Presentacion
{
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IServicios, Servicios>();
            services.AddSingleton<ILoggerService, LoggerService>();
            services.AddTransient<NuevoPedidoViewModel>();

            services.AddSingleton<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow?.Show();
        }
    }
}
