namespace WebApplication1.Models.Response.Local.Salida
{
    /// <summary>
    /// Salida Genérica
    /// </summary>
    public class GenericoSalida
    {
        /// <summary>
        /// Código de salida
        /// 0 = Exitoso
        /// 1 = Error de API
        /// 2 = Error de tercero
        /// </summary>
        public string? Codigo { get; set; }
        /// <summary>
        /// Mensaje al usuario
        /// </summary>
        public string? Descripcion { get; set; }
        /// <summary>
        /// Mensaje técnico
        /// </summary>
        public string? DescripcionTécnica { get; set; }
        /// <summary>
        /// Indica si existe error o no
        /// </summary>
        public bool? Error { get; set; }
    }
}
