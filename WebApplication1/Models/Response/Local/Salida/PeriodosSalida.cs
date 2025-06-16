namespace WebApplication1.Models.Response.Local.Salida
{
    /// <summary>
    /// Salida de periodos
    /// </summary>
    public class PeriodosSalida : GenericoSalida
    {
        /// <summary>
        /// Periodos
        /// </summary>
        public List<Periodo> periodos { get; set; }
    }
    /// <summary>
    /// Periodo
    /// </summary>
    public class Periodo
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public int Id { get; set; }
        public string NombreEvaluacion { get; set; }
        /// <summary>
        /// Tipo de periodo
        /// </summary>
        public string TipoPeriodo { get; set; }
        /// <summary>
        /// Año del periodo
        /// </summary>
        public string PeriodoAno { get; set; }
        /// <summary>
        /// Fecha de Creación
        /// </summary>
        public DateTime? FechaCreacion { get; set; }
        /// <summary>
        /// Fecha de cierre
        /// </summary>
        public DateTime? FechaCierre { get; set; }
        /// <summary>
        /// Fecha de Inicio
        /// </summary>
        public DateTime? FechaInicio { get; set; }
        /// <summary>
        /// Fecha Fin
        /// </summary>
        public DateTime? FechaFin { get; set; }
        /// <summary>
        /// Estatus. ACTIVO, CERRADO
        /// </summary>
        public string Estatus { get; set; }
    }
}
