using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TiendaPOS.Core.Modelos;
using TiendaPOS.Core.Interfaces;
using TiendaPOS.Presentacion.ViewModels.Base;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaPOS.Presentacion.ViewModels
{
    public class NuevoPedidoViewModel : ViewModelBase
    {
        private readonly IServicios _servicios;
        private readonly ILoggerService _logger;
        private ObservableCollection<Producto> _productos = new();
        private ObservableCollection<ItemCarrito> _carrito = new();
        private decimal _total;
        private string _busqueda = string.Empty;

        public NuevoPedidoViewModel(IServicios servicios, ILoggerService logger)
        {
            _servicios = servicios;
            _logger = logger;
            
            // Inicializar comandos
            AgregarAlCarritoCommand = new RelayCommand<Producto>(AgregarAlCarrito);
            EliminarDelCarritoCommand = new RelayCommand<ItemCarrito>(EliminarDelCarrito);
            AumentarCantidadCommand = new RelayCommand<ItemCarrito>(AumentarCantidad);
            DisminuirCantidadCommand = new RelayCommand<ItemCarrito>(DisminuirCantidad);
            FinalizarPedidoCommand = new RelayCommand(FinalizarPedido, PuedeFinalizarPedido);

            CargarProductos();
        }

        public ObservableCollection<Producto> Productos
        {
            get => _productos;
            set => SetProperty(ref _productos, value);
        }

        public ObservableCollection<ItemCarrito> Carrito
        {
            get => _carrito;
            set => SetProperty(ref _carrito, value);
        }

        public decimal Total
        {
            get => _total;
            set => SetProperty(ref _total, value);
        }

        public string Busqueda
        {
            get => _busqueda;
            set
            {
                if (SetProperty(ref _busqueda, value))
                {
                    FiltrarProductos();
                }
            }
        }

        public ICommand AgregarAlCarritoCommand { get; private set; }
        public ICommand EliminarDelCarritoCommand { get; private set; }
        public ICommand AumentarCantidadCommand { get; private set; }
        public ICommand DisminuirCantidadCommand { get; private set; }
        public ICommand FinalizarPedidoCommand { get; private set; }

        private async void CargarProductos()
        {
            try
            {
                var productos = await _servicios.ObtenerProductos();
                Productos = new ObservableCollection<Producto>(productos);
            }
            catch (Exception ex)
            {
                // Manejar el error
                _logger.LogError("Error al cargar productos", ex);
            }
        }

        private void FiltrarProductos()
        {
            if (string.IsNullOrWhiteSpace(Busqueda))
            {
                CargarProductos();
                return;
            }

            var productosFiltrados = _productos.Where(p =>
                p.Nombre.Contains(Busqueda, StringComparison.OrdinalIgnoreCase) ||
                p.Codigo.Contains(Busqueda, StringComparison.OrdinalIgnoreCase))
                .ToList();

            Productos = new ObservableCollection<Producto>(productosFiltrados);
        }

        private void AgregarAlCarrito(Producto producto)
        {
            if (producto == null) return;

            var itemExistente = Carrito.FirstOrDefault(i => i.Producto.Id == producto.Id);
            if (itemExistente != null)
            {
                itemExistente.Cantidad++;
                ActualizarTotal();
                return;
            }

            var nuevoItem = new ItemCarrito
            {
                Producto = producto,
                Cantidad = 1,
                Subtotal = producto.Precio
            };

            Carrito.Add(nuevoItem);
            ActualizarTotal();
        }

        private void EliminarDelCarrito(ItemCarrito item)
        {
            if (item == null) return;

            Carrito.Remove(item);
            ActualizarTotal();
        }

        private void AumentarCantidad(ItemCarrito item)
        {
            if (item == null) return;

            item.Cantidad++;
            item.Subtotal = item.Producto.Precio * item.Cantidad;
            ActualizarTotal();
        }

        private void DisminuirCantidad(ItemCarrito item)
        {
            if (item == null || item.Cantidad <= 1) return;

            item.Cantidad--;
            item.Subtotal = item.Producto.Precio * item.Cantidad;
            ActualizarTotal();
        }

        private void ActualizarTotal()
        {
            Total = Carrito.Sum(item => item.Subtotal);
        }

        private async void FinalizarPedido()
        {
            if (!PuedeFinalizarPedido())
                return;

            try
            {
                var pedido = new Pedido
                {
                    FechaCreacion = DateTime.Now,
                    Estado = EstadoPedido.Nuevo,
                    MetodoPago = MetodoPago.Efectivo, // Por defecto
                    Items = Carrito.Select(item => new DetallePedido
                    {
                        ProductoId = item.Producto.Id,
                        Cantidad = item.Cantidad,
                        PrecioUnitario = item.Producto.Precio
                        // Subtotal se calcula internamente en DetallePedido
                    }).ToList(),
                    Total = Total
                };

                await _servicios.GuardarPedido(pedido);
                
                // Limpiar carrito después de guardar
                Carrito.Clear();
                ActualizarTotal();
            }
            catch (Exception ex)
            {
                // Manejar el error
                _logger.LogError("Error al finalizar pedido", ex);
            }
        }

        private bool PuedeFinalizarPedido()
        {
            return Carrito.Any();
        }
    }
}
