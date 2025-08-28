using Microsoft.EntityFrameworkCore;
using System.Windows;
using TiendaPOS.Infraestructura.Data;
using TiendaPOS.Infraestructura.Servicios;
using Microsoft.Extensions.DependencyInjection;
using TiendaPOS.Aplicacion.Interfaces;
using TiendaPOS.Aplicacion.Servicios;

namespace TiendaPOS.Presentacion;

/// <summary>
/// Lógica de inicio de la aplicación
/// </summary>
public partial class App : Application
{
    private IServiceProvider _serviceProvider;

    protected override void OnStartup(StartupEventArgs e)
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();

        // Crear y mostrar la ventana principal
        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // Configurar DbContext
        services.AddDbContext<TiendaPOSDbContext>(options =>
            options.UseSqlite("Data Source=tiendapos.db"));

        // Registrar servicios
        services.AddScoped<IRappiService, RappiService>();
        services.AddScoped<IPedidosYaService, PedidosYaService>();
        services.AddScoped<ISunatService, SunatService>();
        services.AddScoped<ICloudSyncService>(sp => new CloudSyncService("credenciales.json"));
        services.AddScoped<IExcelExportService, ExcelExportService>();

        // Registrar ventanas
        services.AddTransient<MainWindow>();

        // Configurar background service para Rappi y PedidosYa
        services.AddHostedService<DeliveryBackgroundService>();
    }
}

/// <summary>
/// Servicio en segundo plano para sincronizar pedidos de delivery
/// </summary>
public class DeliveryBackgroundService : BackgroundService
{
    private readonly IRappiService _rappiService;
    private readonly IPedidosYaService _pedidosYaService;
    private readonly IServiceProvider _serviceProvider;

    public DeliveryBackgroundService(
        IServiceProvider serviceProvider,
        IRappiService rappiService,
        IPedidosYaService pedidosYaService)
    {
        _serviceProvider = serviceProvider;
        _rappiService = rappiService;
        _pedidosYaService = pedidosYaService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Sincronizar pedidos de Rappi
                var pedidosRappi = await _rappiService.ObtenerPedidosPendientes();
                foreach (var pedido in pedidosRappi)
                {
                    // TODO: Procesar pedido de Rappi
                }

                // Sincronizar pedidos de PedidosYa
                var pedidosPedidosYa = await _pedidosYaService.ObtenerPedidosPendientes();
                foreach (var pedido in pedidosPedidosYa)
                {
                    // TODO: Procesar pedido de PedidosYa
                }

                // Esperar 2 minutos antes de la siguiente sincronización
                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }
            catch (Exception ex)
            {
                // Log del error
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }
}
