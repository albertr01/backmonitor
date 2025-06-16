namespace WebApplication1.Models.Response.Local.Salida.Natural
{
    /// <summary>
    /// Ficha del cliente natural
    /// </summary>
    public class FichaClienteNaturalSalida
    {
        /// <summary>
        /// Institución del sector bancario
        /// </summary>
        public InstitucionSectorBancario institucionSectorBancario { get; set; }
        /// <summary>
        /// Datos personales del cliente
        /// </summary>
        public DatosPersonalesCliente datosPersonalesCliente { get; set; }
        /// <summary>
        /// Institución ReferenciaCleinte1
        /// </summary>
        public string ReferenciaCliente1Institucion { get; set; }
        /// <summary>
        /// Nombre del producto ReferenciaCleinte1
        /// </summary>
        public string ReferenciaCliente1NombreProducto { get; set; }
        /// <summary>
        /// Número del producto ReferenciaCleinte1
        /// </summary>
        public string ReferenciaCliente1NumeroProducto { get; set; }
        /// <summary>
        /// Cifras promedios ReferenciaCleinte1
        /// </summary>
        public string ReferenciaCliente1CifrasPromedios { get; set; }
        /// <summary>
        /// Institución ReferenciaCleinte1
        /// </summary>
        public string ReferenciaCliente2Institucion { get; set; }
        /// <summary>
        /// Nombre del producto ReferenciaCleinte1
        /// </summary>
        public string ReferenciaCliente2NombreProducto { get; set; }
        /// <summary>
        /// Número del producto ReferenciaCleinte1
        /// </summary>
        public string ReferenciaCliente2NumeroProducto { get; set; }
        /// <summary>
        /// Cifras promedios ReferenciaCleinte1
        /// </summary>
        public string ReferenciaCliente2CifrasPromedios { get; set; }
        /// <summary>
        /// Nombre completo ReferenciaPersonal1
        /// </summary>
        public string ReferenciaPersonal1NombreCompleto { get; set; }
        /// <summary>
        /// Cedula ReferenciaPersonal1
        /// </summary>
        public string ReferenciaPersonal1Cedula { get; set; }
        /// <summary>
        /// Telefono ReferenciaPersonal1
        /// </summary>
        public string ReferenciaPersonal1Telefono { get; set; }
        /// <summary>
        /// Celular ReferenciaPersonal1
        /// </summary>
        public string ReferenciaPersonal1Celular { get; set; }
        /// <summary>
        /// Nombre completo ReferenciaPersonal2
        /// </summary>
        public string ReferenciaPersonal2NombreCompleto { get; set; }
        /// <summary>
        /// Cedula ReferenciaPersonal2
        /// </summary>
        public string ReferenciaPersonal2Cedula { get; set; }
        /// <summary>
        /// Telefono ReferenciaPersonal2
        /// </summary>
        public string ReferenciaPersonal2Telefono { get; set; }
        /// <summary>
        /// Celular ReferenciaPersonal2
        /// </summary>
        public string ReferenciaPersonal2Celular { get; set; }
        /// <summary>
        /// Economico Financiera del Cliente
        /// </summary>
        public EconomicoFinanciera economicoFinanciera { get; set; }
        /// <summary>
        /// Servicio Cambiario
        /// </summary>
        public ServicioCambiario servicioCambiario { get; set; }
        /// <summary>
        /// Declaración Jurada
        /// </summary>
        public DeclaracionJurada declaracionJurada { get; set; }
    }
    /// <summary>
    /// Institución del sector bancario
    /// </summary>
    public class InstitucionSectorBancario
    {
        /// <summary>
        /// Nombre
        /// </summary>
        public  string Nombre { get; set; }
        /// <summary>
        /// RIF
        /// </summary>
        public  string RIF { get; set; }
        /// <summary>
        /// Sucursal o Agencia
        /// </summary>
        public  string Sucursal { get; set; }
        public  string NombreGerente { get; set; }
        public  string NombreEjecutivo { get; set; }
    }
    /// <summary>
    /// Datos personales del cliente
    /// </summary>
    public class DatosPersonalesCliente
    {
        /// <summary>
        /// Tipo de documento
        /// </summary>
        public string TipoDocumento { get; set; }
        /// <summary>
        /// Documento
        /// </summary>
        public string Documento { get; set; }
        /// <summary>
        /// Nombres
        /// </summary>
        public string Nombres { get; set; }
        /// <summary>
        /// Apellidos
        /// </summary>
        public string Apellidos { get; set; }
        /// <summary>
        /// Fecha de nacimiento
        /// </summary>
        public string FechaNacimiento { get; set; }
        /// <summary>
        /// Lugar de nacimiento
        /// </summary>
        public string LugarNacimiento { get; set; }
        /// <summary>
        /// Nacionalidad
        /// </summary>
        public string Nacionalidad { get; set; }
        /// <summary>
        /// Otra nacionalidad
        /// </summary>
        public string OtraNacionalidad { get; set; }
        /// <summary>
        /// Genero
        /// </summary>
        public string Genero { get; set; }
        /// <summary>
        /// Profesion
        /// </summary>
        public string Profesion { get; set; }
        /// <summary>
        /// Condición de vivienda
        /// </summary>
        public string CondicionVivienda { get; set; }
        /// <summary>
        /// Carga Familiar
        /// </summary>
        public string CargaFamiliar { get; set; }
        /// <summary>
        /// Estado Civil
        /// </summary>
        public string EstadoCivil { get; set; }
        /// <summary>
        /// Nombre completo del conyuge
        /// </summary>
        public string NombreCompletoConyuge { get; set; }
        /// <summary>
        /// Tipo de documento del conyuge
        /// </summary>
        public string ConyugeTipoDocumento { get; set; }
        /// <summary>
        /// Documento del conyuge
        /// </summary>
        public string ConyugeDocumento { get; set; }
        /// <summary>
        /// Fuente de ingreso conyuge
        /// </summary>
        public string ConyugeFuenteIngresoConyuge { get; set; }
        /// <summary>
        /// Edificio
        /// </summary>
        public string Edificio { get; set; }
        /// <summary>
        /// Piso
        /// </summary>
        public string Piso { get; set; }
        /// <summary>
        /// Apto
        /// </summary>
        public string Apto { get; set; }
        /// <summary>
        /// Calle
        /// </summary>
        public string Calle { get; set; }
        /// <summary>
        /// Urbanización
        /// </summary>
        public string Urbanizacion { get; set; }
        /// <summary>
        /// Ciudad
        /// </summary>
        public string Ciudad { get; set; }
        /// <summary>
        /// Estado
        /// </summary>
        public string Estado { get; set; }
        /// <summary>
        /// Código postal
        /// </summary>
        public string CodigoPostal { get; set; }
        /// <summary>
        /// Teléfono
        /// </summary>
        public string Telefono { get; set; }
        /// <summary>
        /// Correo
        /// </summary>
        public string Correo { get; set; }
        /// <summary>
        /// PEP
        /// </summary>
        public string PEP { get; set; }
        /// <summary>
        /// PEP Cliente
        /// </summary>
        public string PEPCliente { get; set; }
        /// <summary>
        /// Nombre del ente de administración de PEP Cliente
        /// </summary>
        public string PEPClienteNombreEnteAdministracion { get; set; }
        /// <summary>
        /// Cargo de PEP Cliente
        /// </summary>
        public string PEPClienteCargo { get; set; }
        /// <summary>
        /// Pais de PEP Cliente
        /// </summary>
        public string PEPClientePais { get; set; }
        /// <summary>
        /// Identificación de PEP Cliente
        /// </summary>
        public string PEPClienteIdentificacion { get; set; }
        /// <summary>
        /// PEP Parentesco
        /// </summary>
        public string PEPParentesco { get; set; }
        /// <summary>
        /// Nombre del ente de administración de PEP Parentesco
        /// </summary>
        public string PEPParentescoNombreEnteAdministracion { get; set; }
        /// <summary>
        /// Cargo de PEP Parentesco
        /// </summary>
        public string PEPParentescoCargo { get; set; }
        /// <summary>
        /// Pais de PEP Parentesco
        /// </summary>
        public string PEPParentescoPais { get; set; }
        /// <summary>
        /// Identificación de PEP Parentesco
        /// </summary>
        public string PEPParentescoIdentificacion { get; set; }
        /// <summary>
        /// PEP Asociado
        /// </summary>
        public string PEPAsociado { get; set; }
        /// <summary>
        /// Nombre del ente de administración de PEP Asociado
        /// </summary>
        public string PEPAsociadoNombreEnteAdministracion { get; set; }
        /// <summary>
        /// Cargo de PEP Asociado
        /// </summary>
        public string PEPAsociadoCargo { get; set; }
        /// <summary>
        /// Pais de PEP Asociado
        /// </summary>
        public string PEPAsociadoPais { get; set; }
        /// <summary>
        /// Identificación de PEP Asociado
        /// </summary>
        public string PEPAsociadoIdentificacion { get; set; }
        /// <summary>
        /// Tipo de documento de Representate Legal
        /// </summary>
        public string RepresentateTipoDocumento { get; set; }
        /// <summary>
        /// Documento de Representante Legal
        /// </summary>
        public string RepresentateDocumento { get; set; }
        /// <summary>
        /// Nombre completo de Representante Legal
        /// </summary>
        public string RepresentateNombreCompleto { get; set; }
        /// <summary>
        /// Lugar y Fecha de nacimiento de Representante Legal
        /// </summary>
        public string RepresentateLugarFechaNacimiento { get; set; }
        /// <summary>
        /// Telefono de Representante Legal
        /// </summary>
        public string RepresentateTelefono { get; set; }
        /// <summary>
        /// Datos de Documento de Representante Legal
        /// </summary>
        public string RepresentateDatosDocumento { get; set; }

    }
    /// <summary>
    /// Economico Financiera del Cliente
    /// </summary>
    public class EconomicoFinanciera
    {
        /// <summary>
        /// Actividad Economica
        /// </summary>
        public string ActividadEconomica { get; set; }
        /// <summary>
        /// Actividad Especifica
        /// </summary>
        public string ActividadEspecifica { get; set; }
        /// <summary>
        /// Categoria Especial
        /// </summary>
        public string CategoriaEspecial { get; set; }
        /// <summary>
        /// Relacion de dependecia
        /// </summary>
        public string RelacionDependencia { get; set; }
        /// <summary>
        /// Nombre de relacion de dependencia
        /// </summary>
        public string RelacionDependenciaNombre { get; set; }
        /// <summary>
        /// RIF de relacion de dependencia
        /// </summary>
        public string RelacionDependenciaRIF { get; set; }
        /// <summary>
        /// Remunearcion de relacion de dependencia
        /// </summary>
        public string RelacionDependenciaRemuneracion { get; set; }
        /// <summary>
        /// Fecha de ingreso de relacion de dependencia
        /// </summary>
        public string RelacionDependenciaFechaIngreso { get; set; }
        /// <summary>
        /// Cargo que ocupa de relacion de dependencia
        /// </summary>
        public string RelacionDependenciaCargoOcupa { get; set; }
        /// <summary>
        /// Direción de empresa de relacion de dependencia
        /// </summary>
        public string RelacionDependenciaDirecionEmpresa { get; set; }
        /// <summary>
        /// Telefono de relacion de dependencia
        /// </summary>
        public string RelacionDependenciaTelefono { get; set; }
        /// <summary>
        /// Ramo de negocio de relacion de dependencia
        /// </summary>
        public string RelacionDependenciaRamoNegocio { get; set; }
        /// <summary>
        /// Negocio Propio
        /// </summary>
        public string NegocioPropio { get; set; }
        /// <summary>
        /// Nombre de Negocio Propio
        /// </summary>
        public string NegocioPropioNombreEmpresa { get; set; }
        /// <summary>
        /// RIF de Negocio Propio
        /// </summary>
        public string NegocioPropioRIF { get; set; }
        /// <summary>
        /// Ingreso mensual de Negocio Propio
        /// </summary>
        public string NegocioPropioIngresoMensual { get; set; }
        /// <summary>
        /// Fecha de constitucion de Negocio Propio
        /// </summary>
        public string NegocioPropioFechaConstitucion { get; set; }
        /// <summary>
        /// Nombre del registro de Negocio Propio
        /// </summary>
        public string NegocioPropioRegistroNombre { get; set; }
        /// <summary>
        /// Numero del registro de Negocio Propio
        /// </summary>
        public string NegocioPropioRegistroNumero { get; set; }
        /// <summary>
        /// Folio del registro de Negocio Propio
        /// </summary>
        public string NegocioPropioRegistroFolio { get; set; }
        /// <summary>
        /// Libro del registro de Negocio Propio
        /// </summary>
        public string NegocioPropioRegistroLibro { get; set; }
        /// <summary>
        /// Fiscal del registro de Negocio Propio
        /// </summary>
        public string NegocioPropioDirecionFiscal { get; set; }
        /// <summary>
        /// Telefono de Negocio Propio
        /// </summary>
        public string NegocioPropioTelefono { get; set; }
        /// <summary>
        /// Ramo del negocio de Negocio Propio
        /// </summary>
        public string NegocioPropioRamoNegocio { get; set; }
        /// <summary>
        /// Proveedores de Negocio Propio
        /// </summary>
        public string NegocioPropioProveedores { get; set; }
        /// <summary>
        /// Nombre de Proveedor de Negocio Propio uno
        /// </summary>
        public string NegocioPropioProveedoresNombre1 { get; set; }
        /// <summary>
        /// Estado de Proveedor de Negocio Propio uno
        /// </summary>
        public string NegocioPropioProveedoresEstado1 { get; set; }
        /// <summary>
        /// Pais de Proveedor de Negocio Propio uno
        /// </summary>
        public string NegocioPropioProveedoresPais1 { get; set; }
        /// <summary>
        /// Nombre de Proveedor de Negocio Propio dos
        /// </summary>
        public string NegocioPropioProveedoresNombre2 { get; set; }
        /// <summary>
        /// Estado de Proveedor de Negocio Propio dos
        /// </summary>
        public string NegocioPropioProveedoresEstado2 { get; set; }
        /// <summary>
        /// Pais de Proveedor de Negocio Propio dos
        /// </summary>
        public string NegocioPropioProveedoresPais2 { get; set; }
        /// <summary>
        /// Nombre de Proveedor de Negocio Propio tres
        /// </summary>
        public string NegocioPropioProveedoresNombre3 { get; set; }
        /// <summary>
        /// Estado de Proveedor de Negocio Propio tres
        /// </summary>
        public string NegocioPropioProveedoresEstado3 { get; set; }
        /// <summary>
        /// Pais de Proveedor de Negocio Propio tres
        /// </summary>
        public string NegocioPropioProveedoresPais3 { get; set; }
        /// <summary>
        /// Nombre de Proveedor de Negocio Propio cuatro
        /// </summary>
        public string NegocioPropioProveedoresNombre4 { get; set; }
        /// <summary>
        /// Estado de Proveedor de Negocio Propio cuatro
        /// </summary>
        public string NegocioPropioProveedoresEstado4 { get; set; }
        /// <summary>
        /// Pais de Proveedor de Negocio Propio cuatro
        /// </summary>
        public string NegocioPropioProveedoresPais4 { get; set; }
        /// <summary>
        /// Clientes de Negocio Propio
        /// </summary>
        public string NegocioPropioClientes { get; set; }
        /// <summary>
        /// Nombre de Cliente de Negocio Propio uno
        /// </summary>
        public string NegocioPropioClientesNombre1 { get; set; }
        /// <summary>
        /// Estado de Cliente de Negocio Propio uno
        /// </summary>
        public string NegocioPropioClientesEstado1 { get; set; }
        /// <summary>
        /// Pais de Cliente de Negocio Propio uno
        /// </summary>
        public string NegocioPropioClientesPais1 { get; set; }
        /// <summary>
        /// Nombre de Cliente de Negocio Propio dos
        /// </summary>
        public string NegocioPropioClientesNombre2 { get; set; }
        /// <summary>
        /// Estado de Cliente de Negocio Propio dos
        /// </summary>
        public string NegocioPropioClientesEstado2 { get; set; }
        /// <summary>
        /// Pais de Cliente de Negocio Propio dos
        /// </summary>
        public string NegocioPropioClientesPais2 { get; set; }
        /// <summary>
        /// Nombre de Cliente de Negocio Propio tres
        /// </summary>
        public string NegocioPropioClientesNombre3 { get; set; }
        /// <summary>
        /// Estado de Cliente de Negocio Propio tres
        /// </summary>
        public string NegocioPropioClientesEstado3 { get; set; }
        /// <summary>
        /// Pais de Cliente de Negocio Propio tres
        /// </summary>
        public string NegocioPropioClientesPais3 { get; set; }
        /// <summary>
        /// Nombre de Cliente de Negocio Propio cuatro
        /// </summary>
        public string NegocioPropioClientesNombre4 { get; set; }
        /// <summary>
        /// Estado de Cliente de Negocio Propio cuatro
        /// </summary>
        public string NegocioPropioClientesEstado4 { get; set; }
        /// <summary>
        /// Pais de Cliente de Negocio Propio cuatro
        /// </summary>
        public string NegocioPropioClientesPais4 { get; set; }
        /// <summary>
        /// Actividad generadora de ingresos
        /// </summary>
        public string ActividadGeneradoraIngresos { get; set; }
        /// <summary>
        /// Ingresos mensuales
        /// </summary>
        public string IngresosMensuales { get; set; }
        /// <summary>
        /// Otras fuentes de ingresos
        /// </summary>
        public string OtraFuenteIngresos { get; set; }
    }
    /// <summary>
    /// Servicio Cambiario
    /// </summary>
    public class ServicioCambiario
    {
        /// <summary>
        /// Nombre del producto
        /// </summary>
        public string NombreProducto { get; set; }
        /// <summary>
        /// Número del producto
        /// </summary>
        public string NroProducto { get; set; }
        /// <summary>
        /// Moneda
        /// </summary>
        public string Moneda { get; set; }
        /// <summary>
        /// Monto promedio mensual
        /// </summary>
        public string MontoPromedioMensual { get; set; }
        /// <summary>
        /// Número promedio de transacciones Mensuales Crédito
        /// </summary>
        public string NroPromedioTransaccionesMensualCredito { get; set; }
        /// <summary>
        /// Número promedio de transacciones Mensuales Débito
        /// </summary>
        public string NroPromedioTransaccionesMensualDebito { get; set; }
        /// <summary>
        /// Pais de origen
        /// </summary>
        public string PaisOrigen { get; set; }
        /// <summary>
        /// Pais Destino
        /// </summary>
        public string PaisDestino { get; set; }
        /// <summary>
        /// Uso Moneda Virtual
        /// </summary>
        public string UsoMonedaVirtual { get; set; }
        /// <summary>
        /// Nombre de producto Cuenta1
        /// </summary>
        public string Cuenta1NombreProducto { get; set; }
        /// <summary>
        /// Número de producto Cuenta1
        /// </summary>
        public string Cuenta1NroProducto { get; set; }
        /// <summary>
        /// Moneda Cuenta1
        /// </summary>
        public string Cuenta1Moneda { get; set; }
        /// <summary>
        /// Nombre de producto Cuenta2
        /// </summary>
        public string Cuenta2NombreProducto { get; set; }
        /// <summary>
        /// Número de producto Cuenta2
        /// </summary>
        public string Cuenta2NroProducto { get; set; }
        /// <summary>
        /// Moneda Cuenta2
        /// </summary>
        public string Cuenta2Moneda { get; set; }
        /// <summary>
        /// Nombre de producto Cuenta3
        /// </summary>
        public string Cuenta3NombreProducto { get; set; }
        /// <summary>
        /// Número de producto Cuenta3
        /// </summary>
        public string Cuenta3NroProducto { get; set; }
        /// <summary>
        /// Moneda Cuenta3
        /// </summary>
        public string Cuenta3Moneda { get; set; }
        /// <summary>
        /// Motivo
        /// </summary>
        public string Motivo { get; set; }
        /// <summary>
        /// Origen de fondo
        /// </summary>
        public string OrigenFondo { get; set; }
        /// <summary>
        /// Destino de fondo
        /// </summary>
        public string DestinoFondo { get; set; }
    }
    /// <summary>
    /// Declaración Jurada
    /// </summary>
    public class DeclaracionJurada
    {
        /// <summary>
        /// Nombre completo
        /// </summary>
        public string NombreCompleto { get; set; }
        /// <summary>
        /// Elaborado Por
        /// </summary>
        public string ElaboradoPor { get; set; }
        /// <summary>
        /// Verificado Por
        /// </summary>
        public string VerificadoPor { get; set; }
        /// <summary>
        /// Nombre de ejecutivo
        /// </summary>
        public string NombreEjecutivo { get; set; }
        /// <summary>
        /// Fecha de Ejecutivo
        /// </summary>
        public string FechaEjecutivo { get; set; }
        /// <summary>
        /// Nombre de Gerente de Servicios
        /// </summary>
        public string NombreGerenteServicios { get; set; }
        /// <summary>
        /// Fecha de Gerente de Servicios
        /// </summary>
        public string FechaGerenteServicios { get; set; }
        /// <summary>
        /// Nombre de gerente de negocios
        /// </summary>
        public string NombreGerenteNegocios { get; set; }
        /// <summary>
        /// Fecha de gerente de negocios
        /// </summary>
        public string FechaGerenteNegocios { get; set; }
    }
}
