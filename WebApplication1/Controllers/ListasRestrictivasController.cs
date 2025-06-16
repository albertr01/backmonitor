using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;
using System.Text.Json;
using WebApplication1.Models.DB;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ListasRestrictivasController : Controller
    {
        // GET: ListasRestrictivasController/Details/5
        private readonly MonitoreopyaContext _context;
        private readonly ILogger<MaestroParametrosController> _logger;

        public ListasRestrictivasController(MonitoreopyaContext db, ILogger<MaestroParametrosController> logger)
        {
            _context = db;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene la lista interna de personas y sus datos asociados.
        /// </summary>
        /// <param name="numDocumento"></param>
        /// <param name="nombRazonSocial"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        [HttpGet("ListaInterna/")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ListaInterna>>> GetListInterna(
            [FromQuery] int? numDocumento,
            [FromQuery] string? nombRazonSocial)
            //[FromQuery] int page = 1,
            //[FromQuery] int pageSize = 1000)
        {
            try
            {
                var query = _context.ListaInternas.AsQueryable();

                if (query == null)
                {
                    throw new BadRequestException($"No se encontro la propiedad en el context");
                }

                if (numDocumento.HasValue)
                {
                    query = query.Where(list => list.NumDocumento == numDocumento.Value);
                }
                if (!string.IsNullOrEmpty(nombRazonSocial))
                {   
                    query = query.Where(list => list.NombRazonSocial == nombRazonSocial);
                }

                var totalRegistros = await query.CountAsync();

                var lists = await query
                    .OrderByDescending(list => list.Id)
                    //.Skip((page - 1) * pageSize)
                    //.Take(pageSize)
                    .ToListAsync();

                return Ok(new { totalRegistros, lists });
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
        /// Obtiene la lista de personas politicamente expuestas (PEP) y sus datos asociados.
        /// </summary>
        /// <param name="numDocumento"></param>
        /// <param name="nombRazonSocial"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        [HttpGet("ListaPEP/")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ListaInterna>>> GetListPEP(
            [FromQuery] int? numDocumento,
            [FromQuery] string? nombRazonSocial)
        //[FromQuery] int page = 1,
        //[FromQuery] int pageSize = 1000)
        {
            try
            {
                var query = _context.ListaPeps.AsQueryable();

                if (query == null)
                {
                    throw new BadRequestException($"No se encontro la propiedad en el context");
                }

                if (numDocumento.HasValue)
                {
                    query = query.Where(list => list.NumDocumento == numDocumento.Value);
                }
                if (!string.IsNullOrEmpty(nombRazonSocial))
                {
                    query = query.Where(list => list.NombApellido == nombRazonSocial);
                }

                var totalRegistros = await query.CountAsync();

                var lists = await query
                    .OrderByDescending(list => list.Id)
                    //.Skip((page - 1) * pageSize)
                    //.Take(pageSize)
                    .ToListAsync();

                return Ok(new { totalRegistros, lists });
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
        /// PUT Lista PEP y Lista Interna
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="BadHttpRequestException"></exception>
        [HttpPut("")]
        [Authorize]
        public async Task<IActionResult> PutListasRestrictivas(
            [FromQuery] string lista,
            [FromQuery] int id, 
            [FromBody] JsonElement entity)
        {
            try
            {
                Log.Information("Inicia Metodo PUTListasRestrictivas");

                var userName = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
                string tabla = string.Empty;

                switch (lista)
                {
                    case "interna":
                        tabla = "ListaInternas";
                        break;
                    case "PEP":
                        tabla = "ListaPeps";
                        break;
                    default:
                        break;
                }

                if (tabla.IsNullOrEmpty())
                {
                    throw new BadRequestException($"Tabla no válida");
                }

                var dbSetProperty = _context.GetType().GetProperty(tabla);
                if (dbSetProperty == null)
                {
                    throw new BadRequestException($"Nombre de tabla no valido: {tabla}");
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
                GuardarLog(userName, userRole, $"PUT: api/{tabla}", tabla, "APICONTROLLER", $"Registro: {existingRegistro} Update: {newRegistro}");

                await _context.SaveChangesAsync();

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
        /// POST Lista PEP y Lista Interna
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        /// <exception cref="BadHttpRequestException"></exception>
        [HttpPost("")]
        [Authorize]
        public async Task<IActionResult> PostListasRestrictivas(
            [FromQuery] string lista,
            [FromBody] JsonElement entity)
        {
            try
            {
                Log.Information("Inicia Metodo POSTListasRestrictivas");
                var userName = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
                string tabla = string.Empty;

                switch (lista)
                {
                    case "interna":
                        tabla = "ListaInternas";
                        break;
                    case "PEP":
                        tabla = "ListaPeps";
                        break;
                    default:
                        break;
                }

                if (tabla.IsNullOrEmpty())
                {
                    throw new BadRequestException($"Tabla no válida");
                }

                var dbSetProperty = _context.GetType().GetProperty(tabla);

                if (dbSetProperty == null)
                {
                    throw new BadRequestException($"Nombre de tabla no valido: {tabla}");
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
                    GuardarLog(userName, userRole, $"POST: api/<{tabla}>", tabla, "APICONTROLLER", $"Registro: {newEntity}");
                }
                catch (DbUpdateException)
                {
                    throw new Exception("Error al guardar la entidad");
                }

                return CreatedAtAction(nameof(GetListPEP), new { tabla, id = ((dynamic)newEntity).Id }, newEntity);
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
        /// DELETE Lista PEP y Lista Interna
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        /// <exception cref="NotFoundException"></exception>
        [HttpDelete("")]
        [Authorize]
        public async Task<IActionResult> DeleteListasRestrictivas(
            [FromQuery] string lista,
            [FromQuery] int id)
        {
            try
            {
                Log.Information("Inicia Metodo DELETEListasRestrictivas");
                var userName = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

                string tabla = string.Empty;

                switch (lista)
                {
                    case "interna":
                        tabla = "ListaInternas";
                        break;
                    case "PEP":
                        tabla = "ListaPeps";
                        break;
                    default:
                        break;
                }

                if (tabla.IsNullOrEmpty())
                {
                    throw new BadRequestException($"Tabla no válida");
                }

                var dbSetProperty = _context.GetType().GetProperty(tabla);

                if (dbSetProperty == null)
                {
                    throw new BadRequestException($"Nombre de tabla no valido: {tabla}");
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

                GuardarLog(userName, userRole, $"DELETE: api/{tabla}", tabla, "APICONTROLLER", $"Registro: {existingEntity}");

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
        ///  DELETE MULTIPLES Lista PEP y Lista Interna
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        [HttpDelete("List/")]
        [Authorize]
        public async Task<IActionResult> DeleteMstTablasParametros(
            [FromQuery] string lista,
            [FromBody] List<int> ids)
        {
            try
            {
                Log.Information("Inicia Metodo DELETEListasRestrictivas List");
                var userName = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

                string tabla = string.Empty;

                switch (lista)
                {
                    case "interna":
                        tabla = "ListaInternas";
                        break;
                    case "PEP":
                        tabla = "ListaPeps";
                        break;
                    default:
                        break;
                }

                if (tabla == null)
                {
                    throw new BadRequestException($"Tabla no válida");
                }

                var dbSetProperty = _context.GetType().GetProperty(tabla);

                if (dbSetProperty == null)
                {
                    throw new BadRequestException($"Nombre de tabla no valido: {tabla}");
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
                    GuardarLog(userName, userRole, $"DELETE MULTIPLES: api/{tabla}", tabla, "APICONTROLLER", $"Registro: {DeleteJson}");
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
        /// POST IMPORT Lista PEP y Lista Interna
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        /// <exception cref="NotFoundException"></exception>
        [HttpPost("Import/")]
        [Authorize]
        public async Task<IActionResult> PostImportarListas(
            [FromQuery] string lista,
            [FromBody] JsonElement entity)
        {
            try
            {
                Log.Information("Inicia Metodo PostImportarListas");
                var userName = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

                string tabla = string.Empty;
                string VersionContent = string.Empty;

                switch (lista)
                {
                    case "interna":
                        tabla = "ListaInternas";
                        break;
                    case "PEP":
                        tabla = "ListaPeps";
                        break;
                    default:
                        break;
                }

                if (tabla.IsNullOrEmpty())
                {
                    throw new BadRequestException($"Tabla no válida: {tabla}");
                }

                var dbSetProperty = _context.GetType().GetProperty(tabla);

                if (dbSetProperty == null)
                {
                    throw new BadRequestException($"Nombre de tabla no valido: {tabla}");
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

                // Extraer el valor de idVersionContent
                var jsonArray = JsonDocument.Parse(entity.GetRawText()).RootElement;
                VersionContent = jsonArray[0].GetProperty("idVersionContent").GetString() ?? string.Empty;

                _context.AddRange(newEntities);

                try
                {
                    await _context.SaveChangesAsync();

                    foreach (var entitie in newEntities)
                    {
                        string InsertJson = JsonSerializer.Serialize(entitie);
                        GuardarLog(userName, userRole, $"POST: api/<Import {tabla}>", tabla, "APICONTROLLER", $"Registro: {InsertJson} Comentario de Version : {VersionContent}");
                    }

                }
                catch (DbUpdateException)
                {
                    return StatusCode(500, "Error al Importar la entidad");
                }

                return CreatedAtAction(nameof(GetListPEP), new { tabla }, newEntities);
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
                Modulo = "LISTA RESTRICTIVA",
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
