

using WebApplication1.Models.DB;

namespace WebApplication1.Models.Response
{
    public class RolesDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<RoleMenuPermissionDto> MenuPermissions { get; set; } = new List<RoleMenuPermissionDto>();
    }
}
