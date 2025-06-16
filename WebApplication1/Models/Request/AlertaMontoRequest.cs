namespace WebApplication1.Models.Request
{
    public class AlertaMontoRequest
    {
        public int Id { get; set; }
        public int IdTipoOperacion { get; set; }
        public string CodigoTipoOperacion { get; set; }
        public int IdTipoMoneda { get; set; }
        public decimal Umbral { get; set; }
        public string Comentario { get; set; }
        public int IdCLiente { get; set; }
        public int IdAgencia { get; set; }
    }
}
