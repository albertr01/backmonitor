namespace WebApplication1.Models.Request
{
    public class RiesgoRequest
    {
        /// <summary>
        /// Identificador de riesgo
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Amenazada
        /// </summary>
        public string? Amenaza { get; set; }
        /// <summary>
        /// Vulnerabilidad
        /// </summary>
        public string? Vulnerabilidad { get; set; }
        /// <summary>
        /// Consecuencia
        /// </summary>
        public string? Consecuencia { get; set; }
        /// <summary>
        /// Identificador de factor de riesgo
        /// </summary>
        public int? IdFactorRiesgo { get; set; }
        /// <summary>
        /// Identificador de sub-factor de riesgo
        /// </summary>
        public int? IdSubFactorRiesgo { get; set; }
        /// <summary>
        /// Identificador de tipo de riesgo
        /// </summary>
        public int? IdTipoRiesgo { get; set; }
        /// <summary>
        /// Causa
        /// </summary>
        public string? Causa { get; set; }
        /// <summary>
        /// Identificador de probabilidad
        /// </summary>
        public int? IdProbabilidad { get; set; }
        /// <summary>
        /// Valor de probabilidad
        /// </summary>
        public int? ProbabilidadValor { get; set; }
        /// <summary>
        /// Impacto
        /// </summary>
        public int? Impacto { get; set; }
        /// <summary>
        /// Valor de impacto
        /// </summary>
        public int? ImpactoValor { get; set; }
        /// <summary>
        /// Riego de inherente
        /// </summary>
        public int? RiesgoInherente { get; set; }
        /// <summary>
        /// Severidad
        /// </summary>
        public string? Severidad { get; set; }
        /// <summary>
        /// Descripcion
        /// </summary>
        public string? Descripcion { get; set; }
        /// <summary>
        /// Identificador de tipo
        /// </summary>
        public int? IdTipo { get; set; }
        /// <summary>
        /// Identificador de automatización
        /// </summary>
        public int? IdAutomatizacion { get; set; }
        /// <summary>
        /// Identificador de frecuencia
        /// </summary>
        public int? IdFrecuencia { get; set; }
        /// <summary>
        /// Valores de control
        /// </summary>
        public int? ValoresControl { get; set; }
        /// <summary>
        /// Valor de riesgo residual
        /// </summary>
        public double? RiesgoResidualValor { get; set; }
        /// <summary>
        /// Valor de severidad
        /// </summary>
        public int? SeveridadValor { get; set; }
        /// <summary>
        /// Accion
        /// </summary>
        public string? Accion { get; set; }
        /// <summary>
        /// Responable
        /// </summary>
        public string? Responsable { get; set; }
        /// <summary>
        /// Identificador de tiempo de ejecucion
        /// </summary>
        public int? IdTiempoEjecucion { get; set; }
        /// <summary>
        /// identificador de tratamiento de riesgo
        /// </summary>
        public int? IdTratamientoRiesgo { get; set; }
    }
}
