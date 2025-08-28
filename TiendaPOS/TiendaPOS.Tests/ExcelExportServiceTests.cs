using Xunit;
using Moq;
using TiendaPOS.Dominio.Entidades;
using TiendaPOS.Aplicacion.Interfaces;
using TiendaPOS.Infraestructura.Data;
using TiendaPOS.Infraestructura.Servicios;
using Microsoft.EntityFrameworkCore;

namespace TiendaPOS.Tests;

public class ExcelExportServiceTests
{
    [Fact]
    public async Task ExportarPedidos_DebeCrearArchivo()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TiendaPOSDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDB")
            .Options;

        using var context = new TiendaPOSDbContext(options);
        var mockCloudSync = new Mock<ICloudSyncService>();
        var service = new ExcelExportService(context, mockCloudSync.Object);

        // Crear algunos pedidos de prueba
        var cliente = new Cliente { Id = 1, Nombre = "Test Cliente" };
        context.Clientes.Add(cliente);

        var pedido = new Pedido
        {
            Id = 1,
            Numero = "P001",
            Fecha = DateTime.Today,
            ClienteId = cliente.Id,
            Cliente = cliente,
            Total = 100.00m,
            Estado = "Completado",
            TipoPedido = "Presencial"
        };
        context.Pedidos.Add(pedido);
        await context.SaveChangesAsync();

        // Act
        var rutaArchivo = await service.ExportarPedidos(DateTime.Today);

        // Assert
        Assert.True(File.Exists(rutaArchivo));
        Assert.EndsWith(".xlsx", rutaArchivo);
    }

    [Fact]
    public async Task SincronizarConNube_DebeRetornarTrue()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TiendaPOSDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDB2")
            .Options;

        using var context = new TiendaPOSDbContext(options);
        var mockCloudSync = new Mock<ICloudSyncService>();
        mockCloudSync.Setup(m => m.SubirArchivo(It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync("123");
        mockCloudSync.Setup(m => m.VerificarSincronizacion("123"))
                    .ReturnsAsync(true);

        var service = new ExcelExportService(context, mockCloudSync.Object);

        // Act
        var resultado = await service.SincronizarConNube("test.xlsx");

        // Assert
        Assert.True(resultado);
        mockCloudSync.Verify(m => m.SubirArchivo(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        mockCloudSync.Verify(m => m.VerificarSincronizacion("123"), Times.Once);
    }
}
