using System.Windows;

namespace TiendaPOS.Presentacion;

/// <summary>
/// Interacción lógica para MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // Deshabilitar el cierre de la ventana con Alt+F4
        this.KeyDown += (s, e) =>
        {
            if (e.SystemKey == System.Windows.Input.Key.F4 &&
                (System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Alt) == System.Windows.Input.ModifierKeys.Alt)
            {
                e.Handled = true;
            }
        };

        // Configurar el inicio automático (requiere permisos de administrador)
        ConfigurarInicioAutomatico();
    }

    private void ConfigurarInicioAutomatico()
    {
        try
        {
            string startupPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            string shortcutPath = Path.Combine(startupPath, "TiendaPOS.lnk");

            // Crear acceso directo en la carpeta de inicio
            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
            IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutPath);
            shortcut.TargetPath = exePath;
            shortcut.Save();
        }
        catch (Exception ex)
        {
            MessageBox.Show("No se pudo configurar el inicio automático: " + ex.Message,
                          "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
