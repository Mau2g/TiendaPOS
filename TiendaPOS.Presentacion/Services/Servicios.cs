using TiendaPOS.Core.Modelos;
using TiendaPOS.Core.Interfaces;

namespace TiendaPOS.Presentacion.Services
{
    public class Servicios : IServicios
    {
        public async Task<List<Producto>> ObtenerProductos()
        {
            // Implementación temporal para pruebas
            await Task.Delay(100); // Simular latencia
            return new List<Producto>
            {
                new Producto { Id = 1, Codigo = "P001", Nombre = "Producto 1", Precio = 10.50m, Descripcion = "Descripción del producto 1", ImagenUrl = "/images/p1.jpg" },
                new Producto { Id = 2, Codigo = "P002", Nombre = "Producto 2", Precio = 15.75m, Descripcion = "Descripción del producto 2", ImagenUrl = "/images/p2.jpg" },
                // Agregar más productos según sea necesario
            };
        }

        public async Task<bool> GuardarPedido(Pedido pedido)
        {
            // Implementación temporal para pruebas
            await Task.Delay(100); // Simular latencia
            return true;
        }
    }
}
