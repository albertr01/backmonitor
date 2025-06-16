using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace WebApplication1.Controllers
{

    [ApiController]
    [Route("api/indicadores")]
    public class ReportesController : Controller
    {


        [HttpGet]
        public IActionResult ObtenerIndicadores([FromQuery] string desde, string hasta)
        {

            //alertas por mes
            var listaAlertasResultado = new ListaAlertasResultado
            {
                Anio = 2025,
                Alertas = new List<AlertaDetalle>()
            };

            //alertas por mes
            var listaAlertasAtenderResultado = new ListaAlertasResultado
            {
                Anio = 2025,
                Alertas = new List<AlertaDetalle>()
            };

            //alertas por mes
            var listaAlertasTipoAResultado = new ListaAlertasResultado
            {
                Anio = 2025,
                Alertas = new List<AlertaDetalle>()
            };

            var nombresAlertas = new List<string> { "Generadas  en el Mes", "Pendientes mes anterior" };

            var nombresAlertasPendiendes = new List<string> { "Pendientas en Análisis", "En Seguimiento" };

            var nombresAlertaTipoA = new List<string> { "Cliente_pep", "Debida_diligencia", "Tx_supera_limite", "Tx_no_usual" };

            var random = new Random();

            foreach (var nombreAlerta in nombresAlertas)
            {
                var alertaDetalle = new AlertaDetalle
                {
                    NombreAlerta = nombreAlerta,

                    CantidadesPorMes = new List<AlertaMensualCantidad>()
                };

                for (int mes = 1; mes <= 12; mes++)
                {
                    alertaDetalle.CantidadesPorMes.Add(new AlertaMensualCantidad
                    {
                        Mes = mes,
                        Cantidad = random.Next(5, 50) // Genera una cantidad aleatoria para cada mes
                    });
                }
                listaAlertasResultado.Alertas.Add(alertaDetalle);
            }

            //alertas pendientes detalle

            foreach (var nombreAlerta in nombresAlertasPendiendes)
            {
                var alertaDetallePendiente = new AlertaDetalle
                {
                    NombreAlerta = nombreAlerta,

                    CantidadesPorMes = new List<AlertaMensualCantidad>()
                };

                for (int mes = 1; mes <= 12; mes++)
                {
                    alertaDetallePendiente.CantidadesPorMes.Add(new AlertaMensualCantidad
                    {
                        Mes = mes,
                        Cantidad = random.Next(5, 50) // Genera una cantidad aleatoria para cada mes
                    });
                }
                listaAlertasAtenderResultado.Alertas.Add(alertaDetallePendiente);
            }

            //alertas atender detalle

            foreach (var nombreAlerta in nombresAlertaTipoA)
            {
                var alertaDetalleAtender = new AlertaDetalle
                {
                    NombreAlerta = nombreAlerta,

                    CantidadesPorMes = new List<AlertaMensualCantidad>()
                };

                for (int mes = 1; mes <= 12; mes++)
                {
                    alertaDetalleAtender.CantidadesPorMes.Add(new AlertaMensualCantidad
                    {
                        Mes = mes,
                        Cantidad = random.Next(5, 50) // Genera una cantidad aleatoria para cada mes
                    });
                }
                listaAlertasTipoAResultado.Alertas.Add(alertaDetalleAtender);
            }

            //clasificacion 
            var calificacionesAnuales = new CalificacionRiesgoAnual
            {
                Anio = 2025,
                CalificacionesPorMes = new List<ClientesPorMes>()
            };

            // Generar datos de ejemplo para cada mes
            for (int mes = 1; mes <= 12; mes++)
            {
                var clientesMes = new ClientesPorMes
                {
                    Mes = mes,
                    Naturales = new CalificacionRiesgoCliente
                    {
                        Alto = new Random().Next(10, 30),   // Ejemplo de valores aleatorios
                        Medio = new Random().Next(25, 50),
                        Moderado = new Random().Next(40, 70)
                    },
                    Juridicos = new CalificacionRiesgoCliente
                    {
                        Alto = new Random().Next(3, 15),
                        Medio = new Random().Next(8, 25),
                        Moderado = new Random().Next(15, 40)
                    }
                };
                calificacionesAnuales.CalificacionesPorMes.Add(clientesMes);
            }

            var respuesta = new IndicadoresResponseDto
            {
                AlertasPeriodo = new AlertasPeriodoDto { Total = 200, Analizadas = 150, Pendientes = 50 },

                Ras = new AlertasPeriodoDto { Total = 200, Analizadas = 150, Pendientes = 50 },

                ClientesRiesgo = new ClientesRiesgoDto { Series = new List<int> { 12, 50, 125 }, Labels = new List<string> { "Alto", "Medio", "Bajo" } },

                ClientesTipo = new ClientesTipoDto { Series = new List<int> { 75, 125 }, Labels = new List<string> { "Persona Natural", "Persona Juridica" } },

                AlertasPorMes = new AlertasPorMesDto
                {
                    Labels = new List<string> { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" },
                    Datasets = new List<AlertasPorMesDatasetDto>
                {
                    new AlertasPorMesDatasetDto { Label = "Atendidas", BackgroundColor = "green", Data = new List<int> { 45, 50, 40, 55, 60, 42, 38, 50, 65, 70, 80, 75 } },
                    new AlertasPorMesDatasetDto { Label = "Pendientes", BackgroundColor = "red", Data = new List<int> { 20, 18, 25, 15, 10, 22, 30, 20, 12, 8, 5, 10 } }
                }
                },

                TotalAlertasPorTipo = new Dictionary<string, int>
                {
                    { "Cliente_pep", 11 },
                    { "Debida_diligencia", 14 },
                    { "Tx_supera_limite", 160 },
                    { "Tx_no_usual", 15 }
                },

                TotalAlertasPorTipoPendientes = new Dictionary<string, int>
                {
                    { "Cliente_pep", 8 },
                    { "Debida_diligencia", 13 },
                    { "Tx_Supera_limite", 17 },
                    { "Tx_no_usual", 12 }
                },

                TotalAlertasAnalizadas = new Dictionary<string, int>
                {
                    { "Desestimadas", 17 },
                    { "Cerrados", 125 },
                    { "Evaluacion_RAS", 8 }
                },

                GestionAlertasAnalistas = new GestionAlertasAnalistasDto
                {
                    Data = new List<GestionAnalistaDto>
                {
                    new GestionAnalistaDto { Analista = "Jesus Lopez", Asignadas = 20, EnAnalisis = 14, Desestimadas = 20, EnSeguimiento = 10,  Cerradas = 17, EvaluacionRas = 2, Pendientes = 7, Atendidas = 37, Efectividad = 84 },
                    new GestionAnalistaDto { Analista = "Maria Perez", Asignadas = 15, EnAnalisis = 10, Desestimadas = 10, EnSeguimiento = 8, Cerradas = 12, EvaluacionRas = 1, Pendientes = 5, Atendidas = 28, Efectividad = 78 }
                }
                },

                ClientesActualizaciónDatosVencida = new ClientesActualizaciónDatosVencida { ClientesRiegoAlto = 200, ClientesRiesgoBajo = 150, ClientesRiesgoModerado = 50, Total = 400 },

                ClientesDebidaDiligencia = new ClientesDebidaDiligenciaDto { Vigente = 200, Vencida = 150, SinDebidaDiligencia = 50, Total = 400 },

                ClientesPaisesAltoRiesgo = new ClientesPaisesAltoRiesgoDto
                {
                    Paises = new List<PaisDto>
                {
                    new PaisDto { Nombre = "Haiti", Cantidad = 2, Porcentaje = 20 },
                    new PaisDto { Nombre = "Monaco", Cantidad = 2, Porcentaje = 20 },
                    new PaisDto { Nombre = "Iran", Cantidad = 2, Porcentaje = 20 },
                    new PaisDto { Nombre = "Bulgaria", Cantidad = 2, Porcentaje = 20 }
                }
                },

                ClientesNacionalidadAltoRiesgo = new ClientesNacionalidadAltoRiesgoDto
                {
                    Paises = new List<PaisDto>
                {
                    new PaisDto { Nombre = "Haitiano", Cantidad = 2, Porcentaje = 20 },
                    new PaisDto { Nombre = "Dominicano", Cantidad = 2, Porcentaje = 20 },
                    new PaisDto { Nombre = "Irani", Cantidad = 2, Porcentaje = 20 },
                    new PaisDto { Nombre = "Español", Cantidad = 2, Porcentaje = 20 }
                }
                },

                ConsultaOrganismos = new ConsultaOrganismosDto
                {
                    Organismos = new List<PaisDto>
                    {
                        new PaisDto { Nombre = "SUDEBAN", Cantidad = 2, Porcentaje = 20 },
                        new PaisDto { Nombre = "OFAC", Cantidad = 2, Porcentaje = 20 }
                    }
                },

                factorRiesgoZonaInherente = new List<RespuestaDTO>
                {
                     new RespuestaDTO
                        {
                            Id = 123,
                            Estado = "Miranda",
                            CantidadAgencias = 5,
                            CantNaturalBajo = 10,
                            CantNaturalMedio = 25,
                            CantNaturalAlto = 5,
                            CantNaturalSn = 2,
                            JurNaturalBajo = 3,
                            JurNaturalMedio = 15,
                            JurNaturalAlto = 7,
                            JurNaturalSn = 1,
                            Total = 68
                        },
                     new RespuestaDTO
                        {
                            Id = 123,
                            Estado = "DC",
                            CantidadAgencias = 5,
                            CantNaturalBajo = 10,
                            CantNaturalMedio = 25,
                            CantNaturalAlto = 5,
                            CantNaturalSn = 2,
                            JurNaturalBajo = 3,
                            JurNaturalMedio = 15,
                            JurNaturalAlto = 7,
                            JurNaturalSn = 1,
                            Total = 68
                        },
                },

                riesgoCanalesDistribucions = new List<CanalesDistribucion>
                {
                    new CanalesDistribucion
                    {
                        Id = 2,
                        Denominacion = "Banca por Internet",
                        Descripcion = "Canal de distribución digital.",
                        Riesgo = "Medio"
                    },
                    new CanalesDistribucion
                    {
                        Id = 1,
                        Denominacion = "Agencia Principal",
                        Descripcion = "Canal de distribución principal a nivel nacional.",
                        Riesgo = "Bajo"
                    }
                },

                subFactorProductosServicios = new List<SubFactorProductosServicios> {
                    new SubFactorProductosServicios
                    {
                        Id = 1,
                        DenominacionDelProductoOServicio = "Cuenta Corriente Plus",
                        FechaDeAutorizacionDeLaSUDEBAN = new DateTime(2023, 10, 26),
                        Tipo = "Cuenta de Depósito",
                        Descripcion = "Cuenta corriente con beneficios adicionales.",
                        CalificacionDeRiesgo = "Bajo"
                    }
                },

                clientesPorProductoDtos = new List<ClientesPorProductoDto> {
                    new ClientesPorProductoDto {
                        id = 1,
                        Nombre = "Cuenta ahorro",
                        Cantidad = 5800
                    },
                    new ClientesPorProductoDto {
                        id = 1,
                        Nombre = "Cuenta Corriente",
                        Cantidad = 4500
                    }
                },

                calificacionRiesgoAnual = calificacionesAnuales,

                listaAlertasGeneradasPorMes = listaAlertasResultado,

                listaAlertasprocesadasPorMes = listaAlertasResultado,

                listaAlertasTipoMensualPorMes = listaAlertasTipoAResultado,

                listaAlertasPorAtenderPorMes = listaAlertasAtenderResultado,

                listaAlertasPendiendesPorMes = listaAlertasTipoAResultado

            };

            return Ok(respuesta);
        }
    }

    public class IndicadoresResponseDto
    {
        public AlertasPeriodoDto AlertasPeriodo { get; set; }
        public AlertasPeriodoDto Ras { get; set; }
        public ClientesRiesgoDto ClientesRiesgo { get; set; }
        public ClientesTipoDto ClientesTipo { get; set; }
        public AlertasPorMesDto AlertasPorMes { get; set; }
        public Dictionary<string, int> TotalAlertasPorTipo { get; set; }
        public Dictionary<string, int> TotalAlertasPorTipoPendientes { get; set; }
        public Dictionary<string, int> TotalAlertasAnalizadas { get; set; }
        public GestionAlertasAnalistasDto GestionAlertasAnalistas { get; set; }
        public ClientesActualizaciónDatosVencida ClientesActualizaciónDatosVencida { get; set; }
        public ClientesDebidaDiligenciaDto ClientesDebidaDiligencia { get; set; }
        public ClientesPaisesAltoRiesgoDto ClientesPaisesAltoRiesgo { get; set; }
        public ClientesNacionalidadAltoRiesgoDto ClientesNacionalidadAltoRiesgo { get; set; }
        public ConsultaOrganismosDto ConsultaOrganismos { get; set; }
        public List<RespuestaDTO> factorRiesgoZonaInherente { get; set; }
        public List<CanalesDistribucion> riesgoCanalesDistribucions { get; set; }
        public List<SubFactorProductosServicios> subFactorProductosServicios { get; set; }
        public List<ClientesPorProductoDto> clientesPorProductoDtos { get; set; }
        public CalificacionRiesgoAnual calificacionRiesgoAnual { get; set; }
        public ListaAlertasResultado listaAlertasGeneradasPorMes { get; set; }

        public ListaAlertasResultado listaAlertasprocesadasPorMes { get; set; }

        public ListaAlertasResultado listaAlertasTipoMensualPorMes { get; set; }

        public ListaAlertasResultado listaAlertasPorAtenderPorMes { get; set; }

        public ListaAlertasResultado listaAlertasPendiendesPorMes { get; set; }
    }

    public class AlertasPeriodoDto
    {
        public int Total { get; set; }
        public int Analizadas { get; set; }
        public int Pendientes { get; set; }
    }

    public class ClientesRiesgoDto
    {
        public List<int> Series { get; set; }
        public List<string> Labels { get; set; }
    }

    public class ClientesTipoDto
    {
        public List<int> Series { get; set; }
        public List<string> Labels { get; set; }
    }

    public class AlertasPorMesDto
    {
        public List<string> Labels { get; set; }
        public List<AlertasPorMesDatasetDto> Datasets { get; set; }
    }

    public class AlertasPorMesDatasetDto
    {
        public string Label { get; set; }
        public string BackgroundColor { get; set; }
        public List<int> Data { get; set; }
    }

    public class GestionAlertasAnalistasDto
    {
        public List<GestionAnalistaDto> Data { get; set; }
    }

    public class GestionAnalistaDto
    {
        public string Analista { get; set; }
        public int Asignadas { get; set; }
        public int EnAnalisis { get; set; }
        public int Desestimadas { get; set; }
        public int EnSeguimiento { get; set; }
        public int Cerradas { get; set; }
        public int EvaluacionRas { get; set; }
        public int Pendientes { get; set; }
        public int Atendidas { get; set; }
        public int Efectividad { get; set; }
    }

    public class ClientesDebidaDiligenciaDto/************Borrar esta linea es necesario**********/
    {
        public int Vigente { get; set; }
        public int Vencida { get; set; }
        public int SinDebidaDiligencia { get; set; }
        public int Total { get; set; }
    }

    public class ClientesActualizaciónDatosVencida
    {
        public int ClientesRiegoAlto { get; set; }
        public int ClientesRiesgoModerado { get; set; }
        public int ClientesRiesgoBajo { get; set; }
        public int Total { get; set; }
    }

    public class ClientesPaisesAltoRiesgoDto
    {
        public List<PaisDto> Paises { get; set; }
    }

    public class ClientesNacionalidadAltoRiesgoDto
    {
        public List<PaisDto> Paises { get; set; }
    }

    public class ConsultaOrganismosDto
    {
        public List<PaisDto> Organismos { get; set; }
    }

    public class PaisDto
    {
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public int Porcentaje { get; set; }
    }

    public class ClientesPorProductoDto
    {
        public int id { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
    }

    public class RespuestaDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("estado")]
        public string Estado { get; set; }

        [JsonPropertyName("cantidadAgencias")]
        public int CantidadAgencias { get; set; }

        [JsonPropertyName("cant_natural_bajo")]
        public int CantNaturalBajo { get; set; }

        [JsonPropertyName("cant_natural_medio")]
        public int CantNaturalMedio { get; set; }

        [JsonPropertyName("cant_natural_alto")]
        public int CantNaturalAlto { get; set; }

        [JsonPropertyName("cant_natural_sn")]
        public int CantNaturalSn { get; set; }

        [JsonPropertyName("jur_natural_bajo")]
        public int JurNaturalBajo { get; set; }

        [JsonPropertyName("jur_natural_medio")]
        public int JurNaturalMedio { get; set; }

        [JsonPropertyName("jur_natural_alto")]
        public int JurNaturalAlto { get; set; }

        [JsonPropertyName("jur_natural_sn")]
        public int JurNaturalSn { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class CanalesDistribucion
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("denominacion")]
        public string Denominacion { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }

        [JsonPropertyName("riesgo")]
        public string Riesgo { get; set; }
    }

    public class SubFactorProductosServicios
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("denominacionDelProductoOServicio")]
        public string DenominacionDelProductoOServicio { get; set; }

        [JsonPropertyName("fechaDeAutorizacionDeLaSUDEBAN")]
        public DateTime FechaDeAutorizacionDeLaSUDEBAN { get; set; }

        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }

        [JsonPropertyName("calificacionDeRiesgo")]
        public string CalificacionDeRiesgo { get; set; }
    }

    public class CalificacionRiesgoCliente
    {
        [JsonPropertyName("alto")]
        public int Alto { get; set; }

        [JsonPropertyName("medio")]
        public int Medio { get; set; }

        [JsonPropertyName("moderado")]
        public int Moderado { get; set; }
    }

    public class ClientesPorMes
    {
        [JsonPropertyName("mes")]
        public int Mes { get; set; }

        [JsonPropertyName("naturales")]
        public CalificacionRiesgoCliente Naturales { get; set; }

        [JsonPropertyName("juridicos")]
        public CalificacionRiesgoCliente Juridicos { get; set; }
    }

    public class CalificacionRiesgoAnual
    {
        [JsonPropertyName("anio")]
        public int Anio { get; set; }

        [JsonPropertyName("calificacionesPorMes")]
        public List<ClientesPorMes> CalificacionesPorMes { get; set; }
    }

    public class AlertaMensualCantidad
    {
        [JsonPropertyName("mes")]
        public int Mes { get; set; }

        [JsonPropertyName("cantidad")]
        public int Cantidad { get; set; }
    }
    public class AlertaDetalle
    {
        [JsonPropertyName("nombreAlerta")]
        public string NombreAlerta { get; set; }

        [JsonPropertyName("cantidadesPorMes")]
        public List<AlertaMensualCantidad> CantidadesPorMes { get; set; }
    }
    public class ListaAlertasResultado
    {
        [JsonPropertyName("anio")]
        public int Anio { get; set; }

        [JsonPropertyName("alertas")]
        public List<AlertaDetalle> Alertas { get; set; }
    }
}
