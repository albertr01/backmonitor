using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;
using WebApplication1.Connections.BD;
using WebApplication1.Models.DB;
using WebApplication1.Models.Request;
using WebApplication1.Models.Response.Local.Salida;
using WebApplication1.Utils;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GestionSuscripcionAlertasController : ControllerBase
    {
        private readonly ManagementLogs _managementLogs;
        private readonly SubcripcionAlertaMetodos _subcripcionAlertaMetodos;
        private readonly AlertaMetodos _alertaMetodos;
        private readonly ITokenProvider _tokenProvider;
        public GestionSuscripcionAlertasController(ILogger<GestionSuscripcionAlertasController> logger, MonitoreopyaContext modelContext, ITokenProvider tokenProvider)
        {
            _managementLogs = new ManagementLogs(logger);
            _subcripcionAlertaMetodos = new(modelContext, logger);
            _alertaMetodos = new(modelContext, logger);
            _tokenProvider = tokenProvider;
        }
        /// <summary>
        /// Servicio de registrar alerta
        /// </summary>
        [HttpPost("GuardarSubscripcionAlerta")]
        [SwaggerResponse(200, type: typeof(GenericoSalida))]
        public IActionResult GuardarSubscripcionAlerta(SubcripcionAlertaRequest subcripcionAlertaRequest)
        {
            try
            {
                var response = _subcripcionAlertaMetodos.RegistrarSubcripcionAlerta(subcripcionAlertaRequest.IdAlerta, subcripcionAlertaRequest.NombreAlerta, subcripcionAlertaRequest.Descripcion,
                                                                                    subcripcionAlertaRequest.MetodoDistribucion, subcripcionAlertaRequest.BandejaEntrada, subcripcionAlertaRequest.Comentario,
                                                                                    subcripcionAlertaRequest.Estado, _tokenProvider.GetUserName(),
                                                                                    subcripcionAlertaRequest.CorreoElectronico, 
                                                                                    subcripcionAlertaRequest.IdCliente,
                                                                                    subcripcionAlertaRequest.NotificacionCorreo);
                
                return Ok(new GenericoSalida
                {
                    Codigo = "0",
                    Descripcion = "Exito",
                    DescripcionTécnica = "Exito",
                    Error = false
                });
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                    { nameof(subcripcionAlertaRequest), JsonSerializer.Serialize(subcripcionAlertaRequest) }
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al guardar alerta",
                    data);

                return Ok(new GenericoSalida
                {
                    Codigo = "1",
                    Descripcion = "Error al guardar alerta",
                    DescripcionTécnica = e.Message,
                    Error = true
                });
            }
        }
        /// <summary>
        /// Consultar Suscripcion de Alertas
        /// </summary>
        [HttpGet("consultarSubcripcionAlertas")]
        [SwaggerResponse(200, type: typeof(SubscripcionAlertaSalida))]
        public IActionResult consultarSubcripcionAlertas()
        {
            try
            {
                var listaSuscripciones = _subcripcionAlertaMetodos.ConsultarSuscripcionAlerta();

                List<LogsAuditorium> listaLogs = new List<LogsAuditorium>();
                List<SuscripcionAlerta> listaSuscripcionesFinal = new List<SuscripcionAlerta>();

                foreach (var suscripcion in listaSuscripciones)
                {
                    listaLogs = _subcripcionAlertaMetodos.ConsultarHistorial(suscripcion.Id);

                    listaSuscripcionesFinal.Add(new SuscripcionAlerta
                    {
                        BandejaEntrada = suscripcion.BandejaEntrada,
                        Comentario = suscripcion.Comentario,
                        Descripcion = suscripcion.Descripcion,
                        Estado = suscripcion.Estado,
                        Id = suscripcion.Id,
                        IdAlerta = suscripcion.FkAlerta,
                        MetodoDistribucion = suscripcion.MetodoDistribucion,
                        NombreAlerta = suscripcion.FkAlertaNavigation == null ? suscripcion.Nombre : suscripcion.FkAlertaNavigation.Nombre,
                        Fecha = suscripcion.FechaCreacion,
                        Usuario = suscripcion.Usuario,
                        CorreoElectronico = suscripcion.Correo,
                        IdCliente = suscripcion.IdCliente,
                        NombreTipoCliente = suscripcion.IdCliente switch
                        {
                            1 => "Natural",
                            2 => "Jurídico",
                            3 => "Empleado",
                            4 => "Proveedor",
                            _ => "Desconocido"
                        },
                        NotificacionCorreo = suscripcion.NotificacionCorreo,
                        Historial = listaLogs.Select(l => new AlertaHistorial
                        {
                            EsEncendido = l.DetalleRegistro.Split(";")[1].Split(":")[1].Equals("True"),
                            FechaHora = l.FechaIngreso.Value,
                            Usuario = l.IdUsuario
                        }).ToList()
                    });
                }

                return Ok(new SubscripcionAlertaSalida
                {
                    Codigo = "0",
                    Descripcion = "Exito",
                    DescripcionTécnica = "Exito",
                    Error = false,
                    suscripcionAlertas = listaSuscripcionesFinal
                });
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al consultar alertas",
                    data);

                return Ok(new GenericoSalida
                {
                    Codigo = "1",
                    Descripcion = "Error al consultar alertas",
                    DescripcionTécnica = e.Message,
                    Error = true
                });
            }
        }
        /// <summary>
        /// Consultar Suscripcion de Alertas
        /// </summary>
        [HttpGet("consultarTiposAlertas")]
        [SwaggerResponse(200, type: typeof(TipoAlertasSalida))]
        public IActionResult consultarTiposAlertas()
        {
            try
            {
                var listaAlertas = _alertaMetodos.ConsultarAlertas();

                return Ok(new TipoAlertasSalida
                {
                    Codigo = "0",
                    Descripcion = "Exito",
                    DescripcionTécnica = "Exito",
                    Error = false,
                    tipoAlertas = listaAlertas.Select(s => new TipoAlerta
                    {
                        Id = s.Id,
                        Nombre = s.Nombre,
                        Codigo = s.Codigo,
                        Descripcion = s.Descripcion
                    }).ToList(),
                    TiposCliente = new List<SeleccionableSalida>
                    {
                        new SeleccionableSalida
                        {
                            Id = 1,
                            Nombre = "Natural",
                        },
                        new SeleccionableSalida
                        {
                            Id = 2,
                            Nombre = "Jurídico",
                        },
                        new SeleccionableSalida
                        {
                            Id = 3,
                            Nombre = "Empleado"
                        },
                        new SeleccionableSalida
                        {
                            Id = 4,
                            Nombre = "Proveedor"
                        }
                    }
                });
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al consultar alertas",
                    data);

                return Ok(new GenericoSalida
                {
                    Codigo = "1",
                    Descripcion = "Error al consultar alertas",
                    DescripcionTécnica = e.Message,
                    Error = true
                });
            }
        }
        /// <summary>
        /// Servicio de modificar tipo alerta
        /// </summary>
        [HttpPut("ModificarSubscripcionAlerta")]
        [SwaggerResponse(200, type: typeof(GenericoSalida))]
        public IActionResult ModificarSubscripcionAlerta(SubcripcionAlertaRequest subcripcionAlertaRequest)
        {
            try
            {
                _subcripcionAlertaMetodos.ModificarSubcripcionAlerta(subcripcionAlertaRequest.Id.Value, subcripcionAlertaRequest.IdAlerta, subcripcionAlertaRequest.NombreAlerta, subcripcionAlertaRequest.Descripcion,
                                                                    subcripcionAlertaRequest.MetodoDistribucion, subcripcionAlertaRequest.BandejaEntrada, subcripcionAlertaRequest.Comentario,
                                                                    subcripcionAlertaRequest.Estado, _tokenProvider.GetUserName(),
                                                                    subcripcionAlertaRequest.CorreoElectronico,
                                                                    subcripcionAlertaRequest.IdCliente,
                                                                    subcripcionAlertaRequest.NotificacionCorreo);

                return Ok(new GenericoSalida
                {
                    Codigo = "0",
                    Descripcion = $"Exito",
                    DescripcionTécnica = "Exito",
                    Error = false
                });
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                    { nameof(subcripcionAlertaRequest), JsonSerializer.Serialize(subcripcionAlertaRequest) }
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al modificar alerta",
                    data);

                return Ok(new GenericoSalida
                {
                    Codigo = "1",
                    Descripcion = "Error al modificar alerta",
                    DescripcionTécnica = e.Message,
                    Error = true
                });
            }
        }
        /// <summary>
        /// Servicio de camb tipo alerta
        /// </summary>
        [HttpPut("CambiarEstatusSubscripcionAlerta")]
        [SwaggerResponse(200, type: typeof(GenericoSalida))]
        public IActionResult CambiarEstatusSubscripcionAlerta(List<SubcripcionAlertaRequest> subcripcionAlertaRequest)
        {
            try
            {
                subcripcionAlertaRequest.ForEach(s =>
                {
                    _subcripcionAlertaMetodos.CambiarEstatusSubcripcionAlerta(s.Id.Value, s.Estado, _tokenProvider.GetUserName(), _tokenProvider.GetRol());
                });

                return Ok(new GenericoSalida
                {
                    Codigo = "0",
                    Descripcion = $"Exito",
                    DescripcionTécnica = "Exito",
                    Error = false
                });
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                    { nameof(subcripcionAlertaRequest), JsonSerializer.Serialize(subcripcionAlertaRequest) }
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al cambiar estatus de alertas",
                    data);

                return Ok(new GenericoSalida
                {
                    Codigo = "1",
                    Descripcion = "Error al cambiar estatus de alerta",
                    DescripcionTécnica = e.Message,
                    Error = true
                });
            }
        }
        /// <summary>
        /// Eliminar Tipo de Alerta
        /// </summary>
        [HttpDelete("EliminarSubscripcionAlerta")]
        [SwaggerResponse(200, type: typeof(GenericoSalida))]
        public IActionResult EliminarSubscripcionAlerta(int id)
        {
            try
            {
                _subcripcionAlertaMetodos.EliminarSubcripcionAlerta(id);

                return Ok(new AlertaMontosSalida
                {
                    Codigo = "0",
                    Descripcion = "Exito",
                    DescripcionTécnica = "Exito",
                    Error = false
                });
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al eliminar alerta",
                    data);

                return Ok(new GenericoSalida
                {
                    Codigo = "1",
                    Descripcion = "Error al eliminar alerta",
                    DescripcionTécnica = e.Message,
                    Error = true
                });
            }
        }
    }
}
