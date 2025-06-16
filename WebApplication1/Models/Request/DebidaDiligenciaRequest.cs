namespace WebApplication1.Models.Request
{
    /// <summary>
    /// Solicitud de guardar Debida Diligencia
    /// </summary>
    public class DebidaDiligenciaRequest
    {
        /// <summary>
        /// Tipo de identificación
        /// </summary>
        public string TipoIdentificacion { get; set; }
        /// <summary>
        /// identificación
        /// </summary>
        public string Identificacion { get; set; }
        /// <summary>
        /// Indica si tiene debida diligencia
        /// </summary>
        public bool DebidaDiligencia { get; set; }
        /// <summary>
        /// Indica si tiene debida diligencia vencida
        /// </summary>
        public bool DebidaDiligenciaVencida { get; set; }
        /// <summary>
        /// Fecha de debida diligencia
        /// </summary>
        public DateTime FechaDebidaDiligencia { get; set; }
        /// <summary>
        /// Observaciones
        /// </summary>
        public string Observaciones { get; set; }
        /// <summary>
        /// Tipo de debida diligencia
        /// </summary>
        public string TipoDebidaDiligencia { get; set; }
    }
}
