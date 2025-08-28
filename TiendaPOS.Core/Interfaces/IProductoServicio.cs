using TiendaPOS.Core.Modelos;

namespace TiendaPOS.Core.Interfaces
{
    public interface IProductoServicio
    {
        Task<IEnumerable<Producto>> ObtenerTodosAsync();
        Task<Producto> ObtenerPorIdAsync(int id);
        Task<Producto> CrearProductoAsync(Producto producto);
        Task<Producto> ActualizarProductoAsync(Producto producto);
        Task<bool> EliminarProductoAsync(int id);
    }
}
