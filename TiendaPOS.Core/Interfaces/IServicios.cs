using TiendaPOS.Core.Modelos;

namespace TiendaPOS.Core.Interfaces
{
    public interface IServicios
    {
        Task<List<Producto>> ObtenerProductos();
        Task<bool> GuardarPedido(Pedido pedido);
    }
}
