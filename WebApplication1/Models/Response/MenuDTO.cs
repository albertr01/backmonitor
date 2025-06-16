using WebApplication1.Models.DB;

namespace WebApplication1.Models.Response
{
    public class MenuDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? Url { get; set; }
        public string? Icono { get; set; }
        public int Orden { get; set; }
        public int? MenuPadreId { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }


        public MenuDTO() { }

        public MenuDTO(Menu menu)
        {
            Id = menu.Id;
            Nombre = menu.Label;
            Url = menu.RouteLink;
            Icono = menu.Icon;
            Orden = menu.OrderIndex;
            MenuPadreId = menu.ParentId;
        }
    }
}
