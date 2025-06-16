

using WebApplication1.Models.DB;

namespace WebApplication1.Models.Response
{
    public class CreateRoleDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public List<MenuPermissionDto> MenuPermissions { get; set; } = new List<MenuPermissionDto>();
    }

    public class MenuPermissionDto
    {
        public int MenuId { get; set; }
        public List<int> PermissionIds { get; set; } = new List<int>();
    }
}
