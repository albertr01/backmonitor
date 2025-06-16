namespace WebApplication1.Models.Response.Local.Salida
{
    public class ListaRiesgosMapaCalorSalida : GenericoSalida
    {
        /// <summary>
        /// Lista de riesgos
        /// </summary>
        public List<RiesgoMapaCalor> Riesgos { get; set; }
    }
    public class RiesgoMapaCalor
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public int Id { get; set; }
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
    }
}
