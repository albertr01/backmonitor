using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;
using System.Text.Json;
using WebApplication1.Models.DB;


namespace WebApplication1.Controllers
{
    [Serializable]
    public class BadRequestException(string message) : Exception(message)
    {
        private readonly string message = message;
    }

    public class NotFoundException(string message) : Exception(message) 
    {
        private readonly string message = message;
    }

    [Route("api/[controller]")]
    [ApiController]
    public class MaestroParametrosController : ControllerBase
    {

        private readonly MonitoreopyaContext _context;
        private readonly ILogger<MaestroParametrosController> _logger;

        public MaestroParametrosController(MonitoreopyaContext db, ILogger<MaestroParametrosController> logger)
        {
            _context = db;
            _logger = logger;
        }

        /// <summary>
        /// GET: api/<MstParametros>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("MstParametros/{id}")]
        [Authorize]
        public async Task<ActionResult<MstTablasParametro>> GetMstParametro(int id)
        {
            var mstParametro = await _context.MstTablasParametros.FindAsync(id);

            if (mstParametro == null)
            {
                return NotFound();
            }

            return mstParametro;
        }

        /// <summary>
        /// GET: api/<MstTablasParametros>
        /// </summary>
        /// <param name="tabla">Numero de tabla asignado en MstTablasParametros</param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        [HttpGet("MstTablasParametros/{tabla}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<object>>> GetMstTablasParametros(int tabla)
        {
            IDictionary<string, object> data = new Dictionary<string, object>();

            try
            {
                Log.Information("Inicia Metodo GetMstTablasParametros");

                var tablaParametro = await _context.MstTablasParametros.FindAsync(tabla);
                IQueryable<object>? dbSet = null;

                if (tablaParametro == null)
                {
                    throw new BadRequestException($"Tabla no válida: {tabla}");
                }

                var tableName = tablaParametro.TablaParametro;

                var dbSetProperty = _context.GetType().GetProperty(tableName);

                if (dbSetProperty == null)
                {
                    throw new BadRequestException($"Nombre de tabla no valido: {tableName}");
                }

                dbSet = dbSetProperty.GetValue(_context) as IQueryable<object>;

                if (dbSet == null)
                {
                    throw new BadRequestException($"No se encontro la propiedad en el context: {dbSetProperty}");
                }

                if (tabla == 19)
                {
                    var entityType = dbSet.ElementType;
                    var existingEntity = await dbSet.Where(e => EF.Property<int>(e, "TipoTabla") == 2).ToListAsync();

                    return Ok(existingEntity);
                }
                else if ((tabla >= 10 && tabla <= 18) || tabla == 20)
                {
                    var parametros = await dbSet
                        .Select(x => new
                        {
                            Id = EF.Property<int>(x, "Id"),
                            TipoParametroRiesgo = EF.Property<string>(x, "TipoParametroRiesgo"),
                            ValorIn = EF.Property<int?>(x, "ValorIn"),
                            ValorFn = EF.Property<int?>(x, "ValorFn"),
                            Color = EF.Property<string>(x, "Color"),
                            IdTblParametro = EF.Property<int>(x, "IdTblParametro"),
                            IdMtsParametrosRiegos = EF.Property<int?>(x, "IdMtsParametrosRiegos")
                        })
                        .ToListAsync();
                    return Ok(parametros);
                }

                var result = await dbSet.ToListAsync();

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

        /// <summary>
        /// PUT: api/<MstTablasParametros>/5
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="BadHttpRequestException"></exception>
        [HttpPut("MstTablasParametros/{tabla}/{id}")]
        [Authorize]
        public async Task<IActionResult> PutMstTablasParametros(int tabla, int id, [FromBody] JsonElement entity)
        {
            try
            {
                Log.Information("Inicia Metodo PutMstTablasParametros");

                var userName = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

                var tablaParametro = await _context.MstTablasParametros.FindAsync(tabla);
                if (tablaParametro == null)
                {
                    throw new BadRequestException($"Tabla no válida: {tabla}");
                }

                var tableName = tablaParametro.TablaParametro;
                var dbSetProperty = _context.GetType().GetProperty(tableName);
                if (dbSetProperty == null)
                {
                    throw new BadRequestException($"Nombre de tabla no valido: {tableName}");
                }

                var dbSet = dbSetProperty.GetValue(_context) as IQueryable<object>;
                if (dbSet == null)
                {
                    throw new BadRequestException($"No se encontro la propiedad en el context: {dbSetProperty}");
                }

                var entityType = dbSet.ElementType;
                var existingEntity = await dbSet.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
                if (existingEntity == null)
                {
                    throw new NotFoundException($"Datos de entidad no válidos: {entityType}");
                }

                var updatedEntity = JsonSerializer.Deserialize(entity.GetRawText(), entityType, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                if (updatedEntity == null)
                {
                    throw new BadHttpRequestException($"Datos de entidad no válidos: {updatedEntity}");
                }

                _context.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
                _context.Entry(existingEntity).State = EntityState.Modified;

                string existingRegistro = JsonSerializer.Serialize(existingEntity);
                string newRegistro = entity.GetRawText();
                GuardarLog(userName, userRole, "PUT: api/<MstTablasParametros>", tableName, "APICONTROLLER", $"Registro: {existingRegistro} Update: {newRegistro}");

                await _context.SaveChangesAsync();

                if (tabla == 2)
                {
                    await ActualizarNivelRiesgoEnNacionalidad(id, entityType, updatedEntity, userName, userRole);
                }

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

        /// <summary>
        /// POST: api/<MstTablasParametros>
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost("MstTablasParametros/{tabla}")]
        [Authorize]
        public async Task<IActionResult> PostMstTablasParametros(int tabla, [FromBody] JsonElement entity)
        {
            try
            {
                Log.Information("Inicia Metodo PostMstTablasParametros");
                var userName = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
                var tablaParametro = await _context.MstTablasParametros.FindAsync(tabla);

                if (tablaParametro == null)
                {
                    throw new BadRequestException($"Tabla no válida: {tabla}");
                }

                var tableName = tablaParametro.TablaParametro;
                var dbSetProperty = _context.GetType().GetProperty(tableName);

                if (dbSetProperty == null)
                {
                    throw new BadRequestException($"Nombre de tabla no valido: {tableName}");
                }

                var dbSet = dbSetProperty.GetValue(_context) as IQueryable<object>;
                if (dbSet == null)
                {
                    throw new BadRequestException($"No se encontro la propiedad en el context: {dbSetProperty}");
                }

                var entityType = dbSet.ElementType;

                var newEntity = JsonSerializer.Deserialize(entity.GetRawText(), entityType, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });


                if (newEntity == null)
                {
                    throw new BadHttpRequestException($"Datos de entidad no válidos: {newEntity}");
                }

                _context.Add(newEntity);

                try
                {
                    await _context.SaveChangesAsync();
                    GuardarLog(userName, userRole, "POST: api/<MstTablasParametros>", tableName, "APICONTROLLER", $"Registro: {newEntity}");
                }
                catch (DbUpdateException)
                {
                    throw new Exception("Error al guardar la entidad");
                }

                return CreatedAtAction(nameof(GetMstTablasParametros), new { tabla, id = ((dynamic)newEntity).Id }, newEntity);
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

        /// <summary>
        /// DELETE: api/<MstTablasParametros>/5
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("MstTablasParametros/{tabla}/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteMstTablasParametros(int tabla, int id)
        {
            try
            {
                Log.Information("Inicia Metodo DeleteMstTablasParametros");
                var userName = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
                var tablaParametro = await _context.MstTablasParametros.FindAsync(tabla);

                if (tablaParametro == null)
                {
                    throw new BadRequestException($"Tabla no válida: {tabla}");
                }

                var tableName = tablaParametro.TablaParametro;

                var dbSetProperty = _context.GetType().GetProperty(tableName);

                if (dbSetProperty == null)
                {
                    throw new BadRequestException($"Nombre de tabla no valido: {tableName}");
                }

                var dbSet = dbSetProperty.GetValue(_context) as IQueryable<object>;
                if (dbSet == null)
                {
                    throw new BadRequestException($"No se encontro la propiedad en el context: {dbSetProperty}");
                }

                var entityType = dbSet.ElementType;
                var existingEntity = await dbSet.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);

                if (existingEntity == null)
                {
                    throw new NotFoundException($"Datos de entidad no válidos: {entityType}");
                }

                _context.Remove(existingEntity);
                await _context.SaveChangesAsync();

                GuardarLog(userName, userRole, "DELETE: api/<MstTablasParametros>", tableName, "APICONTROLLER", $"Registro: {existingEntity}");

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

        /// <summary>
        /// DELETE MULTIPLES: api/<MstTablasParametros>/5
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete("MstTablasParametros/List/{tabla}")]
        [Authorize]
        public async Task<IActionResult> DeleteMstTablasParametros(int tabla, [FromBody] List<int> ids)
        {
            try
            {
                Log.Information("Inicia Metodo DeleteMstTablasParametros List");
                var userName = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
                var tablaParametro = await _context.MstTablasParametros.FindAsync(tabla);

                if (tablaParametro == null)
                {
                    throw new BadRequestException($"Tabla no válida: {tabla}");
                }

                var tableName = tablaParametro.TablaParametro;

                var dbSetProperty = _context.GetType().GetProperty(tableName);

                if (dbSetProperty == null)
                {
                    throw new BadRequestException($"Nombre de tabla no valido: {tableName}");
                }

                var dbSet = dbSetProperty.GetValue(_context) as IQueryable<object>;

                if (dbSet == null)
                {
                    throw new BadRequestException($"No se encontro la propiedad en el context: {dbSetProperty}");
                }

                var entityType = dbSet.ElementType;

                var entities = await dbSet.Where(e => ids.Contains(EF.Property<int>(e, "Id"))).ToListAsync();

                _context.RemoveRange(entities);
                await _context.SaveChangesAsync();

                foreach (var entitie in entities)
                {
                    string DeleteJson = JsonSerializer.Serialize(entitie);
                    GuardarLog(userName, userRole, "DELETE MULTIPLES: api/<MstTablasParametros>", tableName, "APICONTROLLER", $"Registro: {DeleteJson}");
                }

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

        /// <summary>
        /// POST: api/<ImportTablasParametros>
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost("ImportTablasParametros/{tabla}")]
        [Authorize]
        public async Task<IActionResult> PostImportarTablas(int tabla, [FromBody] JsonElement entity)
        {
            try
            {
                Log.Information("Inicia Metodo PostImportarTablas");
                var userName = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
                var tablaParametro = await _context.MstTablasParametros.FindAsync(tabla);

                if (tablaParametro == null)
                {
                    throw new BadRequestException($"Tabla no válida: {tabla}");
                }

                var tableName = tablaParametro.TablaParametro;

                var dbSetProperty = _context.GetType().GetProperty(tableName);

                if (dbSetProperty == null)
                {
                    throw new BadRequestException($"Nombre de tabla no valido: {tableName}");
                }

                var dbSet = dbSetProperty.GetValue(_context) as IQueryable<object>;
                if (dbSet == null)
                {
                    throw new BadRequestException($"No se encontro la propiedad en el context: {dbSetProperty}");
                }

                var entityType = dbSet.ElementType;

                var newEntities = JsonSerializer.Deserialize(entity.GetRawText(), typeof(List<>).MakeGenericType(entityType), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) as IEnumerable<object>;


                if (newEntities == null)
                {
                    throw new NotFoundException($"Datos de entidad no válidos: {newEntities}");
                }

                _context.AddRange(newEntities);

                try
                {
                    await _context.SaveChangesAsync();

                    foreach (var entitie in newEntities)
                    {
                        string InsertJson = JsonSerializer.Serialize(entitie);
                        GuardarLog(userName, userRole, "POST: api/<ImportTablasParametros>", tableName, "APICONTROLLER", $"Registro: {InsertJson}");
                    }

                }
                catch (DbUpdateException)
                {
                    return StatusCode(500, "Error al Importar la entidad");
                }

                return CreatedAtAction(nameof(GetMstTablasParametros), new { tabla }, newEntities);
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

        private bool MstParametroExists(int id)
        {
            return _context.MstTablasParametros.Any(e => e.Id == id);
        }

        private bool MstZonaGeograficaExists(int id)
        {
            return _context.MstZonaGeograficas.Any(e => e.Id == id);
        }

        private void GuardarLog(string username, string rol, string evento, string tablaDB, string Operación,string Detalle_registro)
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

        private async Task ActualizarNivelRiesgoEnNacionalidad(int id, Type entityType, object updatedEntity, string userName, string userRole)
        {
            var nivelRiesgoProperty = entityType.GetProperty("NivelRiesgo");
            if (nivelRiesgoProperty == null)
            {
                throw new BadHttpRequestException($"Propiedad NivelRiesgo no encontrada: {entityType}");
            }

            string? nivelRiesgo = nivelRiesgoProperty.GetValue(updatedEntity) as string;
            if (nivelRiesgo == null)
            {
                throw new BadHttpRequestException($"NivelRiesgo no puede ser nulo: {updatedEntity}");
            }

            string newJsonN = JsonSerializer.Serialize(updatedEntity);
            var nacionalidades = await _context.MstNacionalidads.Where(n => n.IdPais == id).ToListAsync();

            if (nivelRiesgo != nacionalidades.FirstOrDefault()?.NivelRiesgo)
            {
                foreach (var nacionalidad in nacionalidades)
                {
                    string existingJsonN = JsonSerializer.Serialize(nacionalidad);
                    GuardarLog(userName, userRole, "PUT: api/<MstTablasParametros>", "MstNacionalidad", "APICONTROLLER", $"Registro: {existingJsonN} Update: {newJsonN}");
                    nacionalidad.NivelRiesgo = nivelRiesgo;
                    _context.Entry(nacionalidad).State = EntityState.Modified;
                }

                await _context.SaveChangesAsync();
            }
        }

    }
}
