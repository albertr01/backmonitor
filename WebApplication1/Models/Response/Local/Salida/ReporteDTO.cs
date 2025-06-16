namespace WebApplication1.Models.Response.Local.Salida
{
    public class ReporteDTO
    {
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public int Porcentaje { get; set; }
    }

    public class ListaGenerica { 
        public string Nombre { get; set; }
        public List<ReporteDTO> items { get; set; }
    }

}
