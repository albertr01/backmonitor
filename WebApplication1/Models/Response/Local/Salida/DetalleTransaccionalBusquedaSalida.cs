namespace WebApplication1.Models.Response.Local.Salida
{
    /// <summary>
    /// Cuentas
    /// </summary>
    public class DetalleTransaccionalBusquedaSalida
    {
        /// <summary>
        /// Cuentas
        /// </summary>
        public List<Cuenta> cuentas { get; set; }
        /// <summary>
        /// Tipos de cuentas
        /// </summary>
        public List<TablaSalida> tipoCuentas { get; set; }
        /// <summary>
        /// Tipo de monedas
        /// </summary>
        public List<TablaSalida> tipoMonedas { get; set; }

    }
    public class Cuenta
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Número de cuenta
        /// </summary>
        public string NroCuenta { get; set; }
        /// <summary>
        /// Tipo de cuenta
        /// </summary>
        public int tipoCuenta { get; set; }
        /// <summary>
        /// Tipo de moneda
        /// </summary>
        public int tipoMoneda { get; set; }
    }
}
