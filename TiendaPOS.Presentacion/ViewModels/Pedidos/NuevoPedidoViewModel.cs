using System.Collections.ObjectModel;
using System.Windows.Input;
using TiendaPOS.Core.Modelos;
using TiendaPOS.Presentacion.ViewModels.Base;

namespace TiendaPOS.Presentacion.ViewModels.Pedidos
{
    public class NuevoPedidoViewModel : ViewModelBase
    {
        private ObservableCollection<Producto> _productos;
        private ObservableCollection<ItemCarrito> _carrito;
        private decimal _total;
        private string _busqueda;

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

        private readonly IProductoServicio _productoServicio;
        private readonly IPedidoServicio _pedidoServicio;
        private ObservableCollection<Producto> _todosLosProductos;

        public NuevoPedidoViewModel(IProductoServicio productoServicio, IPedidoServicio pedidoServicio)
        {
            _productoServicio = productoServicio;
            _pedidoServicio = pedidoServicio;
            
            Carrito = new ObservableCollection<ItemCarrito>();
            
            // Inicializar comandos
            AgregarAlCarritoCommand = new RelayCommand<Producto>(AgregarAlCarrito);
            EliminarDelCarritoCommand = new RelayCommand<ItemCarrito>(EliminarDelCarrito);
            AumentarCantidadCommand = new RelayCommand<ItemCarrito>(AumentarCantidad);
            DisminuirCantidadCommand = new RelayCommand<ItemCarrito>(DisminuirCantidad);
            FinalizarPedidoCommand = new RelayCommand(FinalizarPedido, PuedeFinalizarPedido);

            CargarProductos();
        }

        private async void CargarProductos()
        {
            _todosLosProductos = new ObservableCollection<Producto>(await _productoServicio.ObtenerTodosAsync());
            Productos = new ObservableCollection<Producto>(_todosLosProductos);
        }

        private void FiltrarProductos()
        {
            if (string.IsNullOrWhiteSpace(Busqueda))
            {
                Productos = new ObservableCollection<Producto>(_todosLosProductos);
                return;
            }

            var productosFiltrados = _todosLosProductos
                .Where(p => p.Nombre.ToLower().Contains(Busqueda.ToLower()) || 
                           p.Codigo.ToLower().Contains(Busqueda.ToLower()))
                .ToList();

            Productos = new ObservableCollection<Producto>(productosFiltrados);
        }

        private void AgregarAlCarrito(Producto producto)
        {
            var itemExistente = Carrito.FirstOrDefault(i => i.Producto.Id == producto.Id);

            if (itemExistente != null)
            {
                itemExistente.Cantidad++;
            }
            else
            {
                Carrito.Add(new ItemCarrito
                {
                    Producto = producto,
                    Cantidad = 1
                });
            }

            ActualizarTotal();
        }

        private void EliminarDelCarrito(ItemCarrito item)
        {
            Carrito.Remove(item);
            ActualizarTotal();
        }

        private void AumentarCantidad(ItemCarrito item)
        {
            if (item.Cantidad < item.Producto.Stock)
            {
                item.Cantidad++;
                ActualizarTotal();
            }
        }

        private void DisminuirCantidad(ItemCarrito item)
        {
            if (item.Cantidad > 1)
            {
                item.Cantidad--;
                ActualizarTotal();
            }
            else
            {
                EliminarDelCarrito(item);
            }
        }

        private void ActualizarTotal()
        {
            Total = Carrito.Sum(item => item.Producto.Precio * item.Cantidad);
        }

        private bool PuedeFinalizarPedido()
        {
            return Carrito.Any();
        }

        private async void FinalizarPedido()
        {
            var pedido = new Pedido
            {
                Fecha = DateTime.Now,
                Items = Carrito.Select(i => new DetallePedido
                {
                    ProductoId = i.Producto.Id,
                    Cantidad = i.Cantidad,
                    PrecioUnitario = i.Producto.Precio
                }).ToList(),
                Total = Total
            };

            await _pedidoServicio.CrearPedidoAsync(pedido);

            // Actualizar el stock de los productos
            foreach (var item in Carrito)
            {
                item.Producto.Stock -= item.Cantidad;
                await _productoServicio.ActualizarProductoAsync(item.Producto);
            }

            // Limpiar el carrito
            Carrito.Clear();
            ActualizarTotal();
            
            // Recargar los productos para actualizar el stock mostrado
            CargarProductos();
        }
    }

    public class ItemCarrito : ViewModelBase
    {
        private Producto _producto;
        private int _cantidad;

        public Producto Producto
        {
            get => _producto;
            set => SetProperty(ref _producto, value);
        }

        public int Cantidad
        {
            get => _cantidad;
            set => SetProperty(ref _cantidad, value);
        }

        public decimal Subtotal => Producto?.Precio * Cantidad ?? 0;
    }
}
