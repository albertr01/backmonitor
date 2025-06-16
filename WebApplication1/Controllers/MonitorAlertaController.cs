using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;
using System.Text.Json;
using WebApplication1.Models.DB;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Connections.BD;
using WebApplication1.Utils;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonitorAlertaController : ControllerBase
    {
        private readonly MonitoreopyaContext _context;
        private readonly ILogger<MaestroParametrosController> _logger;

        public MonitorAlertaController(MonitoreopyaContext db, ILogger<MaestroParametrosController> logger)
        {
            _context = db;
            _logger = logger;
        }


        [HttpGet("GetAlertas/{type}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<object>>> GetAlertas(string type)
        {
            IDictionary<string, object> data = new Dictionary<string, object>();

            try
            {
                var userName = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

                Log.Information("Inicia Metodo GetAlertas");

                // Verifica si la tabla AlertaTransacciones existe en el contexto
                var dbSet = _context.AlertaTransacciones ?? throw new BadRequestException("No se encontró la propiedad AlertaTransacciones en el contexto.");

                // Verifica si la tabla AlertaTransacciones tiene datos
                IQueryable<AlertaTransaccione> query = dbSet;

                if (type == "pendientesAsignar")
                {
                    query = query.Where(alerta => alerta.EstatusEjecucion == 0);
                }
                else if (type == "pendientesAtender")
                {
                    query = query.Where(alerta => alerta.EstatusEjecucion == 1);
                }
                else if (type == "enProcesoAnalisis")
                {
                    query = query.Where(alerta => alerta.EstatusEjecucion == 2);
                }
                else if (type == "atendidas")
                {
                    query = query.Where(alerta => alerta.EstatusEjecucion == 3);
                }
                else if (type == "distribuidasAutomaticamente")
                {
                    query = query.Where(alerta => alerta.EstatusEjecucion == 0 && alerta.TipoAlerta == "Automática");
                }
                else if (type == "enSeguimiento")
                {
                    query = query.Where(alerta => alerta.EstatusEjecucion == 3 && alerta.EstatusAlerta == "Seguimiento");
                }
                else
                {
                    return BadRequest("El tipo de alerta proporcionado no es válido.");
                }

                if (userRole == "Analista de Cumplimiento")
                {
                    query = query.Where(alerta => alerta.AnalistaAsignado == userName);
                }

                // Ejecuta la consulta y devuelve los resultados
                var result = await query.ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return ex switch
                {
                    BadRequestException => BadRequest(ex.Message),
                    NotFoundException => NotFound(ex.Message),
                    _ => StatusCode(500, ex.Message)
                };
            }
        }

        [HttpGet("GetAlertasCount")]
        [Authorize]
        public async Task<ActionResult<object>> GetAlertasCount()
        {
            try
            {
                _logger.LogInformation("Inicia Método GetAlertasCount");

                var userName = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;

                var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

                // Verifica si la tabla AlertaTransacciones existe en el contexto
                var dbSet = _context.AlertaTransacciones ?? throw new BadRequestException("No se encontró la propiedad AlertaTransacciones en el contexto.");

                IQueryable<AlertaTransaccione> query = dbSet;

                if (userRole == "Analista de Cumplimiento")
                {
                    query = query.Where(alerta => alerta.AnalistaAsignado == userName);
                }

                
                var pendientesAsignar = await query.CountAsync(alerta => alerta.EstatusEjecucion == 0);
                var pendientesAtender = await query.CountAsync(alerta => alerta.EstatusEjecucion == 1);
                var enProcesoAnalisis = await query.CountAsync(alerta => alerta.EstatusEjecucion == 2);
                var atendidasCount = await query.CountAsync(alerta => alerta.EstatusEjecucion == 3);
                var distribuidasAutomaticamenteCount = await query.CountAsync(alerta => alerta.EstatusEjecucion == 0 && alerta.TipoAlerta == "Automática");
                var enSeguimientoCount = await query.CountAsync(alerta => alerta.EstatusEjecucion == 3 && alerta.EstatusAlerta == "Seguimiento");

                
                var result = new
                {
                    pendientesAsignar,
                    pendientesAtender,
                    enProcesoAnalisis,
                    atendidasCount,
                    distribuidasAutomaticamenteCount,
                    enSeguimientoCount,
                    userRole,
                    userName
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return ex switch
                {
                    BadRequestException => BadRequest(ex.Message),
                    NotFoundException => NotFound(ex.Message),
                    _ => StatusCode(500, ex.Message)
                };
            }
        }

        [HttpPut("UpdateAnalistaAsignado")]
        [Authorize]
        public async Task<IActionResult> UpdateAnalistaAsignado(
            [FromQuery] int id,
            [FromQuery] string analista)
        {
            try
            {
                _logger.LogInformation("Inicia Método UpdateAnalistaAsignado");

                // Verifica si la tabla AlertaTransacciones existe en el contexto
                var dbSet = _context.AlertaTransacciones ?? throw new BadRequestException("No se encontró la propiedad AlertaTransacciones en el contexto.");

                // Busca la entidad existente por ID
                var existingEntity = await dbSet.FirstOrDefaultAsync(e => e.IdTransaccion == id)
                    ?? throw new NotFoundException($"No se encontró la alerta con ID: {id}");

                // Actualiza solo los campos necesarios
                existingEntity.AnalistaAsignado = analista;
                existingEntity.FechaAsignacion = DateTime.Now;
                existingEntity.EstatusEjecucion = 1;

                // Marca los campos como modificados
                _context.Entry(existingEntity).Property(e => e.AnalistaAsignado).IsModified = true;
                _context.Entry(existingEntity).Property(e => e.FechaAsignacion).IsModified = true;

                // Guarda los cambios
                await _context.SaveChangesAsync();

                // Registra el cambio en los logs
                var userName = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
                GuardarLog(userName, userRole, $"PUT: api/UpdateAnalistaAsignado", "AlertaTransacciones", "APICONTROLLER",
                    $"Registro actualizado: AnalistaAsignado={analista}, FechaAsignacion={DateTime.Now}");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return ex switch
                {
                    BadRequestException => BadRequest(ex.Message),
                    NotFoundException => NotFound(ex.Message),
                    _ => StatusCode(500, ex.Message)
                };
            }
        }

        [HttpPost("PostGuardarAlertaManual")]
        [Authorize]
        public async Task<IActionResult> PostGuardarAlertaManual(
            [FromBody] AlertaTransaccione entity)
        {
            try
            {
                Log.Information("Inicia Metodo POSTListasRestrictivas");
                var userName = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

                // Verifica si la tabla AlertaTransacciones existe en el contexto
                var dbSet = _context.AlertaTransacciones ?? throw new BadRequestException("No se encontró la propiedad AlertaTransacciones en el contexto.");

                entity.EstatusEjecucion = 0;

                entity.TipoAlerta = "Manual";

                entity.EstatusAlerta = "Pendiente";


                if (entity.AnalistaAsignado != "")
                {
                    entity.FechaAsignacion = DateTime.Now;
                    entity.EstatusEjecucion = 1;
                }

                if (entity == null)
                {
                    throw new BadHttpRequestException("Datos de entidad no válidos.");
                }

                _context.Add(entity);

                try
                {
                    await _context.SaveChangesAsync();
                    GuardarLog(userName, userRole, $"POST: api/PostGuardarAlertaManual", "AlertaTransacciones", "APICONTROLLER", $"Registro: {JsonSerializer.Serialize(entity)}");
                }
                catch (DbUpdateException)
                {
                    throw new Exception("Error al guardar la entidad");
                }

                // Devolver el ID del registro recién insertado
                return Ok(new { Message = "Registro creado exitosamente", Id = entity.IdTransaccion });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return ex switch
                {
                    BadRequestException => BadRequest(ex.Message),
                    NotFoundException => NotFound(ex.Message),
                    _ => StatusCode(500, ex.Message)
                };
            }
        }

        [HttpPut("UpdateAlerta")]
        [Authorize]
        public async Task<IActionResult> UpdateAlerta(
             [FromBody] AlertaTransaccione entity)
        {
            try
            {
                // Registra el cambio en los logs   
                var userName = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

                _logger.LogInformation("Inicia Método UpdateAlerta");

                // Verifica si la tabla AlertaTransacciones existe en el contexto
                var dbSet = _context.AlertaTransacciones ?? throw new BadRequestException("No se encontró la propiedad AlertaTransacciones en el contexto.");

                // Busca la entidad existente por ID
                var existingEntity = await dbSet.FirstOrDefaultAsync(e => e.IdTransaccion == entity.IdTransaccion)
                    ?? throw new NotFoundException($"No se encontró la alerta con ID: {entity.IdTransaccion}");

                // Actualiza solo los campos necesarios
                existingEntity.Descripcion = entity.Descripcion;
                existingEntity.Acciones = entity.Acciones;
                existingEntity.Comentarios = entity.Comentarios;
                existingEntity.NombreCliente = entity.NombreCliente;
                existingEntity.DocumentoIdentificacion = entity.DocumentoIdentificacion;
                existingEntity.NombreAlerta = entity.NombreAlerta;
                existingEntity.AnalistaAsignado = entity.AnalistaAsignado;

                if (entity.AnalistaAsignado != "" && entity.EstatusEjecucion == 0)
                {
                    existingEntity.FechaAsignacion = DateTime.Now;
                    existingEntity.EstatusEjecucion = 1;
                    _context.Entry(existingEntity).Property(e => e.AnalistaAsignado).IsModified = true;
                    _context.Entry(existingEntity).Property(e => e.FechaAsignacion).IsModified = true;
                    _context.Entry(existingEntity).Property(e => e.EstatusEjecucion).IsModified = true;
                }
                else if(entity.EstatusEjecucion == 1)
                {
                    existingEntity.FechaAnalisis = DateTime.Now;
                    existingEntity.EstatusEjecucion = 2;
                    _context.Entry(existingEntity).Property(e => e.FechaAnalisis).IsModified = true;
                    _context.Entry(existingEntity).Property(e => e.EstatusEjecucion).IsModified = true;
                }
                else if (entity.EstatusEjecucion == 2)
                {
                    existingEntity.EstatusAlerta = entity.EstatusAlerta;
                    existingEntity.FechaAtencion = DateTime.Now;
                    existingEntity.EstatusEjecucion = 3;
                    _context.Entry(existingEntity).Property(e => e.FechaAtencion).IsModified = true;
                    _context.Entry(existingEntity).Property(e => e.EstatusEjecucion).IsModified = true;
                    _context.Entry(existingEntity).Property(e => e.EstatusAlerta).IsModified = true;
                }

                // Marca los campos como modificados
                _context.Entry(existingEntity).Property(e => e.NombreCliente).IsModified = true;
                _context.Entry(existingEntity).Property(e => e.Descripcion).IsModified = true;
                _context.Entry(existingEntity).Property(e => e.Acciones).IsModified = true;
                _context.Entry(existingEntity).Property(e => e.Comentarios).IsModified = true;


                // Guarda los cambios
                await _context.SaveChangesAsync();

                GuardarLog(userName, userRole, $"PUT: api/UpdateAlerta", "AlertaTransacciones", "APICONTROLLER",
                    $"Registro actualizado: Descripcion={entity.Descripcion}, Acciones={entity.Acciones}");

                return Ok(new { Message = "Registro creado exitosamente", Id = entity.IdTransaccion });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return ex switch
                {
                    BadRequestException => BadRequest(ex.Message),
                    NotFoundException => NotFound(ex.Message),
                    _ => StatusCode(500, ex.Message)
                };
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

        // Función auxiliar para obtener la dirección IP del usuario
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
    }
}
