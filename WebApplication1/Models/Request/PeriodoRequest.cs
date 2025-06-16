namespace WebApplication1.Models.Request
{
    /// <summary>
    /// Periodo registro
    /// </summary>
    public class PeriodoRequest
    {
        /// <summary>
        /// Tipo de periodo
        /// </summary>
        public string TipoPeriodo { get; set; }
        public string NombreEvaluacion { get; set; }
        /// <summary>
        /// Periodo
        /// </summary>
        public string Periodo { get; set; }
        /// <summary>
        /// Fecha de inicio
        /// </summary>
        public DateTime FechaInicio { get; set; }
        /// <summary>
        /// Fecha fin
        /// </summary>
        public DateTime FechaFin { get; set; }
    }
}
