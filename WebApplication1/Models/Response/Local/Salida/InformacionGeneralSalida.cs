namespace WebApplication1.Models.Response.Local.Salida
{
    /// <summary>
    /// Información General
    /// </summary>
    public class InformacionGeneralSalida: GenericoSalida
    {
        /// <summary>
        /// Nombre del cliente
        /// </summary>
        public  string NombreCliente { get; set; }
        /// <summary>
        /// RIF
        /// </summary>
        public  string Rif { get; set; }
        /// <summary>
        /// Fecha de vencimiento. Formato: dd-MM-yyyy
        /// </summary>
        public  string FechaVencimiento { get; set; }
        /// <summary>
        /// Riesgo inicial
        /// </summary>
        public  double RiesgoInicial { get; set; }
        /// <summary>
        /// Riesgo final
        /// </summary>
        public  double RiesgoFinal { get; set; }
        /// <summary>
        /// Indica si tiene debida diligencia
        /// </summary>
        public bool? DibidaDiligencia { get; set; }
        /// <summary>
        /// Indica si tiene debida diligencia vencida
        /// </summary>
        public bool? DibidaDiligenciaVencida { get; set; }
        /// <summary>
        /// Tipo de debida diligencia
        /// </summary>
        public string? TipoDebidaDiligencia { get; set; }
        /// <summary>
        /// Fecha de debida diligencia
        /// </summary>
        public DateTime? FechaDebidaDiligencia { get; set; }
        public string? FechaDeUltimaActualizacionDatos { get; set; }
        /// <summary>
        /// Observaciones
        /// </summary>
        public string? Observaciones { get; set; }
        /// <summary>
        /// Condición
        /// </summary>
        public string? Condicion { get; set; }
        public RiesgoDetalle riesgoInicialDetalle { get; set; }
        public RiesgoDetalle riesgoFinalDetalle { get; set; }
    }
    public class RiesgoDetalle
    {
        public int ZonaGeografica { get; set; }
        public int PaisNacimiento { get; set; }
        public int Nacionalidad { get; set; }
        public int ActividadEconomica { get; set; }
        public int PEP { get; set; }
        public int Profesion { get; set; }
        public int Producto{ get; set; }
        public int Canal { get; set; }
        public int MontoPromedio { get; set; }
    }
}
