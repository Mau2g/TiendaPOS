namespace TiendaPOS.Dominio.Entidades;

/// <summary>
/// Representa un pedido en el sistema
/// </summary>
public class Pedido
{
    public int Id { get; set; }
    public string Numero { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }
    public decimal Total { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string TipoPedido { get; set; } = string.Empty; // Presencial, Rappi, PedidosYa
    public string? ReferenciaPedidoExterno { get; set; } // ID del pedido en Rappi o PedidosYa
    public List<DetallePedido> Detalles { get; set; } = new();
}

/// <summary>
/// Representa el detalle de un pedido
/// </summary>
public class DetallePedido
{
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public Pedido? Pedido { get; set; }
    public int ProductoId { get; set; }
    public Producto? Producto { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
}
