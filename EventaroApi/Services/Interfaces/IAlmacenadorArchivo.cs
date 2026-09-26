namespace EventaroApi.Services.Interfaces
{
    public interface IAlmacenadorArchivo
    {
        Task<string> GuardarArchivo(byte[] contenido, string extension, string contenedor, string contentType);
        Task<string> EditarArchivo(byte[] contenido, string extension, string contenedor, string ruta, string contentType);
        Task EliminarArchivo(string ruta, string contenedor);
    }
}
