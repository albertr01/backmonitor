namespace WebApplication1.Models.Response.Local.Salida
{
    public class SubscripcionAlertaSalida : GenericoSalida
    {
        public List<SuscripcionAlerta> suscripcionAlertas { get; set; }
    }
    public class SuscripcionAlerta
    {
        public int? Id { get; set; }
        public int? IdAlerta { get; set; }
        public string NombreAlerta { get; set; }
        public string Descripcion { get; set; }
        public int? MetodoDistribucion { get; set; }
        public int? BandejaEntrada { get; set; }
        public string Comentario { get; set; }
        public bool? Estado { get; set; }
        public DateTime? Fecha { get; set; }
        public string Usuario { get; set; }
        public string? CorreoElectronico { get; set; }
        public int? IdCliente { get; set; }
        public string? NombreTipoCliente { get; set; }
        public bool? NotificacionCorreo { get; set; }
        public List<AlertaHistorial> Historial { get; set; }
    }
    public class AlertaHistorial
    {
        public DateTime FechaHora { get; set; }
        public string Usuario { get; set; }
        public bool EsEncendido { get; set; }
    }
}
