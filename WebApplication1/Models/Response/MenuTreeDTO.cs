namespace WebApplication1.Models.Response
{
    public class MenuTreeDTO
    {
        public long Id { get; set; }
        public string? Nombre { get; set; }
        public string? Url { get; set; }
        public string? Icono { get; set; }
        public int? Orden { get; set; }

        public List<AccionDTO> Acciones { get; set; } = new List<AccionDTO>();

        public List<MenuTreeDTO> SubMenus { get; set; } = new List<MenuTreeDTO>();
    }
}
