namespace WebApplication1.Models.Response.Local.Salida
{
    /// <summary>
    /// Relación personal
    /// </summary>
    public class RelacionPersonalSalida : GenericoSalida
    {
        /// <summary>
        /// Clientes naturales
        /// </summary>
        public  List<ClientePersonasNatural> clientePersonasNaturals { get; set; }
        /// <summary>
        /// Clientes juridicos 
        /// </summary>
        public  List<ClientePersonasJuridica> clientePersonasJuridicas { get; set; }
        /// <summary>
        /// Clientes proveedores
        /// </summary>
        public  List<ClientePersonasProveedor> clientePersonasProveedors { get; set; }
        /// <summary>
        /// Empleados
        /// </summary>
        public  List<Empleado> empleados { get; set; }
    }
    /// <summary>
    /// Cliente persona natural
    /// </summary>
    public class ClientePersonasNatural
    {
        /// <summary>
        /// Tipo de identificación
        /// </summary>
        public  string TipoIdentificacion { get; set; }
        /// <summary>
        /// Identificación
        /// </summary>
        public  string Identificacion { get; set; }
        /// <summary>
        /// Nombres
        /// </summary>
        public  string Nombres { get; set; }
        /// <summary>
        /// Apellidos
        /// </summary>
        public  string Apellidos { get; set; }
        /// <summary>
        /// Ocupación
        /// </summary>
        public  string Ocupacion { get; set; }
    }
    public class ClientePersonasJuridica
    {
        /// <summary>
        /// Tipo de identificación
        /// </summary>
        public  string TipoIdentificacion { get; set; }
        /// <summary>
        /// Identificación
        /// </summary>
        public  string Identificacion { get; set; }
        /// <summary>
        /// Nombre de empresa
        /// </summary>
        public  string NombreEmpresa { get; set; }
        /// <summary>
        /// Ocupación
        /// </summary>
        public  string Ocupacion { get; set; }
    }
    public class ClientePersonasProveedor
    {
        /// <summary>
        /// Tipo de identificación
        /// </summary>
        public  string TipoIdentificacion { get; set; }
        /// <summary>
        /// Identificación
        /// </summary>
        public  string Identificacion { get; set; }
        /// <summary>
        /// Nombre de empresa
        /// </summary>
        public  string NombreEmpresa { get; set; }
        /// <summary>
        /// Ocupación
        /// </summary>
        public  string Ocupacion { get; set; }
    }
    public class Empleado
    {
        /// <summary>
        /// Tipo de identificación
        /// </summary>
        public  string TipoIdentificacion { get; set; }
        /// <summary>
        /// Identificación
        /// </summary>
        public  string Identificacion { get; set; }
        /// <summary>
        /// Nombres
        /// </summary>
        public  string Nombres { get; set; }
        /// <summary>
        /// Apellidos
        /// </summary>
        public  string Apellidos { get; set; }
        /// <summary>
        /// Ocupación
        /// </summary>
        public  string Ocupacion { get; set; }
    }
}
