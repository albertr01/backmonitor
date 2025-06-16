using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.DB;
using WebApplication1.Models.Response;

namespace WebApplication1.Services
{
    public class MenuService
    {
        private readonly MonitoreopyaContext _context;

        public MenuService(MonitoreopyaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MenuTreeDTO>> GetMenuAllTree()
        {
            // Obtener las acciones autorizadas para el rol
            var accionesAutorizadas = await _context.Autorizaciones
                .Include(a => a.Accion)  // Traer las acciones relacionadas
                //.ThenInclude(ac => ac.Menu) // Incluir el menú de la acción
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

            return menuTree;
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
