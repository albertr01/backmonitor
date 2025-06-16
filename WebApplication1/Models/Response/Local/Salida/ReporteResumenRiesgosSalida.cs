namespace WebApplication1.Models.Response.Local.Salida
{
    public class ReporteResumenRiesgosSalida : GenericoSalida
    {
        /// <summary>
        /// Cantidad tolal de riesgos
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// Lista de factores de riesgo
        /// </summary>
        public List<FactorRiesgo> FactorRiesgos { get; set; }
    }
    public class FactorRiesgo
    {
        /// <summary>
        /// Identificador de factor de riesgo
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// Nombre de factor de riesgo
        /// </summary>
        public string nombre { get; set; }
        /// <summary>
        /// Cantidad de riesgos para factor de riesgo
        /// </summary>
        public int Cantidad { get; set; }
        /// Lista de subfactores de riesgo
        /// </summary>
        public List<SubFactorRiesgo>? SubFactores { get; set; }
    }

    public class SubFactorRiesgo
    {
        /// <summary>
        /// Identificador de subfactor de riesgo
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// Nombre de subfactor de riesgo
        /// </summary>
        public string nombre { get; set; }
        /// <summary>
        /// Cantidad de riesgos para subfactor de riesgo
        /// </summary>
        public int Cantidad { get; set; }
    }
}
