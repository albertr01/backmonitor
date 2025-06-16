using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;
using System.Text.Json;
using WebApplication1.Models.DB;
using Microsoft.SqlServer.Server;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly MonitoreopyaContext _context;
        private readonly ILogger<MaestroParametrosController> _logger;

        public FileUploadController(MonitoreopyaContext db, ILogger<MaestroParametrosController> logger)
        {
            _context = db;
            _logger = logger;
        }

        public class ArchivoEliminarRequest
        {
            public required List<ArchivoEliminar> FilesToDelete { get; set; }
        }

        public class ArchivoEliminar
        {
            public required string Path { get; set; } // Ruta completa del archivo a eliminar
        }


        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormCollection form)
        {

            // Obtén el valor del parámetro 'id' del FormData
            if (!int.TryParse(form["id"], out int id))
            {
                return BadRequest("El parámetro 'id' es requerido y debe ser un número válido.");
            }

            var dbSet = _context.AlertaTransacciones ?? throw new BadRequestException("No se encontró la propiedad AlertaTransacciones en el contexto.");

            // Busca la entidad existente por ID
            var existingEntity = await dbSet.FirstOrDefaultAsync(e => e.IdTransaccion == id)
                ?? throw new NotFoundException($"No se encontró la alerta con ID: {id}");

            var files = form.Files;

            if (files == null || files.Count == 0)
            {
                return BadRequest("No se encuentran los archivos subidos.");
            }

            try
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "ArchivosCargados", "Upload" + id);

                // Crear el directorio si no existe
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uploadedFiles = new List<string>();

                foreach (var file in files)
                {
                    // Asegúrate de que el nombre del archivo sea seguro
                    var safeFileName = Path.GetFileName(file.FileName);

                    // Construir la ruta completa con el nombre original del archivo
                    var filePath = Path.Combine(uploadsFolder, safeFileName);

                    // Guardar el archivo en el servidor
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    uploadedFiles.Add(filePath);
                }

                existingEntity.ArchivoAdjunto = uploadsFolder;

                _context.Entry(existingEntity).Property(e => e.ArchivoAdjunto).IsModified = true;

                // Registra el cambio en los logs
                var userName = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

                GuardarLog(userName, userRole, $"PUT: api/Upload", "AlertaTransacciones", "APICONTROLLER",
                    $"Registro actualizado: ArchivoAdjunto={uploadsFolder}, FechaAsignacion={DateTime.Now}");

                // Guarda los cambios
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Se completó la subida de los archivos", FilePaths = uploadedFiles });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private void GuardarLog(string username, string rol, string evento, string tablaDB, string Operación, string Detalle_registro)
        {
            //TODO: Guardar traza de Acceso en auditoria
            var log = new LogsAuditorium
            {
                IdUsuario = username,
                IdOperacion = Operación!,
                TpUsuario = rol,
                FechaConexion = DateTime.Now,
                FechaIngreso = DateTime.Now,
                IpDispositivo = GetUserIPAddress(),
                BdTabla = tablaDB!,
                TpEvento = evento,
                Modulo = "PARAMETROS",
                DetalleRegistro = Detalle_registro
            };
            _context.LogsAuditoria.Add(log);
            _context.SaveChanges();

        }

        private string GetUserIPAddress()
        {
            // 1. Desde el Request (puede ser null si está detrás de un proxy)
            string? ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            // 2. Desde los headers (si estás detrás de un proxy)
            if (string.IsNullOrEmpty(ipAddress) && Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                ipAddress = Request.Headers["X-Forwarded-For"];
            }

            // 3. Si usas Kestrel y no estás detrás de un proxy
            if (string.IsNullOrEmpty(ipAddress) && Request.HttpContext.Connection.LocalIpAddress != null)
            {
                ipAddress = Request.HttpContext.Connection.LocalIpAddress.ToString();
            }

            return ipAddress ?? "Unknown";
        }

        [HttpGet("{idTransaccion}")]
        public async Task<IActionResult> GetArchivosPrecargados(string idTransaccion)
        {
            try
            {
                // Obtén el valor del parámetro 'id' del FormData
                if (!int.TryParse(idTransaccion, out int id))
                {
                    return BadRequest("El parámetro 'id' es requerido y debe ser un número válido.");
                }

                var dbSet = _context.AlertaTransacciones ?? throw new BadRequestException("No se encontró la propiedad AlertaTransacciones en el contexto.");

                // Busca la entidad existente por ID
                var existingEntity = await dbSet.FirstOrDefaultAsync(e => e.IdTransaccion == id)
                    ?? throw new NotFoundException($"No se encontró la alerta con ID: {id}");

                // Ruta de la carpeta específica para la alerta
                string? carpetaAlerta = existingEntity.ArchivoAdjunto;

                // Verifica si la carpeta existe
                if (string.IsNullOrEmpty(carpetaAlerta) || !Directory.Exists(carpetaAlerta))
                {
                    return NotFound(new { message = "No se encontraron archivos para esta alerta." });
                }

                // Obtiene los archivos de la carpeta
                var archivos = Directory.GetFiles(carpetaAlerta)
                    .Select(file => new
                    {
                        Nombre = Path.GetFileName(file),
                        Tamano = new FileInfo(file).Length,
                        Ruta = Url.Content($"~/ArchivosCargados/Upload{idTransaccion}/{Path.GetFileName(file)}") // Ruta accesible desde el cliente
                    })
                    .ToList();

                return Ok(archivos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al obtener los archivos.", error = ex.Message });
            }
        }

        [HttpPost("borraArchivos")]
        public async Task<IActionResult> DeleteArchivos([FromBody] ArchivoEliminarRequest request)
        {
            if (request == null || request.FilesToDelete == null || request.FilesToDelete.Count == 0)
            {
                return BadRequest(new { message = "No se proporcionaron archivos para eliminar." });
            }

            foreach (var file in request.FilesToDelete)
            {
                try
                {
                    var absolutePath = Path.Combine(Directory.GetCurrentDirectory(), file.Path.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

                    if (System.IO.File.Exists(absolutePath))
                    {
                        // Use Task.Run to perform file deletion asynchronously
                        await Task.Run(() => System.IO.File.Delete(absolutePath));
                    }
                    else
                    {
                        return NotFound(new { message = $"El archivo {file.Path} no existe." });
                    }
                }
                catch (IOException ex)
                {
                    return StatusCode(500, new { message = $"Error al eliminar el archivo {file.Path}: {ex.Message}" });
                }
            }

            return Ok(new { message = "Archivos eliminados correctamente." });
        }

    }
}
