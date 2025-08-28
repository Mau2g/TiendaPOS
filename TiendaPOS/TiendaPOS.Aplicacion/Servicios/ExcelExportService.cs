using ClosedXML.Excel;
using TiendaPOS.Aplicacion.Interfaces;
using TiendaPOS.Dominio.Entidades;
using TiendaPOS.Infraestructura.Data;
using TiendaPOS.Infraestructura.Servicios;

namespace TiendaPOS.Aplicacion.Servicios;

/// <summary>
/// Implementación del servicio de exportación a Excel
/// </summary>
public class ExcelExportService : IExcelExportService
{
    private readonly TiendaPOSDbContext _context;
    private readonly ICloudSyncService _cloudSyncService;

    public ExcelExportService(TiendaPOSDbContext context, ICloudSyncService cloudSyncService)
    {
        _context = context;
        _cloudSyncService = cloudSyncService;
    }

    public async Task<string> ExportarPedidos(DateTime fecha)
    {
        var pedidos = _context.Pedidos
            .Where(p => p.Fecha.Date == fecha.Date)
            .ToList();

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Pedidos");

        // Configurar encabezados
        worksheet.Cell("A1").Value = "Número";
        worksheet.Cell("B1").Value = "Fecha";
        worksheet.Cell("C1").Value = "Cliente";
        worksheet.Cell("D1").Value = "Total";
        worksheet.Cell("E1").Value = "Estado";
        worksheet.Cell("F1").Value = "Tipo";

        // Llenar datos
        int row = 2;
        foreach (var pedido in pedidos)
        {
            worksheet.Cell($"A{row}").Value = pedido.Numero;
            worksheet.Cell($"B{row}").Value = pedido.Fecha;
            worksheet.Cell($"C{row}").Value = pedido.Cliente?.Nombre ?? "N/A";
            worksheet.Cell($"D{row}").Value = pedido.Total;
            worksheet.Cell($"E{row}").Value = pedido.Estado;
            worksheet.Cell($"F{row}").Value = pedido.TipoPedido;
            row++;
        }

        // Guardar archivo
        string directorio = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes");
        if (!Directory.Exists(directorio))
            Directory.CreateDirectory(directorio);

        string rutaArchivo = Path.Combine(directorio, $"Pedidos_{fecha:yyyyMMdd}.xlsx");
        workbook.SaveAs(rutaArchivo);

        return rutaArchivo;
    }

    public async Task<bool> SincronizarConNube(string rutaArchivo)
    {
        try
        {
            string nombreArchivo = Path.GetFileName(rutaArchivo);
            string archivoId = await _cloudSyncService.SubirArchivo(rutaArchivo, nombreArchivo);
            return await _cloudSyncService.VerificarSincronizacion(archivoId);
        }
        catch
        {
            return false;
        }
    }
}
