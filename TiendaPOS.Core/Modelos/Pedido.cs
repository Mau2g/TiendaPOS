namespace TiendaPOS.Core.Modelos
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; }
        public required List<DetallePedido> Items { get; set; }
        public decimal Total { get; set; }
        public required EstadoPedido Estado { get; set; }
        public required MetodoPago MetodoPago { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
