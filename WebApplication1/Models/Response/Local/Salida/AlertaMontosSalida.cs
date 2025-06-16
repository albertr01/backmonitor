namespace WebApplication1.Models.Response.Local.Salida
{
    public class AlertaMontosSalida : GenericoSalida
    {
        public List<TipoOperacion>? TipoOperacions { get; set; }
        public List<TipoMoneda>? TipoMonedas { get; set; }
        public List<AlertaMonto>? AlertaMontos { get; set; }
        public List<Agencia>? Agencias { get; set; }
        public List<SeleccionableSalida> TiposClientes { get; set; }
    }
    public class TipoOperacion
    {
        public decimal Id { get; set; }
        public string? Codigo { get; set; }
        public string? Nombre { get; set; }
    }
    public class TipoMoneda
    {
        public decimal Id { get; set; }
        public string? Codigo { get; set; }
        public string? Nombre { get; set; }
    }
    public class AlertaMonto : GenericoSalida
    {
        public decimal Id { get; set; }
        public string Codigo { get; set; }
        public decimal? IdTipoOperacion { get; set; }
        public string? NombreTipoOperacion { get; set; }
        public decimal? IdTipoMoneda { get; set; }
        public string? NombreTipoMoneda { get; set; }
        public decimal? Umbral { get; set; }
        public string? Comentario { get; set; }
        public int? IdCliente { get; set; }
        public int? IdAgencia { get; set; }
        public DateTime FechaConfiguracion { get; set; }
    }
    public class Agencia
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}
