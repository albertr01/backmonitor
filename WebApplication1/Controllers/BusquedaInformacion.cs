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
    public class BusquedaInformacion : Controller
    {
        // GET: ListasRestrictivasController/Details/5
        private readonly MonitoreopyaContext _context;
        private readonly ILogger<MaestroParametrosController> _logger;

        public BusquedaInformacion(MonitoreopyaContext db, ILogger<MaestroParametrosController> logger)
        {
            _context = db;
            _logger = logger;
        }

        [HttpPost("BuscarClientesPorNombre")]
        [Authorize]
        public async Task<IActionResult> BuscarClientesPorNombre([FromBody] List<string> nombresBusqueda)
        {
            if (nombresBusqueda == null || !nombresBusqueda.Any())
                return BadRequest("Debe proporcionar al menos un nombre para buscar.");

            // Normaliza los nombres de búsqueda
            var nombresNormalizados = nombresBusqueda
                .Select(n => n.Trim().ToLower())
                .ToList();

            var clientes = await _context.ClienteBases
                .Where(c =>
                    nombresNormalizados.Any(nombre =>
                        (
                            ((c.NombreCompletoPrimerNombre ?? "") + " " +
                             (c.NombreCompletoSegundoNombre ?? "") + " " +
                             (c.NombreCompletoPrimerApellido ?? "") + " " +
                             (c.NombreCompletoSegundoApellido ?? "")
                            ).Trim().ToLower().Contains(nombre)
                        )
                    )
                )
                .ToListAsync();

            var resultado = new List<object>();

            foreach (var cliente in clientes)
            {
                var productos = await _context.ClienteProductos
                    .Where(p => p.IdCliente == cliente.IdCliente)
                    .Select(p => new
                    {
                        nombre = p.ProductoNombre,
                        nroCuenta = p.ProductoUso,
                        //balance = p.ProductoMontoPromedio.HasValue ? p.ProductoMontoPromedio.Value.ToString("N2") : null
                    })
                    .ToListAsync();

                resultado.Add(new
                {
                    tipoIdentificacion = cliente.TipoPersona,
                    identificacion = cliente.IdCliente,
                    nombre = $"{cliente.NombreCompletoPrimerNombre} {cliente.NombreCompletoSegundoNombre} {cliente.NombreCompletoPrimerApellido} {cliente.NombreCompletoSegundoApellido}".Trim(),
                    products = productos
                });
            }

            return Ok(resultado);
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
 