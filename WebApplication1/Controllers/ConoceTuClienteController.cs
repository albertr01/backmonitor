using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;
using WebApplication1.Connections.BD;
using WebApplication1.Models.DB;
using WebApplication1.Models.Request;
using WebApplication1.Models.Response.Local.Salida;
using WebApplication1.Services;
using WebApplication1.Utils;

namespace WebApplication1.Controllers
{
    /// <summary>
    /// Servicios de Conoce a Tu Cliente
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ConoceTuClienteController : ControllerBase
    {
        private readonly ManagementLogs _managementLogs;
        private readonly DebidaDiligenciaMetodos _debidaDiligenciaMetodos;
        private readonly ListaRestrictivaMetodos _listaRestrictivaMetodos;
        private readonly GeminiService _geminiService;

        public ConoceTuClienteController(ILogger<ConoceTuClienteController> logger, MonitoreopyaContext modelContext, GeminiService geminiService, ListaRestrictivaMetodos listaRestrictivaMetodos)
        {
            _managementLogs = new ManagementLogs(logger);
            _debidaDiligenciaMetodos = new(modelContext, logger);
            _listaRestrictivaMetodos = listaRestrictivaMetodos;
            _geminiService = geminiService;
        }


        /// <summary>
        /// Servicio de búsqueda de conoce a tu cliente
        /// </summary>
        /// <param name="tipoIdentificacion">Tipo de identificación</param>
        /// <param name="identificacion">Identificación</param>
        /// <returns><see cref="InformacionGeneralSalida"/></returns>
        [HttpGet("busqueda")]
        [SwaggerResponse(200, type: typeof(InformacionGeneralSalida))]
        public IActionResult Busqueda(string tipoIdentificacion, string identificacion)
        {
            var respuestaDebidaDiligencia = _debidaDiligenciaMetodos.ConsultaPorIdentificacion($"{tipoIdentificacion}{identificacion}");

            return Ok(new InformacionGeneralSalida
            {
                Codigo = "0",
                Descripcion = "Exito",
                DescripcionTécnica = "Exito",
                Error = false,
                NombreCliente = "Pedro Pérez",
                Rif = "19274079-1",
                FechaVencimiento = "26-01-2027",
                FechaDeUltimaActualizacionDatos = "26-01-2027",
                Condicion = "ONG",
                RiesgoInicial = 1.8,
                RiesgoFinal = 1.8,
                DibidaDiligencia = true,
                DibidaDiligenciaVencida = true,
                FechaDebidaDiligencia = new DateTime(2027,1,26),
                Observaciones = "ESTO ES UNA OBSEVARCION",
                TipoDebidaDiligencia = "ESTO ES UN TIPO DE DEBIDA DIILIGENCIA",
                riesgoInicialDetalle = new RiesgoDetalle
                {
                    ZonaGeografica = 2,
                    PaisNacimiento = 2,
                    Nacionalidad = 2,
                    ActividadEconomica = 1,
                    PEP = 0,
                    Profesion = 2,
                    Producto = 2,
                    Canal = 3,
                    MontoPromedio = 3
                },
                riesgoFinalDetalle = new RiesgoDetalle
                {
                    ZonaGeografica = 2,
                    PaisNacimiento = 2,
                    Nacionalidad = 2,
                    ActividadEconomica = 1,
                    PEP = 0,
                    Profesion = 2,
                    Producto = 2,
                    Canal = 3,
                    MontoPromedio = 3
                },
            });
        }

        /// <summary>
        /// Servicio de Guardar Bebida Diligencia
        /// </summary>
        /// <returns><see cref="InformacionGeneralSalida"/></returns>
        [HttpPost("guardarDebidaDiligencia")]
        [SwaggerResponse(200, type: typeof(InformacionGeneralSalida))]
        public IActionResult GuardarBusqueda(DebidaDiligenciaRequest debidaDiligenciaRequest)
        {
            var respuestaDebidaDiligencia = _debidaDiligenciaMetodos.ConsultaPorIdentificacion($"{debidaDiligenciaRequest.TipoIdentificacion}{debidaDiligenciaRequest.Identificacion}");

            if (respuestaDebidaDiligencia is null)
            {
                _debidaDiligenciaMetodos.RegistrarDebidaDiligencia($"{debidaDiligenciaRequest.TipoIdentificacion}{debidaDiligenciaRequest.Identificacion}", debidaDiligenciaRequest.DebidaDiligencia,
                                                                      debidaDiligenciaRequest.DebidaDiligenciaVencida, debidaDiligenciaRequest.FechaDebidaDiligencia, debidaDiligenciaRequest.Observaciones,
                                                                      debidaDiligenciaRequest.TipoDebidaDiligencia);

                return Ok(new GenericoSalida
                {
                    Codigo = "0",
                    Descripcion = "Exito",
                    DescripcionTécnica = "Exito",
                    Error = false
                });
            }

            _debidaDiligenciaMetodos.ModificarDebidaDiligencia(respuestaDebidaDiligencia.Id, $"{debidaDiligenciaRequest.TipoIdentificacion}", debidaDiligenciaRequest.DebidaDiligencia,
                                                                debidaDiligenciaRequest.DebidaDiligenciaVencida, debidaDiligenciaRequest.FechaDebidaDiligencia, debidaDiligenciaRequest.Observaciones,
                                                                debidaDiligenciaRequest.TipoDebidaDiligencia);

            return Ok(new GenericoSalida
            {
                Codigo = "0",
                Descripcion = "Exito",
                DescripcionTécnica = "Exito",
                Error = false
            });
        }
        /// <summary>
        /// Obtiene las relaciones personales
        /// </summary>
        /// <param name="tipoIdentificacion">Tipo de identificacion</param>
        /// <param name="identificacion">Identificacion</param>
        /// <returns></returns>
        [HttpGet("relacionPersonal")]
        [SwaggerResponse(200, type: typeof(RelacionPersonalSalida))]
        public IActionResult ConsultaRelacionPersonal(int tipoIdentificacion, string identificacion)
        {
            return Ok(new RelacionPersonalSalida
            {
                Codigo = "0",
                Descripcion = "Exito",
                DescripcionTécnica = "Exito",
                Error = false,
                clientePersonasNaturals = new List<ClientePersonasNatural>()
                {
                    new ClientePersonasNatural
                    {
                        TipoIdentificacion = "V",
                        Identificacion = "18912019",
                        Nombres = "María Camila",
                        Apellidos = "Moreno Pérez",
                        Ocupacion = "Informática, sistemas y computación / Inversionista y Agencias de Bienes Raíces."
                    }
                },
                clientePersonasJuridicas = new List<ClientePersonasJuridica>()
                {
                    new ClientePersonasJuridica
                    {
                        TipoIdentificacion = "J",
                        Identificacion = "3063077",
                        NombreEmpresa = "Dentons",
                        Ocupacion = "Ejercicio del Derecho (Abogado)"
                    }
                },
                clientePersonasProveedors = new List<ClientePersonasProveedor>
                {
                    new ClientePersonasProveedor
                    {
                        TipoIdentificacion = "J",
                        Identificacion = "3063077",
                        NombreEmpresa = "Dentons",
                        Ocupacion = "Ejercicio del Derecho (Abogado)"
                    }
                },
                empleados = new List<Empleado>()
            });
        }
        /// <summary>
        /// Servicio de consulta de cuentas para seleccionable
        /// </summary>
        /// <param name="tipoIdentificacion">Tipo de identificación</param>
        /// <param name="identificacion">Identificación</param>
        /// <returns><see cref="List{CuentasSalida}"/></returns>
        [HttpGet("consultaDetalleTransaccional")]
        [SwaggerResponse(200, type: typeof(DetalleTransaccionalBusquedaSalida))]
        public IActionResult consultaDetalleTransaccional(int tipoIdentificacion, string identificacion)
        {
            return Ok(new DetalleTransaccionalBusquedaSalida
            {
                cuentas = new List<Cuenta>()
                {
                    new Cuenta
                    {
                        Id = 1,
                        NroCuenta = "1234-5678-9012",
                        tipoCuenta = 1,
                        tipoMoneda = 1
                    },
                    new Cuenta
                    {
                        Id = 2,
                        NroCuenta = "2345-6789-0123",
                        tipoCuenta = 2,
                        tipoMoneda = 1
                    },
                    new Cuenta
                    {
                        Id = 3,
                        NroCuenta = "3456-7890-1234",
                        tipoCuenta = 3,
                        tipoMoneda = 3
                    }
                },
                tipoCuentas = new List<TablaSalida>()
                {
                    new TablaSalida
                    {
                        Id = 1,
                        Codigo = "TCA",
                        Nombre = "Ahorros"
                    },
                    new TablaSalida
                    {
                        Id = 2,
                        Codigo = "TCCO",
                        Nombre = "Corriente"
                    },
                    new TablaSalida
                    {
                        Id = 3,
                        Codigo = "TCCR",
                        Nombre = "Crédito"
                    }
                },
                tipoMonedas = new List<TablaSalida>()
                {
                    new TablaSalida
                    {
                        Id = 1,
                        Codigo = "VEF",
                        Nombre = "VEF"
                    },
                    new TablaSalida
                    {
                        Id = 2,
                        Codigo = "USD",
                        Nombre = "USD"
                    },
                    new TablaSalida
                    {
                        Id = 3,
                        Codigo = "EUR",
                        Nombre = "EUR"
                    }
                }
            });
        }
        /// <summary>
        /// Servicio de consulta de cuentas para seleccionable
        /// </summary>
        /// <param name="tipoCuenta">Identificador de tipo de cuenta</param>
        /// <param name="tipoMoneda">Identificador de tipo de moneda</param>
        /// <param name="nroCuenta">Número de cuenta</param>
        /// <param name="inicioRango">Inicio de rango</param>
        /// <param name="finRango">Fin de rango</param>
        /// <returns><see cref="DetalleTransaccionalSalida"/></returns>
        [HttpGet("detalleTransaccional")]
        [SwaggerResponse(200, type: typeof(DetalleTransaccionalSalida))]
        public IActionResult DetalleTransaccional(int tipoCuenta, int tipoMoneda, string nroCuenta, DateTime inicioRango, DateTime finRango)
        {
            return Ok(new DetalleTransaccionalSalida
            {
                TotalTransacciones = 4,
                ImporteNeto = 1249.6,
                TotalCreditos = 1500.1,
                TotalDebito = 250.5,
                Transaccions = new List<Transaccion>
                {
                    new Transaccion
                    {
                        Fecha = "Feb 1, 2024",
                        CuentaOrigen = "2109-8765-4321",
                        Descripcion = "Depósito",
                        Monto = 1000,
                        EsIngreso = true,
                        Estatus = "COMPLETADO",
                        Referencia = "SAL-001",
                        BancoDestino = "Banplus",
                        Canal = "Online Banking",
                        CedulaEmisor = "V1561561",
                        CedulaReceptor = "V6165454",
                        CuentaDestino = "1234-5678-9012",
                        TipoOperacion = "Deposito"
                    },
                    new Transaccion
                    {
                        Fecha = "Feb 2, 2024",
                        CuentaOrigen = "3210-9876-5432",
                        Descripcion = "Compra Online",
                        Monto = 200,
                        EsIngreso = false,
                        Estatus = "COMPLETADO",
                        Referencia = "PUR-001",
                        BancoDestino = "Banplus",
                        Canal = "Online Banking",
                        CedulaEmisor = "V1561561",
                        CedulaReceptor = "V6165454",
                        CuentaDestino = "1234-5678-9012",
                        TipoOperacion = "Deposito"
                    },
                    new Transaccion
                    {
                        Fecha = "Feb 3, 2024",
                        CuentaOrigen = "2109-8765-4321",
                        Descripcion = "Transferencia Recibida",
                        Monto = 500.1,
                        EsIngreso = true,
                        Estatus = "COMPLETADO",
                        Referencia = "TRF-001",
                        BancoDestino = "Banplus",
                        Canal = "Online Banking",
                        CedulaEmisor = "V1561561",
                        CedulaReceptor = "V6165454",
                        CuentaDestino = "1234-5678-9012",
                        TipoOperacion = "Deposito"
                    },
                    new Transaccion
                    {
                        Fecha = "Feb 4, 2024",
                        CuentaOrigen = "4321-0987-6543",
                        Descripcion = "Pago a Restaurante",
                        Monto = 50.5,
                        EsIngreso = false,
                        Estatus = "PENDIENTE",
                        Referencia = "PAY-001",
                        BancoDestino = "Banplus",
                        Canal = "Online Banking",
                        CedulaEmisor = "V1561561",
                        CedulaReceptor = "V6165454",
                        CuentaDestino = "1234-5678-9012",
                        TipoOperacion = "Deposito"
                    }
                }
            });
        }







        /// <summary>
        /// Servicio de consulta de cuentas para seleccionable
        /// </summary>
        /// <param name="tipoIdentificacion"></param>
        /// <param name="identificacion"></param>
        /// <param name="tipo">NATURAL, JURIDICA, PROVEEDOR, EMPLEADO</param>
        /// <returns><see cref="DetalleTransaccionalBusquedaSalida"/></returns>
        [HttpGet("fichaIdentificacionCliente")]
        [SwaggerResponse(200, type: typeof(DetalleTransaccionalBusquedaSalida))]
        public IActionResult FichaIdentificacionCliente(int tipoIdentificacion, string identificacion, string tipo)
        {
            switch (tipo)
            {
                case "NATURAL":
                    {
                        return Ok(new ConoceTuClienteSalida
                        {
                            fichaClienteNaturalSalida = new Models.Response.Local.Salida.Natural.FichaClienteNaturalSalida
                            {
                                datosPersonalesCliente = new Models.Response.Local.Salida.Natural.DatosPersonalesCliente
                                {
                                    Apellidos = "Perez",
                                    Apto = "123",
                                    Calle = "Bolivar",
                                    CargaFamiliar = "20",
                                    Ciudad = "Caracas",
                                    CodigoPostal = "1563",
                                    CondicionVivienda = "Excelente",
                                    ConyugeFuenteIngresoConyuge = "Trabajo",
                                    ConyugeTipoDocumento = "V",
                                    Correo = "contugue@gmail.com",
                                    Documento = "6156156",
                                    Edificio = "DASF",
                                    Estado = "Miranda",
                                    EstadoCivil = "Soltero",
                                    FechaNacimiento = "12/02/1945",
                                    Genero = "M",
                                    LugarNacimiento = "Caracas",
                                    Nacionalidad = "Venezolana",
                                    NombreCompletoConyuge = "Pepita Perez",
                                    Nombres = "Pepito",
                                    OtraNacionalidad = "No aplica",
                                    PEP = "NO",
                                    PEPAsociado = "No aplica",
                                    PEPAsociadoCargo = "No aplica",
                                    PEPAsociadoIdentificacion = "No aplica",
                                    PEPAsociadoNombreEnteAdministracion = "No aplica",
                                    PEPAsociadoPais = "No aplica",
                                    PEPCliente = "No aplica",
                                    PEPClienteCargo = "No aplica",
                                    PEPClienteIdentificacion = "No aplica",
                                    PEPClienteNombreEnteAdministracion = "No aplica",
                                    PEPClientePais = "No aplica",
                                    PEPParentesco = "No aplica",
                                    PEPParentescoCargo = "No aplica",
                                    PEPParentescoIdentificacion = "No aplica",
                                    PEPParentescoNombreEnteAdministracion = "No aplica",
                                    PEPParentescoPais = "No aplica",
                                    Piso = "6",
                                    Profesion = "Profesor",
                                    RepresentateDatosDocumento = "1616516",
                                    RepresentateDocumento = "124214",
                                    RepresentateLugarFechaNacimiento = "Caracas",
                                    RepresentateNombreCompleto = "Jhon Doe",
                                    RepresentateTelefono = "041564564",
                                    RepresentateTipoDocumento = "V",
                                    Telefono = "21259645",
                                    TipoDocumento = "V",
                                    Urbanizacion = "Bolivar",
                                    ConyugeDocumento = "1561564"
                                },
                                economicoFinanciera = new Models.Response.Local.Salida.Natural.EconomicoFinanciera
                                {
                                    ActividadEconomica = "safasdas",
                                    ActividadEspecifica = "sadsadasdas",
                                    ActividadGeneradoraIngresos = "asfasdsadas",
                                    CategoriaEspecial = "dsadasdsa",
                                    IngresosMensuales = "6516516",
                                    NegocioPropio = "sadasdsada",
                                    NegocioPropioClientes = "sasadsadsadd",
                                    NegocioPropioClientesEstado1 = "dasdas",
                                    NegocioPropioClientesEstado2 = "No aplica",
                                    NegocioPropioClientesEstado3 = "No aplica",
                                    NegocioPropioClientesEstado4 = "No aplica",
                                    NegocioPropioClientesNombre1 = "sadsadsadas",
                                    NegocioPropioClientesNombre2 = "No aplica",
                                    NegocioPropioClientesNombre3 = "No aplica",
                                    NegocioPropioClientesNombre4 = "No aplica",
                                    NegocioPropioClientesPais1 = "sadsadsa",
                                    NegocioPropioClientesPais2 = "No aplica",
                                    NegocioPropioClientesPais3 = "No aplica",
                                    NegocioPropioClientesPais4 = "No aplica",
                                    NegocioPropioDirecionFiscal = "gadfasdasdas",
                                    NegocioPropioFechaConstitucion = "sasafasdfasda",
                                    NegocioPropioIngresoMensual = "56156165",
                                    NegocioPropioNombreEmpresa = "saddasdas",
                                    NegocioPropioProveedores = "asdsadasd",
                                    NegocioPropioProveedoresEstado1 = "fsadsadas",
                                    NegocioPropioProveedoresEstado2 = "No aplica",
                                    NegocioPropioProveedoresEstado3 = "No aplica",
                                    NegocioPropioProveedoresEstado4 = "No aplica",
                                    NegocioPropioProveedoresNombre1 = "asdasdsadsa",
                                    NegocioPropioProveedoresNombre2 = "No aplica",
                                    NegocioPropioProveedoresNombre3 = "No aplica",
                                    NegocioPropioProveedoresNombre4 = "No aplica",
                                    NegocioPropioProveedoresPais1 = "asdsadsad",
                                    NegocioPropioProveedoresPais2 = "No aplica",
                                    NegocioPropioProveedoresPais3 = "No aplica",
                                    NegocioPropioProveedoresPais4 = "No aplica",
                                    NegocioPropioRamoNegocio = "sadsadsa",
                                    NegocioPropioRegistroFolio = "1",
                                    NegocioPropioRegistroLibro = "45",
                                    NegocioPropioRegistroNombre = "asdsadas",
                                    NegocioPropioRegistroNumero = "15",
                                    NegocioPropioRIF = "516165",
                                    NegocioPropioTelefono = "41415619",
                                    RelacionDependencia = "asdsadsadas",
                                    RelacionDependenciaCargoOcupa = "sadsada",
                                    RelacionDependenciaDirecionEmpresa = "asdsadas",
                                    RelacionDependenciaFechaIngreso = "25/08/2012",
                                    RelacionDependenciaNombre = "sadsadsa",
                                    RelacionDependenciaRamoNegocio = "sadsadsa",
                                    RelacionDependenciaRemuneracion = "4961651",
                                    RelacionDependenciaRIF = "156165156",
                                    RelacionDependenciaTelefono = "2125615615",
                                    OtraFuenteIngresos = "safasdfas"
                                },
                                institucionSectorBancario = new Models.Response.Local.Salida.Natural.InstitucionSectorBancario
                                {
                                    Nombre = "sadsadsa",
                                    RIF = "15615665",
                                    Sucursal = "sadsafasfas"
                                },
                                ReferenciaCliente1CifrasPromedios = "15616",
                                ReferenciaCliente1Institucion = "asdsadas",
                                ReferenciaCliente1NombreProducto = "asdsadasda",
                                ReferenciaCliente1NumeroProducto = "sadsadasda",
                                ReferenciaCliente2CifrasPromedios = "No aplica",
                                ReferenciaCliente2Institucion = "No aplica",
                                ReferenciaCliente2NombreProducto = "No aplica",
                                ReferenciaCliente2NumeroProducto = "No aplica",
                                ReferenciaPersonal1Cedula = "1919186",
                                ReferenciaPersonal1Celular = "414156156",
                                ReferenciaPersonal1NombreCompleto = "asdasdsadsa sadas",
                                ReferenciaPersonal1Telefono = "4142165165",
                                ReferenciaPersonal2Cedula = "No aplica",
                                ReferenciaPersonal2Celular = "No aplica",
                                ReferenciaPersonal2NombreCompleto = "No aplica",
                                ReferenciaPersonal2Telefono = "No aplica",
                                servicioCambiario = new Models.Response.Local.Salida.Natural.ServicioCambiario
                                {
                                    Cuenta1Moneda = "USD",
                                    Cuenta1NombreProducto = "ASDASD",
                                    Cuenta1NroProducto = "56161651",
                                    Cuenta2Moneda = "No aplica",
                                    Cuenta2NombreProducto = "No aplica",
                                    Cuenta2NroProducto = "No aplica",
                                    Cuenta3Moneda = "No aplica",
                                    Cuenta3NombreProducto = "No aplica",
                                    Cuenta3NroProducto = "No aplica",
                                    DestinoFondo = "SADSADASD",
                                    Moneda = "USD",
                                    MontoPromedioMensual = "165156654",
                                    Motivo = "ASDSADSA",
                                    NombreProducto = "SADSADA",
                                    NroProducto = "16516516516",
                                    NroPromedioTransaccionesMensualCredito = "15",
                                    NroPromedioTransaccionesMensualDebito = "156",
                                    OrigenFondo = "asdsadsa",
                                    PaisDestino = "sadsadsa",
                                    PaisOrigen = "dasdasdsa",
                                    UsoMonedaVirtual = "SI"
                                },
                                declaracionJurada = new Models.Response.Local.Salida.Natural.DeclaracionJurada
                                {
                                    ElaboradoPor = "Pepe El Grillo",
                                    FechaEjecutivo = "02/02/2015",
                                    FechaGerenteNegocios = "02/03/2015",
                                    FechaGerenteServicios = "02/03/2015",
                                    NombreCompleto = "Pepito Perez",
                                    NombreEjecutivo = "Ejecutivo",
                                    NombreGerenteNegocios = "Gerente de Negocios",
                                    NombreGerenteServicios = "Gerente de Servicios",
                                    VerificadoPor = "Maria Gonzalez"
                                }
                            }
                        });
                    }
                case "JURIDICA":
                    {
                        return Ok(new ConoceTuClienteSalida
                        {
                            fichaClienteJuridicoSalida = new Models.Response.Local.Salida.Juridico.FichaClienteJuridicoSalida
                            {
                                Accionista1Cargo = "Cargo 1",
                                Accionista1Documento = "Documento 1",
                                Accionista1EsPEP = "No aplica",
                                Accionista1Nombre = "Nombre 1",
                                Accionista1PorcentajeCambiario = "10%",
                                Accionista1RelacionadoPEP = "NO",
                                Accionista1TipoDocumento = "V",
                                Accionista2Cargo = "Cargo 2",
                                Accionista2Documento = "Documento 2",
                                Accionista2EsPEP = "No aplica",
                                Accionista2Nombre = "Nombre 2",
                                Accionista2PorcentajeCambiario = "90%",
                                Accionista2RelacionadoPEP = "NO",
                                Accionista2TipoDocumento = "V",
                                Accionista3Cargo = "No aplica",
                                Accionista3Documento = "No aplica",
                                Accionista3EsPEP = "No aplica",
                                Accionista3Nombre = "No aplica",
                                Accionista3PorcentajeCambiario = "No aplica",
                                Accionista3RelacionadoPEP = "No aplica",
                                Accionista3TipoDocumento = "No aplica",
                                Cliente1Estado = "Miranda",
                                Cliente1Nombre = "Nombre 1",
                                Cliente1Pais = "Venezuela",
                                Cliente2Estado = "No aplica",
                                Cliente2Nombre = "No aplica",
                                Cliente2Pais = "No aplica",
                                Cliente3Estado = "No aplica",
                                Cliente3Nombre = "No aplica",
                                Cliente3Pais = "No aplica",
                                Cliente4Estado = "No aplica",
                                Cliente4Nombre = "No aplica",
                                Cliente4Pais = "No aplica",
                                datosIdentificacionEmpresa = new Models.Response.Local.Salida.Juridico.DatosIdentificacionEmpresa
                                {
                                    ActividadEconomica = "Actividad Economica",
                                    ActividadEspecifica = "Activdad Especifica",
                                    Calle = "Calle",
                                    CategoriaEspecial = "Categoria Especial",
                                    Ciudad = "Ciudad",
                                    CodigoPostal = "4565",
                                    Correo = "asdas@sadas.com",
                                    Documento = "621651",
                                    Edificio = "Edificio",
                                    EntePublicoAutoridad = "asdsadas",
                                    EntePublicoCodigoONT = "156156",
                                    EntePublicoNroGaceta = "61561",
                                    EntePublicoFecha = "20/06/1950",
                                    Estado = "Caracas",
                                    Local = "Local",
                                    Municipio = "Municipio",
                                    NombreComercial = "Nombre comercial",
                                    NroFax = "15616515",
                                    Oficina = "Oficina",
                                    Piso = "Piso",
                                    RazonSocial = "Razon social",
                                    RegistroCapitalSocial = "Regisro Capital Social",
                                    RegistroFecha = "11/04/2010",
                                    RegistroFolio = "2",
                                    RegistroNombre = "Nombre",
                                    RegistroNumero = "3",
                                    RegistroTomo = "4",
                                    SitioWeb = "www.sadasd.com",
                                    Telefono = "414212155",
                                    TipoDocumento = "V",
                                    UltimaModificacionCapitalSocial = "15615165$",
                                    UltimaModificacionFecha = "11/04/2010",
                                    UltimaModificacionFolio = "12",
                                    UltimaModificacionNombre = "Nombre",
                                    UltimaModificacionNumero = "15",
                                    UltimaModificacionTomo = "9",
                                    Urbanizacion = "Urbanizacion"
                                },
                                EgresosMensuales = "1561$",
                                EmpresaRelacionada1ActividadEconomica = "asasdfasdas",
                                EmpresaRelacionada1Documento = "516165165",
                                EmpresaRelacionada1Nombre = "sadsadasf",
                                EmpresaRelacionada1TipoDocumento = "V",
                                EmpresaRelacionada2ActividadEconomica = "No aplica",
                                EmpresaRelacionada2Documento = "No aplica",
                                EmpresaRelacionada2Nombre = "No aplica",
                                EmpresaRelacionada2TipoDocumento = "No aplica",
                                EmpresaRelacionada3ActividadEconomica = "No aplica",
                                EmpresaRelacionada3Documento = "No aplica",
                                EmpresaRelacionada3Nombre = "No aplica",
                                EmpresaRelacionada3TipoDocumento = "No aplica",
                                IngresosMensuales = "561561$",
                                institucionSectorBancario = new Models.Response.Local.Salida.Juridico.InstitucionSectorBancario
                                {
                                    Nombre = "ASDASDAS",
                                    RIF = "5161564-5",
                                    Sucursal = "SADASDASDAWQ"
                                },
                                NroEmpleados = "20",
                                NroSubsidarias = "5",
                                PaisMayorPresencia = "SADAFASFAS",
                                PEP1Cargo = "No aplica",
                                PEP1Identificacion = "No aplica",
                                PEP1Nombre = "No aplica",
                                PEP1Pais = "No aplica",
                                PEP2Cargo = "No aplica",
                                PEP2Identificacion = "No aplica",
                                PEP2Nombre = "No aplica",
                                PEP2Pais = "No aplica",
                                PEP3Cargo = "No aplica",
                                PEP3Identificacion = "No aplica",
                                PEP3Nombre = "No aplica",
                                PEP3Pais = "No aplica",
                                Proveedor1Estado = "VBvxageweg",
                                Proveedor1Nombre = "VASDRAsad",
                                Proveedor1Pais = "Psadnofdas",
                                Proveedor2Estado = "No aplica",
                                Proveedor2Nombre = "No aplica",
                                Proveedor2Pais = "No aplica",
                                Proveedor3Estado = "No aplica",
                                Proveedor3Nombre = "No aplica",
                                Proveedor3Pais = "No aplica",
                                Proveedor4Estado = "No aplica",
                                Proveedor4Nombre = "No aplica",
                                Proveedor4Pais = "No aplica",
                                ReferenciaBancaria1CifrasPromedio = "1651$",
                                ReferenciaBancaria1Institucion = "sasfasfdas",
                                ReferenciaBancaria1NombreProducto = "asgvqaweva",
                                ReferenciaBancaria1NroProducto = "1658674621564",
                                ReferenciaBancaria2CifrasPromedio = "No aplica",
                                ReferenciaBancaria2Institucion = "No aplica",
                                ReferenciaBancaria2NombreProducto = "No aplica",
                                ReferenciaBancaria2NroProducto = "No aplica",
                                ReferenciaBancaria3CifrasPromedio = "No aplica",
                                ReferenciaBancaria3Institucion = "No aplica",
                                ReferenciaBancaria3NombreProducto = "No aplica",
                                ReferenciaBancaria3NroProducto = "No aplica",
                                RepresentanteLegal1Cargo = "Vsafqsa",
                                RepresentanteLegal1Condicion = "vasfdsadas",
                                RepresentanteLegal1Documento = "516516515",
                                RepresentanteLegal1EsPEP = "NO",
                                RepresentanteLegal1Nombre = "ADaasfargvx",
                                RepresentanteLegal1RelacionadoPEP = "NO",
                                RepresentanteLegal1TipoDocumento = "V",
                                RepresentanteLegal2Cargo = "No aplica",
                                RepresentanteLegal2Condicion = "No aplica",
                                RepresentanteLegal2Documento = "No aplica",
                                RepresentanteLegal2EsPEP = "No aplica",
                                RepresentanteLegal2Nombre = "No aplica",
                                RepresentanteLegal2RelacionadoPEP = "No aplica",
                                RepresentanteLegal2TipoDocumento = "No aplica",
                                RepresentanteLegal3Cargo = "No aplica",
                                RepresentanteLegal3Condicion = "No aplica",
                                RepresentanteLegal3Documento = "No aplica",
                                RepresentanteLegal3EsPEP = "No aplica",
                                RepresentanteLegal3Nombre = "No aplica",
                                RepresentanteLegal3RelacionadoPEP = "No aplica",
                                RepresentanteLegal3TipoDocumento = "No aplica",
                                servicioCambiario = new Models.Response.Local.Salida.Juridico.ServicioCambiario
                                {
                                    Cuenta1Moneda = "USD",
                                    Cuenta1NombreProducto = "Nombre",
                                    Cuenta1NroProducto = "6516165156",
                                    Cuenta2Moneda = "No aplica",
                                    Cuenta2NombreProducto = "No aplica",
                                    Cuenta2NroProducto = "No aplica",
                                    Cuenta3Moneda = "No aplica",
                                    Cuenta3NombreProducto = "No aplica",
                                    Cuenta3NroProducto = "No aplica",
                                    DestinoFondo = "fasdfsada",
                                    Moneda = "USD",
                                    MontoPromedioMensual = "15461651$",
                                    Motivo = "SAFASDAS",
                                    NombreProducto = "FASFASDSADSA",
                                    NroProducto = "516516515616",
                                    NroPromedioTransaccionesMensualCredito = "1651",
                                    NroPromedioTransaccionesMensualDebito = "1561",
                                    OrigenFondo = "SAFSADSADAS",
                                    PaisDestino = "Pais Destino",
                                    PaisOrigen = "Pais Origen",
                                    UsoMonedaVirtual = "SI"
                                },
                                UltimaDeclaracionISLRAno = "2005",
                                UltimaDeclaracionISLRMonto = "165165",
                                VentasMensuales = "1561",
                                declaracionJurada = new Models.Response.Local.Salida.Juridico.DeclaracionJurada
                                {
                                    ElaboradoPor = "Pepe El Grillo",
                                    FechaEjecutivo = "02/02/2015",
                                    FechaGerenteNegocios = "02/03/2015",
                                    FechaGerenteServicios = "02/03/2015",
                                    NombreCompleto = "Pepito Perez",
                                    NombreEjecutivo = "Ejecutivo",
                                    NombreGerenteNegocios = "Gerente de Negocios",
                                    NombreGerenteServicios = "Gerente de Servicios",
                                    VerificadoPor = "Maria Gonzalez"
                                }
                            }
                        });
                    }
                case "PROVEEDOR":
                    {
                        return Ok(new ConoceTuClienteSalida
                        {
                            fichaClienteProveedorSalida = new Models.Response.Local.Salida.Juridico.FichaClienteJuridicoSalida
                            {
                                Accionista1Cargo = "Cargo 1",
                                Accionista1Documento = "Documento 1",
                                Accionista1EsPEP = "No aplica",
                                Accionista1Nombre = "Nombre 1",
                                Accionista1PorcentajeCambiario = "10%",
                                Accionista1RelacionadoPEP = "NO",
                                Accionista1TipoDocumento = "V",
                                Accionista2Cargo = "Cargo 2",
                                Accionista2Documento = "Documento 2",
                                Accionista2EsPEP = "No aplica",
                                Accionista2Nombre = "Nombre 2",
                                Accionista2PorcentajeCambiario = "90%",
                                Accionista2RelacionadoPEP = "NO",
                                Accionista2TipoDocumento = "V",
                                Accionista3Cargo = "No aplica",
                                Accionista3Documento = "No aplica",
                                Accionista3EsPEP = "No aplica",
                                Accionista3Nombre = "No aplica",
                                Accionista3PorcentajeCambiario = "No aplica",
                                Accionista3RelacionadoPEP = "No aplica",
                                Accionista3TipoDocumento = "No aplica",
                                Cliente1Estado = "Miranda",
                                Cliente1Nombre = "Nombre 1",
                                Cliente1Pais = "Venezuela",
                                Cliente2Estado = "No aplica",
                                Cliente2Nombre = "No aplica",
                                Cliente2Pais = "No aplica",
                                Cliente3Estado = "No aplica",
                                Cliente3Nombre = "No aplica",
                                Cliente3Pais = "No aplica",
                                Cliente4Estado = "No aplica",
                                Cliente4Nombre = "No aplica",
                                Cliente4Pais = "No aplica",
                                datosIdentificacionEmpresa = new Models.Response.Local.Salida.Juridico.DatosIdentificacionEmpresa
                                {
                                    ActividadEconomica = "Actividad Economica",
                                    ActividadEspecifica = "Activdad Especifica",
                                    Calle = "Calle",
                                    CategoriaEspecial = "Categoria Especial",
                                    Ciudad = "Ciudad",
                                    CodigoPostal = "4565",
                                    Correo = "asdas@sadas.com",
                                    Documento = "621651",
                                    Edificio = "Edificio",
                                    EntePublicoAutoridad = "asfasdasf",
                                    EntePublicoFecha = "20/06/1950",
                                    EntePublicoCodigoONT = "1651",
                                    EntePublicoNroGaceta = "4156",
                                    Estado = "Caracas",
                                    Local = "Local",
                                    Municipio = "Municipio",
                                    NombreComercial = "Nombre comercial",
                                    NroFax = "15616515",
                                    Oficina = "Oficina",
                                    Piso = "Piso",
                                    RazonSocial = "Razon social",
                                    RegistroCapitalSocial = "Regisro Capital Social",
                                    RegistroFecha = "11/04/2010",
                                    RegistroFolio = "2",
                                    RegistroNombre = "Nombre",
                                    RegistroNumero = "3",
                                    RegistroTomo = "4",
                                    SitioWeb = "www.sadasd.com",
                                    Telefono = "414212155",
                                    TipoDocumento = "V",
                                    UltimaModificacionCapitalSocial = "15615165$",
                                    UltimaModificacionFecha = "11/04/2010",
                                    UltimaModificacionFolio = "12",
                                    UltimaModificacionNombre = "Nombre",
                                    UltimaModificacionNumero = "15",
                                    UltimaModificacionTomo = "9",
                                    Urbanizacion = "Urbanizacion"
                                },
                                EgresosMensuales = "1561$",
                                EmpresaRelacionada1ActividadEconomica = "asasdfasdas",
                                EmpresaRelacionada1Documento = "516165165",
                                EmpresaRelacionada1Nombre = "sadsadasf",
                                EmpresaRelacionada1TipoDocumento = "V",
                                EmpresaRelacionada2ActividadEconomica = "No aplica",
                                EmpresaRelacionada2Documento = "No aplica",
                                EmpresaRelacionada2Nombre = "No aplica",
                                EmpresaRelacionada2TipoDocumento = "No aplica",
                                EmpresaRelacionada3ActividadEconomica = "No aplica",
                                EmpresaRelacionada3Documento = "No aplica",
                                EmpresaRelacionada3Nombre = "No aplica",
                                EmpresaRelacionada3TipoDocumento = "No aplica",
                                IngresosMensuales = "561561$",
                                institucionSectorBancario = new Models.Response.Local.Salida.Juridico.InstitucionSectorBancario
                                {
                                    Nombre = "ASDASDAS",
                                    RIF = "5161564-5",
                                    Sucursal = "SADASDASDAWQ"
                                },
                                NroEmpleados = "20",
                                NroSubsidarias = "5",
                                PaisMayorPresencia = "SADAFASFAS",
                                PEP1Cargo = "No aplica",
                                PEP1Identificacion = "No aplica",
                                PEP1Nombre = "No aplica",
                                PEP1Pais = "No aplica",
                                PEP2Cargo = "No aplica",
                                PEP2Identificacion = "No aplica",
                                PEP2Nombre = "No aplica",
                                PEP2Pais = "No aplica",
                                PEP3Cargo = "No aplica",
                                PEP3Identificacion = "No aplica",
                                PEP3Nombre = "No aplica",
                                PEP3Pais = "No aplica",
                                Proveedor1Estado = "VBvxageweg",
                                Proveedor1Nombre = "VASDRAsad",
                                Proveedor1Pais = "Psadnofdas",
                                Proveedor2Estado = "No aplica",
                                Proveedor2Nombre = "No aplica",
                                Proveedor2Pais = "No aplica",
                                Proveedor3Estado = "No aplica",
                                Proveedor3Nombre = "No aplica",
                                Proveedor3Pais = "No aplica",
                                Proveedor4Estado = "No aplica",
                                Proveedor4Nombre = "No aplica",
                                Proveedor4Pais = "No aplica",
                                ReferenciaBancaria1CifrasPromedio = "1651$",
                                ReferenciaBancaria1Institucion = "sasfasfdas",
                                ReferenciaBancaria1NombreProducto = "asgvqaweva",
                                ReferenciaBancaria1NroProducto = "1658674621564",
                                ReferenciaBancaria2CifrasPromedio = "No aplica",
                                ReferenciaBancaria2Institucion = "No aplica",
                                ReferenciaBancaria2NombreProducto = "No aplica",
                                ReferenciaBancaria2NroProducto = "No aplica",
                                ReferenciaBancaria3CifrasPromedio = "No aplica",
                                ReferenciaBancaria3Institucion = "No aplica",
                                ReferenciaBancaria3NombreProducto = "No aplica",
                                ReferenciaBancaria3NroProducto = "No aplica",
                                RepresentanteLegal1Cargo = "Vsafqsa",
                                RepresentanteLegal1Condicion = "vasfdsadas",
                                RepresentanteLegal1Documento = "516516515",
                                RepresentanteLegal1EsPEP = "NO",
                                RepresentanteLegal1Nombre = "ADaasfargvx",
                                RepresentanteLegal1RelacionadoPEP = "NO",
                                RepresentanteLegal1TipoDocumento = "V",
                                RepresentanteLegal2Cargo = "No aplica",
                                RepresentanteLegal2Condicion = "No aplica",
                                RepresentanteLegal2Documento = "No aplica",
                                RepresentanteLegal2EsPEP = "No aplica",
                                RepresentanteLegal2Nombre = "No aplica",
                                RepresentanteLegal2RelacionadoPEP = "No aplica",
                                RepresentanteLegal2TipoDocumento = "No aplica",
                                RepresentanteLegal3Cargo = "No aplica",
                                RepresentanteLegal3Condicion = "No aplica",
                                RepresentanteLegal3Documento = "No aplica",
                                RepresentanteLegal3EsPEP = "No aplica",
                                RepresentanteLegal3Nombre = "No aplica",
                                RepresentanteLegal3RelacionadoPEP = "No aplica",
                                RepresentanteLegal3TipoDocumento = "No aplica",
                                servicioCambiario = new Models.Response.Local.Salida.Juridico.ServicioCambiario
                                {
                                    Cuenta1Moneda = "USD",
                                    Cuenta1NombreProducto = "Nombre",
                                    Cuenta1NroProducto = "6516165156",
                                    Cuenta2Moneda = "No aplica",
                                    Cuenta2NombreProducto = "No aplica",
                                    Cuenta2NroProducto = "No aplica",
                                    Cuenta3Moneda = "No aplica",
                                    Cuenta3NombreProducto = "No aplica",
                                    Cuenta3NroProducto = "No aplica",
                                    DestinoFondo = "fasdfsada",
                                    Moneda = "USD",
                                    MontoPromedioMensual = "15461651$",
                                    Motivo = "SAFASDAS",
                                    NombreProducto = "FASFASDSADSA",
                                    NroProducto = "516516515616",
                                    NroPromedioTransaccionesMensualCredito = "1651",
                                    NroPromedioTransaccionesMensualDebito = "1561",
                                    OrigenFondo = "SAFSADSADAS",
                                    PaisDestino = "Pais Destino",
                                    PaisOrigen = "Pais Origen",
                                    UsoMonedaVirtual = "SI"
                                },
                                UltimaDeclaracionISLRAno = "2005",
                                UltimaDeclaracionISLRMonto = "165165",
                                VentasMensuales = "1561",
                                declaracionJurada = new Models.Response.Local.Salida.Juridico.DeclaracionJurada
                                {
                                    ElaboradoPor = "Pepe El Grillo",
                                    FechaEjecutivo = "02/02/2015",
                                    FechaGerenteNegocios = "02/03/2015",
                                    FechaGerenteServicios = "02/03/2015",
                                    NombreCompleto = "Pepito Perez",
                                    NombreEjecutivo = "Ejecutivo",
                                    NombreGerenteNegocios = "Gerente de Negocios",
                                    NombreGerenteServicios = "Gerente de Servicios",
                                    VerificadoPor = "Maria Gonzalez"
                                }
                            }
                        });
                    }
                case "EMPLEADO":
                    {
                        return Ok(new ConoceTuClienteSalida
                        {
                            fichaClienteNaturalSalida = new Models.Response.Local.Salida.Natural.FichaClienteNaturalSalida
                            {
                                datosPersonalesCliente = new Models.Response.Local.Salida.Natural.DatosPersonalesCliente
                                {
                                    Apellidos = "Perez",
                                    Apto = "123",
                                    Calle = "Bolivar",
                                    CargaFamiliar = "20",
                                    Ciudad = "Caracas",
                                    CodigoPostal = "1563",
                                    CondicionVivienda = "Excelente",
                                    ConyugeFuenteIngresoConyuge = "Trabajo",
                                    ConyugeTipoDocumento = "V",
                                    Correo = "contugue@gmail.com",
                                    Documento = "6156156",
                                    Edificio = "DASF",
                                    Estado = "Miranda",
                                    EstadoCivil = "Soltero",
                                    FechaNacimiento = "12/02/1945",
                                    Genero = "M",
                                    LugarNacimiento = "Caracas",
                                    Nacionalidad = "Venezolana",
                                    NombreCompletoConyuge = "Pepita Perez",
                                    Nombres = "Pepito",
                                    OtraNacionalidad = "No aplica",
                                    PEP = "NO",
                                    PEPAsociado = "No aplica",
                                    PEPAsociadoCargo = "No aplica",
                                    PEPAsociadoIdentificacion = "No aplica",
                                    PEPAsociadoNombreEnteAdministracion = "No aplica",
                                    PEPAsociadoPais = "No aplica",
                                    PEPCliente = "No aplica",
                                    PEPClienteCargo = "No aplica",
                                    PEPClienteIdentificacion = "No aplica",
                                    PEPClienteNombreEnteAdministracion = "No aplica",
                                    PEPClientePais = "No aplica",
                                    PEPParentesco = "No aplica",
                                    PEPParentescoCargo = "No aplica",
                                    PEPParentescoIdentificacion = "No aplica",
                                    PEPParentescoNombreEnteAdministracion = "No aplica",
                                    PEPParentescoPais = "No aplica",
                                    Piso = "6",
                                    Profesion = "Profesor",
                                    RepresentateDatosDocumento = "1616516",
                                    RepresentateDocumento = "124214",
                                    RepresentateLugarFechaNacimiento = "Caracas",
                                    RepresentateNombreCompleto = "Jhon Doe",
                                    RepresentateTelefono = "041564564",
                                    RepresentateTipoDocumento = "V",
                                    Telefono = "21259645",
                                    TipoDocumento = "V",
                                    Urbanizacion = "Bolivar",
                                    ConyugeDocumento = "1561564"
                                },
                                economicoFinanciera = new Models.Response.Local.Salida.Natural.EconomicoFinanciera
                                {
                                    ActividadEconomica = "safasdas",
                                    ActividadEspecifica = "sadsadasdas",
                                    ActividadGeneradoraIngresos = "asfasdsadas",
                                    CategoriaEspecial = "dsadasdsa",
                                    IngresosMensuales = "6516516",
                                    NegocioPropio = "sadasdsada",
                                    NegocioPropioClientes = "sasadsadsadd",
                                    NegocioPropioClientesEstado1 = "dasdas",
                                    NegocioPropioClientesEstado2 = "No aplica",
                                    NegocioPropioClientesEstado3 = "No aplica",
                                    NegocioPropioClientesEstado4 = "No aplica",
                                    NegocioPropioClientesNombre1 = "sadsadsadas",
                                    NegocioPropioClientesNombre2 = "No aplica",
                                    NegocioPropioClientesNombre3 = "No aplica",
                                    NegocioPropioClientesNombre4 = "No aplica",
                                    NegocioPropioClientesPais1 = "sadsadsa",
                                    NegocioPropioClientesPais2 = "No aplica",
                                    NegocioPropioClientesPais3 = "No aplica",
                                    NegocioPropioClientesPais4 = "No aplica",
                                    NegocioPropioDirecionFiscal = "gadfasdasdas",
                                    NegocioPropioFechaConstitucion = "sasafasdfasda",
                                    NegocioPropioIngresoMensual = "56156165",
                                    NegocioPropioNombreEmpresa = "saddasdas",
                                    NegocioPropioProveedores = "asdsadasd",
                                    NegocioPropioProveedoresEstado1 = "fsadsadas",
                                    NegocioPropioProveedoresEstado2 = "No aplica",
                                    NegocioPropioProveedoresEstado3 = "No aplica",
                                    NegocioPropioProveedoresEstado4 = "No aplica",
                                    NegocioPropioProveedoresNombre1 = "asdasdsadsa",
                                    NegocioPropioProveedoresNombre2 = "No aplica",
                                    NegocioPropioProveedoresNombre3 = "No aplica",
                                    NegocioPropioProveedoresNombre4 = "No aplica",
                                    NegocioPropioProveedoresPais1 = "asdsadsad",
                                    NegocioPropioProveedoresPais2 = "No aplica",
                                    NegocioPropioProveedoresPais3 = "No aplica",
                                    NegocioPropioProveedoresPais4 = "No aplica",
                                    NegocioPropioRamoNegocio = "sadsadsa",
                                    NegocioPropioRegistroFolio = "1",
                                    NegocioPropioRegistroLibro = "45",
                                    NegocioPropioRegistroNombre = "asdsadas",
                                    NegocioPropioRegistroNumero = "15",
                                    NegocioPropioRIF = "516165",
                                    NegocioPropioTelefono = "41415619",
                                    RelacionDependencia = "asdsadsadas",
                                    RelacionDependenciaCargoOcupa = "sadsada",
                                    RelacionDependenciaDirecionEmpresa = "asdsadas",
                                    RelacionDependenciaFechaIngreso = "25/08/2012",
                                    RelacionDependenciaNombre = "sadsadsa",
                                    RelacionDependenciaRamoNegocio = "sadsadsa",
                                    RelacionDependenciaRemuneracion = "4961651",
                                    RelacionDependenciaRIF = "156165156",
                                    RelacionDependenciaTelefono = "2125615615",
                                    OtraFuenteIngresos = "safasdfas"
                                },
                                institucionSectorBancario = new Models.Response.Local.Salida.Natural.InstitucionSectorBancario
                                {
                                    Nombre = "sadsadsa",
                                    RIF = "15615665",
                                    Sucursal = "sadsafasfas"
                                },
                                ReferenciaCliente1CifrasPromedios = "15616",
                                ReferenciaCliente1Institucion = "asdsadas",
                                ReferenciaCliente1NombreProducto = "asdsadasda",
                                ReferenciaCliente1NumeroProducto = "sadsadasda",
                                ReferenciaCliente2CifrasPromedios = "No aplica",
                                ReferenciaCliente2Institucion = "No aplica",
                                ReferenciaCliente2NombreProducto = "No aplica",
                                ReferenciaCliente2NumeroProducto = "No aplica",
                                ReferenciaPersonal1Cedula = "1919186",
                                ReferenciaPersonal1Celular = "414156156",
                                ReferenciaPersonal1NombreCompleto = "asdasdsadsa sadas",
                                ReferenciaPersonal1Telefono = "4142165165",
                                ReferenciaPersonal2Cedula = "No aplica",
                                ReferenciaPersonal2Celular = "No aplica",
                                ReferenciaPersonal2NombreCompleto = "No aplica",
                                ReferenciaPersonal2Telefono = "No aplica",
                                servicioCambiario = new Models.Response.Local.Salida.Natural.ServicioCambiario
                                {
                                    Cuenta1Moneda = "USD",
                                    Cuenta1NombreProducto = "ASDASD",
                                    Cuenta1NroProducto = "56161651",
                                    Cuenta2Moneda = "No aplica",
                                    Cuenta2NombreProducto = "No aplica",
                                    Cuenta2NroProducto = "No aplica",
                                    Cuenta3Moneda = "No aplica",
                                    Cuenta3NombreProducto = "No aplica",
                                    Cuenta3NroProducto = "No aplica",
                                    DestinoFondo = "SADSADASD",
                                    Moneda = "USD",
                                    MontoPromedioMensual = "165156654",
                                    Motivo = "ASDSADSA",
                                    NombreProducto = "SADSADA",
                                    NroProducto = "16516516516",
                                    NroPromedioTransaccionesMensualCredito = "15",
                                    NroPromedioTransaccionesMensualDebito = "156",
                                    OrigenFondo = "asdsadsa",
                                    PaisDestino = "sadsadsa",
                                    PaisOrigen = "dasdasdsa",
                                    UsoMonedaVirtual = "SI"
                                },
                                declaracionJurada = new Models.Response.Local.Salida.Natural.DeclaracionJurada
                                {
                                    ElaboradoPor = "Pepe El Grillo",
                                    FechaEjecutivo = "02/02/2015",
                                    FechaGerenteNegocios = "02/03/2015",
                                    FechaGerenteServicios = "02/03/2015",
                                    NombreCompleto = "Pepito Perez",
                                    NombreEjecutivo = "Ejecutivo",
                                    NombreGerenteNegocios = "Gerente de Negocios",
                                    NombreGerenteServicios = "Gerente de Servicios",
                                    VerificadoPor = "Maria Gonzalez"
                                }
                            }
                        });
                    }
                default:
                    {
                        return Conflict();
                    }
            }        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="documento"></param>
        /// <param name="name"></param>
        /// <param name="tipoBusqueda"></param>
        /// <returns></returns>
        [HttpGet("ConsultarListRestrictivas")]
        [Authorize]
        public IActionResult ConsultarListRestrictivas(
            [FromQuery] int? documento,
            [FromQuery] string? name,
            [FromQuery] int tipoBusqueda)
        {
            var respuestaListaRestrictiva = _listaRestrictivaMetodos.ConsoltaListaRestrictiva(documento, name, tipoBusqueda);

            return Ok(respuestaListaRestrictiva);

        }


        /// <summary>
        /// Servicio de consulta por Inteligencia Artificial Gemini
        /// </summary>
        /// <param name="Prompt"></param>
        [HttpPost("busquedaIA")]
        public async Task<ActionResult<String>> Post([FromBody] PromptRequest request)
        {
            try
            {
                string promt =  _geminiService.GenerateGeminiPrompt(request.tipoPersona, request.getPrompt());
                //return promt;
                var result = await _geminiService.GetResponseAsync(promt);
                return Ok(new { Text = result });
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return Conflict();
            }


        }

        /// <summary>
        /// Request de Busqueda de IA
        /// </summary>
        public class PromptRequest
        {
            /// <summary>
            /// Natural = 0, Juridico= 1
            /// </summary>
            public int tipoPersona { get; set; }
            /// <summary>
            ///  RIF 
            /// </summary>
            public string? rif { get; set; }
            /// <summary>
            /// Nombre o razon social de la empresa
            /// </summary>
            public string? razonSocial { get; set; }

            /// <summary>
            /// (Opcional) Ubicacion aproximada
            /// </summary>
            public string? ubicacion { get; set; }

            /// <summary>
            /// (Opcional) Persona de interes Publico; Experto de Campo; Famoso
            /// </summary>
            public string? categoria  { get; set; }

            public string? Prompt { get; set; }

            public string getPrompt () {


                if (!string.IsNullOrEmpty(Prompt))
                {
                    return Prompt;
                }

                string tipoPersonaTexto = tipoPersona == 0 ? "persona natural" : "persona jurídica";

                string? promptBase = null;
                if (tipoPersona == 1)
                {
                    promptBase = $" sobre la {tipoPersonaTexto} con RIF {rif} y Razon Social \"{razonSocial}\" ";
                } else
                {
                    promptBase = $"  \"{razonSocial}\"  Documento de identidad Nro. {rif}  ";
                }
                if (!string.IsNullOrEmpty(ubicacion))
                {
                    promptBase += $" Ubicada aproximadamente en {ubicacion}.";
                }

                if (!string.IsNullOrEmpty(categoria))
                {
                    promptBase += $" Identificada como {categoria}.";
                }

                Prompt = promptBase;
                return Prompt;

            }
        }
    }
}
