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
    public class GestionAlertaMontoController : ControllerBase
    {
        private readonly ManagementLogs _managementLogs;
        private readonly AlertaMontoMetodos _alertaMontoMetodos;
        public GestionAlertaMontoController(ILogger<GestionAlertaMontoController> logger, MonitoreopyaContext modelContext)
        {
            _managementLogs = new ManagementLogs(logger);
            _alertaMontoMetodos = new(modelContext, logger);
        }
        /// <summary>
        /// Servicio de registrar alerta de monto
        /// </summary>
        [HttpPost("GuardarAlertaMonto")]
        [SwaggerResponse(200, type: typeof(GenericoSalida))]
        public IActionResult GuardarAlertaMonto(AlertaMontoRequest alertaMontoRequest)
        {
            try
            {
                var response = _alertaMontoMetodos.RegistrarAlertaMonto(DateTime.Now, alertaMontoRequest.IdTipoOperacion, alertaMontoRequest.CodigoTipoOperacion,
                                                                        alertaMontoRequest.IdTipoMoneda, alertaMontoRequest.Umbral, alertaMontoRequest.Comentario, alertaMontoRequest.IdAgencia,
                                                                        alertaMontoRequest.IdCLiente);
                
                return Ok(new GenericoSalida
                {
                    Codigo = "0",
                    Descripcion =$"Monto para el monitoreo registrado exitosamente",
                    DescripcionTécnica = "Exito",
                    Error = false
                });
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                    { nameof(alertaMontoRequest), JsonSerializer.Serialize(alertaMontoRequest) }
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al guardar alerta de monto",
                    data);

                return Ok(new GenericoSalida
                {
                    Codigo = "1",
                    Descripcion = "Error al guardar alerta de monto",
                    DescripcionTécnica = e.Message,
                    Error = true
                });
            }
        }
        /// <summary>
        /// Consultar Alertas de Monto
        /// </summary>
        [HttpGet("consultarAlertasMonto")]
        [SwaggerResponse(200, type: typeof(AlertaMontosSalida))]
        public IActionResult consultarAlertasMonto()
        {
            try
            {
                var listaOperacion = _alertaMontoMetodos.ConsultarListaOperacion();
                var listaMonedas = _alertaMontoMetodos.ConsultarListaMonedas();
                var alertasMontos = _alertaMontoMetodos.ConsultarAlertasMonto();
                var agencias = _alertaMontoMetodos.ConsultarAgencias();

                return Ok(new AlertaMontosSalida
                {
                    Codigo = "0",
                    Descripcion = "Exito",
                    DescripcionTécnica = "Exito",
                    Error = false,
                    TipoMonedas = listaMonedas.Select(x => new TipoMoneda
                    {
                        Id = x.Id,
                        Codigo = x.Codigo,
                        Nombre = x.Nombre
                    }).ToList(),
                    TipoOperacions = listaOperacion.Select(x => new Models.Response.Local.Salida.TipoOperacion
                    {
                        Id = x.Id,
                        Codigo = x.Codigo,
                        Nombre = x.Nombre
                    }).ToList(),
                    AlertaMontos = alertasMontos.Select(a => new Models.Response.Local.Salida.AlertaMonto
                    {
                        Id = a.Id,
                        IdTipoOperacion = a.IdTipoOperacion,
                        NombreTipoOperacion = listaOperacion.FirstOrDefault(x => x.Id == a.IdTipoOperacion)?.Nombre,
                        Codigo = a.Codigo,
                        IdTipoMoneda = a.IdTipoMoneda,
                        NombreTipoMoneda = listaMonedas.FirstOrDefault(x => x.Id == a.IdTipoMoneda)?.Nombre,
                        Umbral = a.Umbral,
                        Comentario = a.Comentario,
                        FechaConfiguracion = a.FechaConfiguracion,
                        IdAgencia = a.IdAgencia,
                        IdCliente = a.IdCliente
                    }).ToList(),
                    Agencias = agencias.Select(a => new Agencia
                    {
                        Id = a.Id,
                        Nombre = a.Nombre
                    }).ToList(),
                    TiposClientes = new List<SeleccionableSalida>
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
                    "Error al consultar alerta montos",
                    data);

                return Ok(new GenericoSalida
                {
                    Codigo = "1",
                    Descripcion = "Error al consultar alerta montos",
                    DescripcionTécnica = e.Message,
                    Error = true
                });
            }
        }
        /// <summary>
        /// Servicio de modificar alerta de monto
        /// </summary>
        [HttpPut("ModificarAlertaMonto")]
        [SwaggerResponse(200, type: typeof(GenericoSalida))]
        public IActionResult ModificarAlertaMonto(AlertaMontoRequest alertaMontoRequest)
        {
            try
            {
                _alertaMontoMetodos.ModificarAlertaMonto(alertaMontoRequest.Id, DateTime.Now, alertaMontoRequest.IdTipoOperacion,
                                                                        alertaMontoRequest.IdTipoMoneda, alertaMontoRequest.Umbral, alertaMontoRequest.Comentario, alertaMontoRequest.IdCLiente,
                                                                        alertaMontoRequest.IdAgencia);

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
                    { nameof(alertaMontoRequest), JsonSerializer.Serialize(alertaMontoRequest) }
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al modificar alerta de monto",
                    data);

                return Ok(new GenericoSalida
                {
                    Codigo = "1",
                    Descripcion = "Error al modificar alerta de monto",
                    DescripcionTécnica = e.Message,
                    Error = true
                });
            }
        }
        /// <summary>
        /// Eliminar Alertas de Monto
        /// </summary>
        [HttpDelete("eliminarAlertasMonto")]
        [SwaggerResponse(200, type: typeof(GenericoSalida))]
        public IActionResult eliminarAlertasMonto(int id)
        {
            try
            {
                _alertaMontoMetodos.EliminarAlertaMonto(id);

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
                    "Error al eliminar alerta montos",
                    data);

                return Ok(new GenericoSalida
                {
                    Codigo = "1",
                    Descripcion = "Error al eliminar alerta montos",
                    DescripcionTécnica = e.Message,
                    Error = true
                });
            }
        }
        /// <summary>
        /// Consultar Alerta de monto
        /// </summary>
        [HttpGet("{id}")]
        [SwaggerResponse(200, type: typeof(Models.Response.Local.Salida.AlertaMonto))]
        public IActionResult consultarAlertaMonto(int id)
        {
            try
            {
                var alertaMonto = _alertaMontoMetodos.ConsultarAlertasMonto(id);

                if(alertaMonto == null)
                {
                    return Ok(new GenericoSalida
                    {
                        Codigo = "1",
                        Descripcion = "Alerta de monto no encontrada",
                        DescripcionTécnica = "Alerta de monto no encontrada",
                        Error = true
                    });
                }

                return Ok(new Models.Response.Local.Salida.AlertaMonto
                {
                    Codigo = "0",
                    Descripcion = "Exito",
                    DescripcionTécnica = "Exito",
                    Error = false,
                    Comentario = alertaMonto.Comentario,
                    FechaConfiguracion = alertaMonto.FechaConfiguracion,
                    Id = alertaMonto.Id,
                    IdTipoMoneda = alertaMonto.IdTipoMoneda,
                    IdTipoOperacion = alertaMonto.IdTipoOperacion,
                    NombreTipoMoneda = alertaMonto.IdTipoMonedaNavigation.Nombre,
                    NombreTipoOperacion = alertaMonto.IdTipoOperacionNavigation.Nombre,
                    Umbral = alertaMonto.Umbral,
                    IdAgencia = alertaMonto.IdAgencia,
                    IdCliente = alertaMonto.IdCliente
                });
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al consultar alerta de monto",
                    data);

                return Ok(new GenericoSalida
                {
                    Codigo = "1",
                    Descripcion = "Error al consultar alerta de monto",
                    DescripcionTécnica = e.Message,
                    Error = true
                });
            }
        }
    }
}
