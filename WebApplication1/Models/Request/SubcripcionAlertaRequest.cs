namespace WebApplication1.Models.Request
{
    public class SubcripcionAlertaRequest
    {
        public int? Id { get; set; }
        public int? IdAlerta { get; set; }
        public string NombreAlerta { get; set; }
        public string Descripcion { get; set; }
        public int MetodoDistribucion { get; set; }
        public int BandejaEntrada { get; set; }
        public int? IdCliente { get; set; }
        public bool? NotificacionCorreo { get; set; }
        public string? CorreoElectronico { get; set; }
        public string Comentario { get; set; }
        public bool Estado { get; set; }
    }
}
