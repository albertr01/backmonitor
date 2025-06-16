namespace WebApplication1.Models.Response.Local.Salida
{
    public class RiesgoGuardarSalida: GenericoSalida
    {
        /// <summary>
        /// Id de riesgo
        /// </summary>
        public int IdRiesgo { get; set; }
        public int IdPeriodo { get; set; }
        public List<SeleccionableSalida>? selecionableAutomatizacion { get; set; }
        public List<SeleccionableSalida>? selecionableFrecuencia { get; set; }
        public List<SeleccionableSalida>? selecionableTipoRiesgo { get; set; }
        public List<SeleccionableSalida>? selecionableImpacto { get; set; }
        public List<SeleccionableSalida>? selecionableProbabilidad { get; set; }
        public List<SeleccionableSalida>? selecionableFactorRiesgo { get; set; }
        public List<SeleccionableSalida>? selecionableSubFactorRiesgo { get; set; }
        public List<SeleccionableSalida>? selecionableTipoRiesgoR { get; set; }
    }
}
