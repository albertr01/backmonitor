namespace WebApplication1.Models.Response.Local.Salida
{
    public class TipoAlertasSalida : GenericoSalida
    {
        public List<TipoAlerta> tipoAlertas { get; set; }
        public List<SeleccionableSalida> TiposCliente { get; set; }
    }

    public class TipoAlerta
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Codigo { get; set; }
    }
}
