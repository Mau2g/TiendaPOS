namespace TiendaPOS.Infraestructura.Servicios;

/// <summary>
/// Interfaz para el servicio de Rappi
/// </summary>
public interface IRappiService
{
    Task<IEnumerable<dynamic>> ObtenerPedidosPendientes();
    Task<bool> ActualizarEstadoPedido(string pedidoId, string estado);
}

/// <summary>
/// Implementación del servicio de Rappi
/// </summary>
public class RappiService : IRappiService
{
    // TODO: Implementar la lógica de integración con la API de Rappi
    public async Task<IEnumerable<dynamic>> ObtenerPedidosPendientes()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ActualizarEstadoPedido(string pedidoId, string estado)
    {
        throw new NotImplementedException();
    }
}
