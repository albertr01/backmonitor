namespace WebApplication1.Models.Response.Local.Salida
{
    public class ListaRiesgosSalida : GenericoSalida
    {
        /// <summary>
        /// Lista de riesgos
        /// </summary>
        public List<Riesgo> Riesgos { get; set; }
    }
    public class Riesgo
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Fecha de creación
        /// </summary>
        public DateTime? FechaCreacion { get; set; }
        /// <summary>
        /// Amenaza
        /// </summary>
        public string Amenaza { get; set; }
        /// <summary>
        /// Vulnerabilidad
        /// </summary>
        public string Vulnerabilidad { get; set; }
        /// <summary>
        /// Consecuencia
        /// </summary>
        public string Consecuencia { get; set; }
        /// <summary>
        /// Identificador del factor de riesgo
        /// </summary>
        public int? IdFactorRiesgo { get; set; }
        /// <summary>
        /// Identificador del sub-factor de riesgo
        /// </summary>
        public int? IdSubFactorRiesgo { get; set; }
        /// <summary>
        /// Identificador del tipo de riesgo
        /// </summary>
        public int? IdTipoRiesgo { get; set; }
        /// <summary>
        /// Causa
        /// </summary>
        public string Causa { get; set; }
        /// <summary>
        /// Identificador de la probabilidad
        /// </summary>
        public int? IdProbabilidad { get; set; }
        /// <summary>
        /// Valor de la probabilidad
        /// </summary>
        public string ProbabilidadValor { get; set; }
        /// <summary>
        /// Identificador del impacto
        /// </summary>
        public int? IdImpacto { get; set; }
        /// <summary>
        /// Valor del impacto
        /// </summary>
        public string ImpactoValor { get; set; }
        /// <summary>
        /// Riesgo inherente
        /// </summary>
        public string RiesgoInherente { get; set; }
        /// <summary>
        /// Severidad
        /// </summary>
        public string Severidad { get; set; }
        /// <summary>
        /// Descripción
        /// </summary>
        public string Descripcion { get; set; }
        /// <summary>
        /// Identificador del tipo de riesgo
        /// </summary>
        public int? IdTipo { get; set; }
        /// <summary>
        /// Identificador de la automatización
        /// </summary>
        public int? IdAutomatizacion { get; set; }
        /// <summary>
        /// Identificador de la frecuencia
        /// </summary>
        public int? IdFrecuencia { get; set; }
        /// <summary>
        /// Valores de control
        /// </summary>
        public string ValoresControl { get; set; }
        /// <summary>
        /// Valor de Riesgo residual
        /// </summary>
        public string RiesgoResidualValor { get; set; }
        /// <summary>
        /// Valor de severidad
        /// </summary>
        public string SeveridadValor { get; set; }
        /// <summary>
        /// Acción
        /// </summary>
        public string Accion { get; set; }
        /// <summary>
        /// Responsable
        /// </summary>
        public string Responsable { get; set; }
        /// <summary>
        /// Identificador del tiempo de ejecución
        /// </summary>
        public int? IdTiempoEjecucion { get; set; }
        /// <summary>
        /// Identificador del tratamiento de riesgo
        /// </summary>
        public int? IdTratamientoRiesgo { get; set; }
    }
}
