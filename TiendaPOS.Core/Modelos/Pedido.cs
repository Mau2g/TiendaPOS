namespace TiendaPOS.Core.Modelos
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public List<DetallePedido> Items { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }
        public string MetodoPago { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public Pedido()
        {
            Items = new List<DetallePedido>();
            Estado = "Pendiente";
            FechaCreacion = DateTime.Now;
        }
    }
}
