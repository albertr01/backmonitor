using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.DB;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorizacionesController : ControllerBase
    {
        private readonly MonitoreopyaContext _context;

        public AutorizacionesController(MonitoreopyaContext context)
        {
            _context = context;
        }

        public class AutorizacionRequest
        {
            public int RoleId { get; set; }
            public List<int> AccionIds { get; set; } = new List<int>();
        }

        // Endpoint para asignar acciones a un rol
        [HttpPost("asignar")]
        public async Task<IActionResult> AsignarAccionesARol([FromBody] AutorizacionRequest request)
        {
            if (request == null || request.AccionIds == null || !request.AccionIds.Any())
            {
                return BadRequest("El cuerpo de la solicitud es inválido o la lista de acciones está vacía.");
            }

            // Verificar si el rol existe
            var rolExiste = await _context.Roles.AnyAsync(r => r.Id == request.RoleId);
            if (!rolExiste)
            {
                return NotFound($"El rol con ID {request.RoleId} no existe.");
            }

            // Filtrar acciones que ya están asignadas
            var autorizacionesExistentes = await _context.Autorizaciones
                .Where(a => a.RolId == request.RoleId && request.AccionIds.Contains(a.AccionId))
                .Select(a => a.AccionId)
                .ToListAsync();

            var nuevasAutorizaciones = request.AccionIds
                .Where(a => !autorizacionesExistentes.Contains(a))
                .Select(accionId => new Autorizacione { RolId = request.RoleId, AccionId = accionId })
                .ToList();

            if (nuevasAutorizaciones.Any())
            {
                _context.Autorizaciones.AddRange(nuevasAutorizaciones);
                await _context.SaveChangesAsync();
            }

            return Ok("Autorizaciones asignadas correctamente.");
        }

        // Endpoint para revocar acciones de un rol
        [HttpPost("revocar")]
        public async Task<IActionResult> RevocarAccionesDeRol([FromBody] AutorizacionRequest request)
        {
            if (request == null || request.AccionIds == null || !request.AccionIds.Any())
            {
                return BadRequest("El cuerpo de la solicitud es inválido o la lista de acciones está vacía.");
            }

            var autorizacionesAEliminar = await _context.Autorizaciones
                .Where(a => a.RolId == request.RoleId && request.AccionIds.Contains(a.AccionId))
                .ToListAsync();

            if (!autorizacionesAEliminar.Any())
            {
                return NotFound("No se encontraron autorizaciones para eliminar.");
            }

            _context.Autorizaciones.RemoveRange(autorizacionesAEliminar);
            await _context.SaveChangesAsync();

            return Ok("Autorizaciones eliminadas correctamente.");
        }
    }

}
