namespace TiendaPOS.Infraestructura.Servicios;

/// <summary>
/// Interfaz para el servicio de SUNAT
/// </summary>
public interface ISunatService
{
    Task<string> EnviarFactura(string xml);
    Task<string> ConsultarEstado(string ticket);
}

/// <summary>
/// Implementación del servicio de SUNAT
/// </summary>
public class SunatService : ISunatService
{
    // TODO: Implementar la lógica de integración con los servicios web de SUNAT
    public async Task<string> EnviarFactura(string xml)
    {
        throw new NotImplementedException();
    }

    public async Task<string> ConsultarEstado(string ticket)
    {
        throw new NotImplementedException();
    }
}
