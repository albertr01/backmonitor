using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApplication1.Models.DB;
using WebApplication1.Models.Response;
using WebApplication1.Services;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class RolesController : ControllerBase
    {
        private readonly MonitoreopyaContext _context;
        private readonly MenuService _menuService;

        public RolesController(MonitoreopyaContext context, MenuService menuService)
        {
            _context = context;
            _menuService = menuService; 
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<ResponseRolesDTO>> GetRoles()
        {
            var roles = await _context.Roles
                .Select(r => new RolesDTO
                {
                    Id = r.Id,
                    Nombre = r.Nombre,
                    Descripcion = r.Descripcion,
                    Autorizaciones = r.Autorizaciones.Select(a => new AutorizacionDTO
                    {
                        Id = a.Id,
                        NombreAccion = a.Accion.Nombre 
                    }).ToList(), 
                    Estatus = r.Estatus
                })
                .ToListAsync();

            var menu = _menuService.GetMenuAllTree();
            
            var response = new ResponseRolesDTO
            {
                menu = menu.Result.ToList(), 
                listaRoles = roles
            };
            return Ok(response);
        }
        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RolesDTO>> GetRol(int id)
        {
            var rol = await _context.Roles
                .Where(r => r.Id == id)
                .Select(r => new RolesDTO
                {
                    Id = r.Id,
                    Nombre = r.Nombre,
                    Descripcion = r.Descripcion,
                    Autorizaciones = r.Autorizaciones.Select(a => new AutorizacionDTO
                    {
                        Id = a.Id,
                        NombreAccion = a.Accion.Nombre
                    }).ToList(),
                    Estatus = r.Estatus
                })
                .FirstOrDefaultAsync();

            if (rol == null)
            {
                return NotFound();
            }

            return Ok(rol);
        }


        // POST: api/Roles
        [HttpPost]
        public async Task<ActionResult<Role>> CreateRol(RolesDTO rolDto)
        {
            if (string.IsNullOrWhiteSpace(rolDto.Nombre))
            {
                return BadRequest("El nombre del rol es obligatorio.");
            }

            // Crear el objeto Role a partir del DTO
            var nuevoRol = new Role
            {
                Nombre = rolDto.Nombre,
                Descripcion = rolDto.Descripcion,
                Estatus = rolDto.Estatus,
                Autorizaciones = new HashSet<Autorizacione>() // Se usa HashSet para evitar duplicados
            };

            // Verificar y agregar autorizaciones al nuevo rol
            if (rolDto.Autorizaciones != null && rolDto.Autorizaciones.Any())
            {
                var autorizaciones = await _context.Autorizaciones
                    .Where(a => rolDto.Autorizaciones.Select(dto => dto.Id).Contains(a.Id))
                    .ToListAsync();

                foreach (var autorizacion in autorizaciones)
                {
                    nuevoRol.Autorizaciones.Add(autorizacion);
                }
            }

            // Agregar el rol a la base de datos
            _context.Roles.Add(nuevoRol);
            await _context.SaveChangesAsync();

            // Retornar el objeto creado con su ID asignado
            return CreatedAtAction(nameof(GetRol), new { id = nuevoRol.Id }, nuevoRol);
        }


        // PUT: api/Roles
        [HttpPut]
        public async Task<IActionResult> UpdateRol(RolesDTO rolDto)
        {
            if (rolDto.Id <= 0)
            {
                return BadRequest("El ID del rol es inválido.");
            }

            var rol = await _context.Roles
                .Include(r => r.Autorizaciones) // Incluir las autorizaciones existentes en BD
                .FirstOrDefaultAsync(r => r.Id == rolDto.Id);

            if (rol == null)
            {
                return NotFound("El rol no existe.");
            }

            // Actualizar los campos básicos del rol
            rol.Nombre = rolDto.Nombre;
            rol.Descripcion = rolDto.Descripcion;
            rol.Estatus = rolDto.Estatus;

            // Obtener la lista de autorizaciones actuales en BD
            var autorizacionesActuales = rol.Autorizaciones.ToList();

            // Crear lista de IDs de autorizaciones nuevas desde el DTO
            var nuevasAutorizacionesIds = rolDto.Autorizaciones.Select(a => a.Id).ToList();

            // Identificar autorizaciones a eliminar
            var autorizacionesAEliminar = autorizacionesActuales
                .Where(a => !nuevasAutorizacionesIds.Contains(a.Id))
                .ToList();

            // Identificar autorizaciones a agregar
            var autorizacionesAAgregar = nuevasAutorizacionesIds
                .Where(idNuevo => !autorizacionesActuales.Any(a => a.Id == idNuevo))
                .ToList();

            // Eliminar autorizaciones que ya no están en la solicitud
            _context.Autorizaciones.RemoveRange(autorizacionesAEliminar);

            // Agregar nuevas autorizaciones
            foreach (var nuevaAutorizacionId in autorizacionesAAgregar)
            {
                var nuevaAutorizacion = await _context.Autorizaciones
                    .FirstOrDefaultAsync(a => a.Id == nuevaAutorizacionId);

                if (nuevaAutorizacion != null)
                {
                    rol.Autorizaciones.Add(nuevaAutorizacion);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RolExists(rolDto.Id))
                {
                    return NotFound("El rol fue eliminado antes de actualizarlo.");
                }
                else
                {
                    throw;
                }
            }

            return Ok("Actualizacion realizada");
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRol(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }

            rol.Estatus = 0; 

            _context.Roles.Update(rol);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RolExists(int id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}
