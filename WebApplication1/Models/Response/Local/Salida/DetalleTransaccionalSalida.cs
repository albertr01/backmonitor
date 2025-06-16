namespace WebApplication1.Models.Response.Local.Salida
{
    /// <summary>
    /// Detalle de transaccion
    /// </summary>
    public class DetalleTransaccionalSalida
    {
        /// <summary>
        /// Total de transacciones
        /// </summary>
        public  int TotalTransacciones { get; set; }
        /// <summary>
        /// Importe neto
        /// </summary>
        public  double ImporteNeto { get; set; }
        /// <summary>
        /// Total de credito
        /// </summary>
        public  double TotalCreditos { get; set; }
        /// <summary>
        /// Total de debitos
        /// </summary>
        public  double TotalDebito { get; set; }
        /// <summary>
        /// Transacciones
        /// </summary>
        public  List<Transaccion> Transaccions { get; set; }
    }
    /// <summary>
    /// Transaccion
    /// </summary>
    public class Transaccion
    {
        /// <summary>
        /// Fecha. Formato: MMM dd, yyyy
        /// </summary>
        public  string Fecha { get; set; }
        /// <summary>
        /// Número de cuenta
        /// </summary>
        public  string CuentaOrigen { get; set; }
        public  string CuentaDestino { get; set; }
        public  string Canal { get; set; }
        public  string TipoOperacion { get; set; }
        public  string CedulaEmisor { get; set; }
        public  string CedulaReceptor { get; set; }
        public  string BancoDestino { get; set; }
        /// <summary>
        /// Descripción
        /// </summary>
        public  string Descripcion { get; set; }
        /// <summary>
        /// Monto
        /// </summary>
        public  double Monto { get; set; }
        /// <summary>
        /// Indica si es ingreso o egreso
        /// </summary>
        public  bool EsIngreso { get; set; }
        /// <summary>
        /// Estatus. Posibles valores: COMPLETADO, PENDIENTE
        /// </summary>
        public  string Estatus { get; set; }
        /// <summary>
        /// Referencia
        /// </summary>
        public  string Referencia { get; set; }
    }
}
