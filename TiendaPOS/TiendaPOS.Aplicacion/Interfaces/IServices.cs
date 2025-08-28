using TiendaPOS.Dominio.Entidades;

namespace TiendaPOS.Aplicacion.Interfaces;

/// <summary>
/// Interfaz para el servicio de gestión de pedidos
/// </summary>
public interface IPedidoService
{
    Task<Pedido> CrearPedido(Pedido pedido);
    Task<bool> ActualizarEstadoPedido(int pedidoId, string estado);
    Task<Pedido?> ObtenerPedido(int pedidoId);
    Task<IEnumerable<Pedido>> ObtenerPedidosPorFecha(DateTime fecha);
}

/// <summary>
/// Interfaz para el servicio de facturación
/// </summary>
public interface IFacturacionService
{
    Task<Factura> GenerarFactura(int pedidoId);
    Task<string> GenerarPDF(int facturaId);
    Task<bool> EnviarASunat(int facturaId);
}

/// <summary>
/// Interfaz para el servicio de exportación a Excel
/// </summary>
public interface IExcelExportService
{
    Task<string> ExportarPedidos(DateTime fecha);
    Task<bool> SincronizarConNube(string rutaArchivo);
}

/// <summary>
/// Interfaz para el servicio de autenticación
/// </summary>
public interface IAuthService
{
    Task<bool> ValidarCredenciales(string usuario, string password);
    Task<Usuario?> ObtenerUsuario(string usuario);
}
