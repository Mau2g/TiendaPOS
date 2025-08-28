namespace TiendaPOS.Dominio.Entidades;

/// <summary>
/// Representa una factura en el sistema
/// </summary>
public class Factura
{
    public int Id { get; set; }
    public string Serie { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }
    public int PedidoId { get; set; }
    public Pedido? Pedido { get; set; }
    public decimal SubTotal { get; set; }
    public decimal IGV { get; set; }
    public decimal Total { get; set; }
    public string Estado { get; set; } = string.Empty; // Emitida, Anulada, etc.
    public string? RespuestaSunat { get; set; } // Respuesta del servicio de SUNAT
}
