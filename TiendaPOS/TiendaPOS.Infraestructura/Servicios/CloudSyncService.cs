using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;

namespace TiendaPOS.Infraestructura.Servicios;

/// <summary>
/// Interfaz para el servicio de sincronización en la nube
/// </summary>
public interface ICloudSyncService
{
    Task<string> SubirArchivo(string rutaLocal, string nombreArchivo);
    Task<bool> VerificarSincronizacion(string archivoId);
}

/// <summary>
/// Implementación del servicio de sincronización con Google Drive
/// </summary>
public class CloudSyncService : ICloudSyncService
{
    private readonly DriveService _driveService;

    public CloudSyncService(string credencialesJson)
    {
        var credential = GoogleCredential.FromJson(credencialesJson)
            .CreateScoped(DriveService.ScopeConstants.DriveFile);

        _driveService = new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential
        });
    }

    public async Task<string> SubirArchivo(string rutaLocal, string nombreArchivo)
    {
        var fileMetadata = new Google.Apis.Drive.v3.Data.File()
        {
            Name = nombreArchivo
        };

        using var stream = new FileStream(rutaLocal, FileMode.Open);
        var request = _driveService.Files.Create(fileMetadata, stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        request.Fields = "id";
        var file = await request.UploadAsync();

        return file.Id;
    }

    public async Task<bool> VerificarSincronizacion(string archivoId)
    {
        try
        {
            var request = _driveService.Files.Get(archivoId);
            var file = await request.ExecuteAsync();
            return file != null;
        }
        catch
        {
            return false;
        }
    }
}
