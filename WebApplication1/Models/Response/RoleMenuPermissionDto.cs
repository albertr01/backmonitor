

using WebApplication1.Models.DB;

namespace WebApplication1.Models.Response
{
    public class RoleMenuPermissionDto
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int MenuId { get; set; }
        public int PermissionId { get; set; }
        public bool IsGranted { get; set; }
        public string MenuName { get; set; } = string.Empty;
        public string PermissionName { get; set; } = string.Empty;
        public string PermissionCode { get; set; } = string.Empty;
    }
}
