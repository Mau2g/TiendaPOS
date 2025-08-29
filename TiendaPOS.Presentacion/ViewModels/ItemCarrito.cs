using TiendaPOS.Core.Modelos;

namespace TiendaPOS.Presentacion.ViewModels
{
    public class ItemCarrito
    {
        public Producto Producto { get; set; } = null!;
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
    }
}
