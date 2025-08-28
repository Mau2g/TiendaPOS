namespace TiendaPOS.Dominio.Entidades;

/// <summary>
/// Representa un usuario del sistema
/// </summary>
public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string NombreUsuario { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty; // Admin, Cajero, etc.
    public bool Activo { get; set; }
    public DateTime FechaRegistro { get; set; }
}
