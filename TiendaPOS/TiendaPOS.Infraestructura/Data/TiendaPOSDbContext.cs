using Microsoft.EntityFrameworkCore;
using TiendaPOS.Dominio.Entidades;

namespace TiendaPOS.Infraestructura.Data;

/// <summary>
/// Contexto de base de datos principal de la aplicación
/// </summary>
public class TiendaPOSDbContext : DbContext
{
    public TiendaPOSDbContext(DbContextOptions<TiendaPOSDbContext> options)
        : base(options)
    {
    }

    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<DetallePedido> DetallesPedido => Set<DetallePedido>();
    public DbSet<Factura> Facturas => Set<Factura>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuraciones de las entidades
        modelBuilder.Entity<Pedido>()
            .HasOne(p => p.Cliente)
            .WithMany()
            .HasForeignKey(p => p.ClienteId);

        modelBuilder.Entity<DetallePedido>()
            .HasOne(d => d.Pedido)
            .WithMany(p => p.Detalles)
            .HasForeignKey(d => d.PedidoId);

        modelBuilder.Entity<DetallePedido>()
            .HasOne(d => d.Producto)
            .WithMany()
            .HasForeignKey(d => d.ProductoId);

        modelBuilder.Entity<Factura>()
            .HasOne(f => f.Cliente)
            .WithMany()
            .HasForeignKey(f => f.ClienteId);

        modelBuilder.Entity<Factura>()
            .HasOne(f => f.Pedido)
            .WithMany()
            .HasForeignKey(f => f.PedidoId);
    }
}
