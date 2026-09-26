using EventaroApi.Services.Interfaces;

namespace EventaroApi.Services
{
    public class AlmacenadorArchivoLocal : IAlmacenadorArchivo
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _contextAccessor;

        public AlmacenadorArchivoLocal(IWebHostEnvironment env, IHttpContextAccessor contextAccessor)
        {
            this._env = env;
            this._contextAccessor = contextAccessor;
        }
        public async Task<string> EditarArchivo(byte[] contenido, string extension, string contenedor, string ruta, string contentType)
        {
            await EliminarArchivo(ruta, contenedor);
            return await GuardarArchivo(contenido, extension, contenedor, contentType);
        }

        public Task EliminarArchivo(string ruta, string contenedor)
        {
            if(ruta != null)
            {
                var nombreArchivo = Path.GetFileName(ruta);
                string folder = Path.Combine(_env.WebRootPath, contenedor, nombreArchivo);
                if (File.Exists(folder))
                {
                    File.Delete(folder);
                }
            }

            return Task.FromResult(0);
        }

        public async Task<string> GuardarArchivo(byte[] contenido, string extension, string contenedor, string contentType)
        {
            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(_env.WebRootPath, contenedor);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string ruta = Path.Combine(folder, nombreArchivo);
            await File.WriteAllBytesAsync(ruta, contenido);

            var url = $"{_contextAccessor.HttpContext.Request.Scheme}://{_contextAccessor.HttpContext.Request.Host}";
            var pathBD = Path.Combine(url, contenedor, nombreArchivo).Replace("\\", "/");

            return pathBD;
        }
    }
}
