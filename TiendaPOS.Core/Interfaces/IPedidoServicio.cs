using TiendaPOS.Core.Modelos;

namespace TiendaPOS.Core.Interfaces
{
    public interface IPedidoServicio
    {
        Task<IEnumerable<Pedido>> ObtenerTodosAsync();
        Task<Pedido> ObtenerPorIdAsync(int id);
        Task<Pedido> CrearPedidoAsync(Pedido pedido);
        Task<IEnumerable<Pedido>> ObtenerPorFechaAsync(DateTime fecha);
    }
}
