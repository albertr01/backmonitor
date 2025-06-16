using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using WebApplication1.Models.DB;
using WebApplication1.Models.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly MonitoreopyaContext _context;

        public MenuController(MonitoreopyaContext context)
        {
            _context = context;
        }

        // Obtener todos los menús
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuDTO>>> GetMenus()
        {
            var menus = await _context.Menu.ToListAsync();
            return Ok(menus.Select(m => new MenuDTO(m)));
        }

        // Obtener un menú por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<MenuDTO>> GetMenu(int id)
        {
            var menu = await _context.Menu.FindAsync(id);

            if (menu == null)
                return NotFound();

            return new MenuDTO(menu);
        }

        // Crear un nuevo menú
        [HttpPost]
        public async Task<ActionResult<MenuDTO>> CreateMenu(MenuDTO menuDto)
        {
            var menu = new Menu
            {
                Label = menuDto.Nombre,
                RouteLink = menuDto.Url,
                Icon = menuDto.Icono,
                OrderIndex = menuDto.Orden,
                ParentId = menuDto.MenuPadreId
            };

            _context.Menu.Add(menu);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMenu), new { id = menu.Id }, new MenuDTO(menu));
        }

        // Actualizar un menú
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenu(int id, MenuDTO menuDto)
        {
            if (id != menuDto.Id)
                return BadRequest();

            var menu = await _context.Menu.FindAsync(id);
            if (menu == null)
                return NotFound();

            menu.Label = menuDto.Nombre;
            menu.RouteLink = menuDto.Url;
            menu.Icon = menuDto.Icono;
            menu.OrderIndex = menuDto.Orden;
            menu.ParentId = menuDto.MenuPadreId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Eliminar un menú
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            var menu = await _context.Menu.FindAsync(id);
            if (menu == null)
                return NotFound();

            _context.Menu.Remove(menu);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private List<MenuTreeDTO> BuildMenuTree(List<Menu> menus, long? parentId)
        {
            return menus
                .Where(m => m.ParentId == parentId)
                .OrderBy(m => m.OrderIndex)
                .Select(m => new MenuTreeDTO
                {
                    Id = m.Id,
                    Nombre = m.Label,
                    Url = m.RouteLink,
                    Icono = m.Icon,
                    Orden = m.OrderIndex,
                    SubMenus = BuildMenuTree(menus, m.Id),
                    Acciones = m.Acciones.Select(a => new AccionDTO
                    {
                        Id = a.Id,
                        Nombre = a.Nombre,
                        Endpoint = a.Endpoint
                    }).ToList()
                }).ToList();
        }

        // Obtener el menú dinámico basado en autorizaciones del usuario
        [HttpGet("user-menu/{userId}")]
        public async Task<ActionResult<IEnumerable<MenuTreeDTO>>> GetUserMenu(int userId)
        {
            var user = await _context.UsuariosAutorizados
                .Include(u => u.Rol)
                .ThenInclude(r => r.Autorizaciones)
                .ThenInclude(a => a.Accion)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return NotFound("Usuario no encontrado");

            var autorizaciones = user.Rol.Autorizaciones.Select(a => a.Accion.MenuId).Distinct().ToList();
            var menusAutorizados = await _context.Menu
                .Where(m => autorizaciones.Contains(m.Id))
                .ToListAsync();

            var menuTree = BuildMenuTree(menusAutorizados, null);
            return Ok(menuTree);
        }


        // Obtener el menú en formato de árbol según las acciones permitidas al rol
        [HttpGet("tree")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MenuTreeDTO>>> GetMenuTreeByRole()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Token no proporcionado" });
            }

            // Extraer el UserId del token JWT
            var roleIdClaim = User.Claims.FirstOrDefault(c => c.Type == "RolId")?.Value;
            if (string.IsNullOrEmpty(roleIdClaim) || !int.TryParse(roleIdClaim, out int roleId))
            {
                return BadRequest(new { message = "No se pudo obtener el RoleId del token" });
            }
     
            // Obtener las acciones autorizadas para el rol
            var accionesAutorizadas = await _context.Autorizaciones
                .Where(a => a.RolId == roleId)
                .Include(a => a.Accion)  // Traer las acciones relacionadas
                .ThenInclude(ac => ac.Menu) // Incluir el menú de la acción
                .Select(a => a.Accion)
                .ToListAsync();

            // Obtener los menús que tienen al menos una acción autorizada
            var menusConAcciones = accionesAutorizadas
                .Select(a => a.Menu)
                .Distinct()
                .ToList();

            // Obtener todos los menús, incluyendo los padres para la jerarquía
            var allMenus = await _context.Menu.ToListAsync();

            // Construir el árbol de menús con la nueva lógica
            var menuTree = BuildMenuTree(allMenus, null, menusConAcciones);

            return Ok(menuTree);
        }


        [HttpGet("tree2")]
        public async Task<ActionResult<IEnumerable<MenuTreeDTO>>> GetMenuAllTree()
        {
            // Obtener las acciones autorizadas para el rol
            var accionesAutorizadas = await _context.Autorizaciones
                .Include(a => a.Accion)  // Traer las acciones relacionadas
                .ThenInclude(ac => ac.Menu) // Incluir el menú de la acción
                .Select(a => a.Accion)
                .ToListAsync();

            // Obtener los menús que tienen al menos una acción autorizada
            var menusConAcciones = accionesAutorizadas
                .Select(a => a.Menu)
                .Distinct()
                .ToList();

            // Obtener todos los menús, incluyendo los padres para la jerarquía
            var allMenus = await _context.Menu.ToListAsync();

            // Construir el árbol de menús con la nueva lógica
            var menuTree = BuildMenuTree(allMenus, null, menusConAcciones);

            return Ok(menuTree);
        }

        private List<MenuTreeDTO> BuildMenuTree(List<Menu> allMenus, long? parentId, List<Menu> menusConAcciones)
        {
            return allMenus
                .Where(m => m.ParentId == parentId) // Filtra los menús hijos del actual
                .OrderBy(m => m.OrderIndex)
                .Select(m => new MenuTreeDTO
                {
                    Id = m.Id,
                    Nombre = m.Label,
                    Url = m.RouteLink,
                    Icono = m.Icon,
                    Orden = m.OrderIndex,
                    SubMenus = BuildMenuTree(allMenus, m.Id, menusConAcciones), // Construcción recursiva del árbol
                    Acciones = menusConAcciones.Contains(m)
                        ? m.Acciones.Select(a => new AccionDTO
                        {
                            Id = a.Id,
                            Nombre = a.Nombre,
                            Endpoint = a.Endpoint
                        }).ToList()
                        : new List<AccionDTO>() // Si no tiene acciones, devolver lista vacía
                })
                .Where(m => m.Acciones.Any() || m.SubMenus.Any()) // Solo incluir menús relevantes
                .ToList();
        }


    }
}
