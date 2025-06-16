using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.NetworkInformation;
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
    public class GestionRiesgoController : ControllerBase
    {
        private readonly ManagementLogs _managementLogs;
        private readonly PeriodoMetodos _periodoMetodos;
        private readonly RiesgoMetodos _riesgoMetodos;
        private readonly MstParametrosRiesgoMetodos _mstParametrosRiesgo;
        public GestionRiesgoController(ILogger<GestionRiesgoController> logger, MonitoreopyaContext modelContext)
        {
            _managementLogs = new ManagementLogs(logger);
            _periodoMetodos = new(modelContext, logger);
            _riesgoMetodos = new(modelContext, logger);
            _mstParametrosRiesgo = new(modelContext, logger);
        }
        /// <summary>
        /// Servicio de consulta de periodos
        /// </summary>
        [HttpGet("periodos")]
        [SwaggerResponse(200, type: typeof(PeriodosSalida))]
        public IActionResult Busqueda(string? estatus = "ACTIVO")
        {
            try
            {
                var respuesta = _periodoMetodos.ConsultaPeriodos(estatus);

                return Ok(new PeriodosSalida
                {
                    Codigo = "0",
                    Descripcion = "Exito",
                    DescripcionTécnica = "Exito",
                    Error = false,
                    periodos = respuesta.Select(p => new Models.Response.Local.Salida.Periodo
                    {
                        Estatus = p.Estatus,
                        FechaCierre = p.FechaCierre,
                        FechaCreacion = p.FechaCreacion,
                        Id = p.Id,
                        PeriodoAno = p.Periodo1,
                        TipoPeriodo = p.TipoPeriodo,
                        FechaFin = p.FechaFin,
                        FechaInicio = p.FechaInicio,
                        NombreEvaluacion = p.Nombre
                    }).ToList()
                });
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                    { nameof(estatus), estatus }
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al consulta periodos",
                    data);

                return Ok(new GenericoSalida
                {
                    Codigo = "1",
                    Descripcion = "Error al consulta periodos",
                    DescripcionTécnica = e.Message,
                    Error = true
                });
            }

        }
        /// <summary>
        /// Servicio de guardar periodo
        /// </summary>
        [HttpPost("guardarPeriodo")]
        [SwaggerResponse(200, type: typeof(GenericoSalida))]
        public IActionResult GuardarPeriodo(PeriodoRequest periodoRequest)
        {
            try
            {
                var respuesta = _periodoMetodos.RegistrarPeriodo(periodoRequest.Periodo, periodoRequest.TipoPeriodo, periodoRequest.FechaInicio, periodoRequest.FechaFin, periodoRequest.NombreEvaluacion);

                if (respuesta is not null)
                {
                    var riesgo = _riesgoMetodos.RegistrarRiesgoInicial(respuesta.Id);

                    var selecionableFactorRiesgo = _mstParametrosRiesgo.ConsultarTablaMstParametrosRiego(10);
                    var selecionableProbabilidad = _mstParametrosRiesgo.ConsultarTablaMstParametrosRiego(11);
                    var selecionableImpacto = _mstParametrosRiesgo.ConsultarTablaMstParametrosRiego(12);
                    var selecionableTipoRiesgo = _mstParametrosRiesgo.ConsultarTablaMstParametrosRiego(15);
                    var selecionableAutomatizacion = _mstParametrosRiesgo.ConsultarTablaMstParametrosRiego(16);
                    var selecionableFrecuencia = _mstParametrosRiesgo.ConsultarTablaMstParametrosRiego(17);
                    var selecionableTipoRiesgoR = _mstParametrosRiesgo.ConsultarTablaMstParametrosRiego(21);

                    return Ok(new RiesgoGuardarSalida
                    {
                        Codigo = "0",
                        Descripcion = "Exito",
                        DescripcionTécnica = "Exito",
                        Error = false,
                        IdRiesgo = riesgo.Id,
                        IdPeriodo = respuesta.Id,
                        selecionableAutomatizacion = selecionableAutomatizacion.Select(a => new SeleccionableSalida
                        {
                            Color = a.Color,
                            Id = a.Id,
                            Nombre = a.TipoParametroRiesgo,
                            ValorFn = a.ValorFn,
                            ValorIn = a.ValorIn
                        }).ToList(),
                        selecionableFrecuencia = selecionableFrecuencia.Select(a => new SeleccionableSalida
                        {
                            Color = a.Color,
                            Id = a.Id,
                            Nombre = a.TipoParametroRiesgo,
                            ValorFn = a.ValorFn,
                            ValorIn = a.ValorIn
                        }).ToList(),
                        selecionableTipoRiesgo = selecionableTipoRiesgo.Select(a => new SeleccionableSalida
                        {
                            Color = a.Color,
                            Id = a.Id,
                            Nombre = a.TipoParametroRiesgo,
                            ValorFn = a.ValorFn,
                            ValorIn = a.ValorIn
                        }).ToList(),
                        selecionableImpacto = selecionableImpacto.Select(a => new SeleccionableSalida
                        {
                            Color = a.Color,
                            Id = a.Id,
                            Nombre = a.TipoParametroRiesgo,
                            ValorFn = a.ValorFn,
                            ValorIn = a.ValorIn
                        }).ToList(),
                        selecionableProbabilidad = selecionableProbabilidad.Select(a => new SeleccionableSalida
                        {
                            Color = a.Color,
                            Id = a.Id,
                            Nombre = a.TipoParametroRiesgo,
                            ValorFn = a.ValorFn,
                            ValorIn = a.ValorIn
                        }).ToList(),
                        selecionableFactorRiesgo = selecionableFactorRiesgo.Where(f => f.IdMtsParametrosRiegos == null).Select(a => new SeleccionableSalida
                        {
                            Color = a.Color,
                            Id = a.Id,
                            Nombre = a.TipoParametroRiesgo,
                            ValorFn = a.ValorFn,
                            ValorIn = a.ValorIn
                        }).ToList(),
                        selecionableSubFactorRiesgo = selecionableFactorRiesgo.Where(f => f.IdMtsParametrosRiegos != null).Select(a => new SeleccionableSalida
                        {
                            Color = a.Color,
                            Id = a.Id,
                            Nombre = a.TipoParametroRiesgo,
                            ValorFn = a.ValorFn,
                            ValorIn = a.ValorIn,
                            IdPadre = a.IdMtsParametrosRiegos,
                        }).ToList(),
                        selecionableTipoRiesgoR = selecionableTipoRiesgoR.Select(a => new SeleccionableSalida
                        {
                            Color = a.Color,
                            Id = a.Id,
                            Nombre = a.TipoParametroRiesgo,
                            ValorFn = a.ValorFn,
                            ValorIn = a.ValorIn
                        }).ToList()
                    });
                }

                return Ok(new RiesgoGuardarSalida
                {
                    Codigo = "1",
                    Descripcion = "Periodo existente",
                    DescripcionTécnica = "Periodo existente",
                    Error = true
                });
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                    { nameof(periodoRequest), JsonSerializer.Serialize(periodoRequest) }
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al guardar periodo",
                    data);

                return Ok(new GenericoSalida
                {
                    Codigo = "1",
                    Descripcion = "Error al guardar periodo",
                    DescripcionTécnica = e.Message,
                    Error = true
                });
            }
        }
        /// <summary>
        /// Servicio de cerrar periodo
        /// </summary>
        [HttpPost("cerrarPeriodo")]
        [SwaggerResponse(200, type: typeof(GenericoSalida))]
        public IActionResult CerrarPeriodo(int id)
        {
            try
            {
                _periodoMetodos.ActualizarEstadoPeriodo(id, "CERRADO");

                return Ok(new GenericoSalida
                {
                    Codigo = "0",
                    Descripcion = "Exito",
                    DescripcionTécnica = "Exito",
                    Error = false,
                });
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                    { nameof(id), JsonSerializer.Serialize(id) }
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al cerrar periodo",
                    data);

                return Ok(new GenericoSalida
                {
                    Codigo = "1",
                    Descripcion = "Error al cerrar periodo",
                    DescripcionTécnica = e.Message,
                    Error = true
                });
            }
        }
        /// <summary>
        /// Servicio de guardar riesgo
        /// </summary>
        [HttpPost("guardarRiesgo")]
        [SwaggerResponse(200, type: typeof(GenericoSalida))]
        public IActionResult GuardarRiesgo(RiesgoRequest riesgoRequest)
        {
            try
            {
                _riesgoMetodos.ActualizarRiesgo(riesgoRequest.Id, riesgoRequest.Amenaza, riesgoRequest.Vulnerabilidad, 
                                                riesgoRequest.Consecuencia, riesgoRequest.IdFactorRiesgo, riesgoRequest.IdSubFactorRiesgo,
                                                riesgoRequest.IdTipoRiesgo, riesgoRequest.Causa, riesgoRequest.IdProbabilidad, riesgoRequest.ProbabilidadValor, riesgoRequest.Impacto, riesgoRequest.ImpactoValor, 
                                                riesgoRequest.RiesgoInherente, riesgoRequest.Severidad, riesgoRequest.Descripcion, riesgoRequest.IdTipo, riesgoRequest.IdAutomatizacion, riesgoRequest.IdFrecuencia, 
                                                riesgoRequest.ValoresControl, riesgoRequest.RiesgoResidualValor, riesgoRequest.SeveridadValor, riesgoRequest.Accion, riesgoRequest.Responsable,
                                                riesgoRequest.IdTiempoEjecucion, riesgoRequest.IdTratamientoRiesgo);


                return Ok(new GenericoSalida
                {
                    Codigo = "0",
                    Descripcion = "Exito",
                    DescripcionTécnica = "Exito",
                    Error = false,
                });
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                    { nameof(riesgoRequest), JsonSerializer.Serialize(riesgoRequest) }
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al guardar riesgo",
                    data);

                return Ok(new GenericoSalida
                {
                    Codigo = "1",
                    Descripcion = "Error al guardar riesgo",
                    DescripcionTécnica = e.Message,
                    Error = true
                });
            }
        }
        /// <summary>
        /// Incluir otro riesgo en periodo
        /// </summary>
        [HttpPost("incluirOtroRiesgo")]
        [SwaggerResponse(200, type: typeof(GenericoSalida))]
        public IActionResult incluirOtroRiesgo(int id)
        {
            try
            {
                var riesgo = _riesgoMetodos.RegistrarRiesgoInicial(id);

                var selecionableFactorRiesgo = _mstParametrosRiesgo.ConsultarTablaMstParametrosRiego(10);
                var selecionableProbabilidad = _mstParametrosRiesgo.ConsultarTablaMstParametrosRiego(11);
                var selecionableImpacto = _mstParametrosRiesgo.ConsultarTablaMstParametrosRiego(12);
                var selecionableTipoRiesgo = _mstParametrosRiesgo.ConsultarTablaMstParametrosRiego(15);
                var selecionableAutomatizacion = _mstParametrosRiesgo.ConsultarTablaMstParametrosRiego(16);
                var selecionableFrecuencia = _mstParametrosRiesgo.ConsultarTablaMstParametrosRiego(17);
                var selecionableTipoRiesgoR = _mstParametrosRiesgo.ConsultarTablaMstParametrosRiego(21);

                return Ok(new RiesgoGuardarSalida
                {
                    Codigo = "0",
                    Descripcion = "Exito",
                    DescripcionTécnica = "Exito",
                    Error = false,
                    IdRiesgo = riesgo.Id,
                    selecionableAutomatizacion = selecionableAutomatizacion.Select(a => new SeleccionableSalida
                    {
                        Color = a.Color,
                        Id = a.Id,
                        Nombre = a.TipoParametroRiesgo,
                        ValorFn = a.ValorFn,
                        ValorIn = a.ValorIn
                    }).ToList(),
                    selecionableFrecuencia = selecionableFrecuencia.Select(a => new SeleccionableSalida
                    {
                        Color = a.Color,
                        Id = a.Id,
                        Nombre = a.TipoParametroRiesgo,
                        ValorFn = a.ValorFn,
                        ValorIn = a.ValorIn
                    }).ToList(),
                    selecionableTipoRiesgo = selecionableTipoRiesgo.Select(a => new SeleccionableSalida
                    {
                        Color = a.Color,
                        Id = a.Id,
                        Nombre = a.TipoParametroRiesgo,
                        ValorFn = a.ValorFn,
                        ValorIn = a.ValorIn
                    }).ToList(),
                    selecionableImpacto = selecionableImpacto.Select(a => new SeleccionableSalida
                    {
                        Color = a.Color,
                        Id = a.Id,
                        Nombre = a.TipoParametroRiesgo,
                        ValorFn = a.ValorFn,
                        ValorIn = a.ValorIn
                    }).ToList(),
                    selecionableProbabilidad = selecionableProbabilidad.Select(a => new SeleccionableSalida
                    {
                        Color = a.Color,
                        Id = a.Id,
                        Nombre = a.TipoParametroRiesgo,
                        ValorFn = a.ValorFn,
                        ValorIn = a.ValorIn
                    }).ToList(),
                    selecionableFactorRiesgo = selecionableFactorRiesgo.Where(f => f.IdMtsParametrosRiegos == null).Select(a => new SeleccionableSalida
                    {
                        Color = a.Color,
                        Id = a.Id,
                        Nombre = a.TipoParametroRiesgo,
                        ValorFn = a.ValorFn,
                        ValorIn = a.ValorIn
                    }).ToList(),
                    selecionableSubFactorRiesgo = selecionableFactorRiesgo.Where(f => f.IdMtsParametrosRiegos != null).Select(a => new SeleccionableSalida
                    {
                        Color = a.Color,
                        Id = a.Id,
                        Nombre = a.TipoParametroRiesgo,
                        ValorFn = a.ValorFn,
                        ValorIn = a.ValorIn,
                        IdPadre = a.IdMtsParametrosRiegos,
                    }).ToList(),
                    selecionableTipoRiesgoR = selecionableTipoRiesgoR.Select(a => new SeleccionableSalida
                    {
                        Color = a.Color,
                        Id = a.Id,
                        Nombre = a.TipoParametroRiesgo,
                        ValorFn = a.ValorFn,
                        ValorIn = a.ValorIn
                    }).ToList()
                });
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                    { nameof(id), JsonSerializer.Serialize(id) }
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al incluir riesgo",
                    data);

                return Ok(new GenericoSalida
                {
                    Codigo = "1",
                    Descripcion = "Error al incluir riesgo",
                    DescripcionTécnica = e.Message,
                    Error = true
                });
            }
        }
        /// <summary>
        /// Obtener Seleccionable de Riesgo
        /// </summary>
        [HttpGet("seleccionablesRiesgo")]
        [SwaggerResponse(200, type: typeof(GenericoSalida))]
        public IActionResult SeleccionablesRiesgo()
        {
            try
            {
                var selecionableFactorRiesgo = _mstParametrosRiesgo.ConsultarTablaMstParametrosRiego(10);
                var selecionableProbabilidad = _mstParametrosRiesgo.ConsultarTablaMstParametrosRiego(11);
                var selecionableImpacto = _mstParametrosRiesgo.ConsultarTablaMstParametrosRiego(12);
                var selecionableTipoRiesgo = _mstParametrosRiesgo.ConsultarTablaMstParametrosRiego(15);
                var selecionableAutomatizacion = _mstParametrosRiesgo.ConsultarTablaMstParametrosRiego(16);
                var selecionableFrecuencia = _mstParametrosRiesgo.ConsultarTablaMstParametrosRiego(17);
                var selecionableTipoRiesgoR = _mstParametrosRiesgo.ConsultarTablaMstParametrosRiego(21);

                return Ok(new RiesgoGuardarSalida
                {
                    Codigo = "0",
                    Descripcion = "Exito",
                    DescripcionTécnica = "Exito",
                    Error = false,
                    selecionableAutomatizacion = selecionableAutomatizacion.Select(a => new SeleccionableSalida
                    {
                        Color = a.Color,
                        Id = a.Id,
                        Nombre = a.TipoParametroRiesgo,
                        ValorFn = a.ValorFn,
                        ValorIn = a.ValorIn
                    }).ToList(),
                    selecionableFrecuencia = selecionableFrecuencia.Select(a => new SeleccionableSalida
                    {
                        Color = a.Color,
                        Id = a.Id,
                        Nombre = a.TipoParametroRiesgo,
                        ValorFn = a.ValorFn,
                        ValorIn = a.ValorIn
                    }).ToList(),
                    selecionableTipoRiesgo = selecionableTipoRiesgo.Select(a => new SeleccionableSalida
                    {
                        Color = a.Color,
                        Id = a.Id,
                        Nombre = a.TipoParametroRiesgo,
                        ValorFn = a.ValorFn,
                        ValorIn = a.ValorIn
                    }).ToList(),
                    selecionableImpacto = selecionableImpacto.Select(a => new SeleccionableSalida
                    {
                        Color = a.Color,
                        Id = a.Id,
                        Nombre = a.TipoParametroRiesgo,
                        ValorFn = a.ValorFn,
                        ValorIn = a.ValorIn
                    }).ToList(),
                    selecionableProbabilidad = selecionableProbabilidad.Select(a => new SeleccionableSalida
                    {
                        Color = a.Color,
                        Id = a.Id,
                        Nombre = a.TipoParametroRiesgo,
                        ValorFn = a.ValorFn,
                        ValorIn = a.ValorIn
                    }).ToList(),
                    selecionableFactorRiesgo = selecionableFactorRiesgo.Where(f => f.IdMtsParametrosRiegos == null).Select(a => new SeleccionableSalida
                    {
                        Color = a.Color,
                        Id = a.Id,
                        Nombre = a.TipoParametroRiesgo,
                        ValorFn = a.ValorFn,
                        ValorIn = a.ValorIn
                    }).ToList(),
                    selecionableSubFactorRiesgo = selecionableFactorRiesgo.Where(f => f.IdMtsParametrosRiegos != null).Select(a => new SeleccionableSalida
                    {
                        Color = a.Color,
                        Id = a.Id,
                        Nombre = a.TipoParametroRiesgo,
                        ValorFn = a.ValorFn,
                        ValorIn = a.ValorIn,
                        IdPadre = a.IdMtsParametrosRiegos,
                    }).ToList(),
                    selecionableTipoRiesgoR = selecionableTipoRiesgoR.Select(a => new SeleccionableSalida
                    {
                        Color = a.Color,
                        Id = a.Id,
                        Nombre = a.TipoParametroRiesgo,
                        ValorFn = a.ValorFn,
                        ValorIn = a.ValorIn
                    }).ToList()
                });
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {

                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al obtener seleccionables de riesgo",
                    data);

                return Ok(new GenericoSalida
                {
                    Codigo = "1",
                    Descripcion = "Error al obtener seleccionables de riesgo",
                    DescripcionTécnica = e.Message,
                    Error = true
                });
            }
        }
        /// <summary>
        /// Consultar lista de riesgos
        /// </summary>
        [HttpGet("consultarRiesgos")]
        [SwaggerResponse(200, type: typeof(GenericoSalida))]
        public IActionResult consultarRiesgos(int idPeriodo)
        {
            try
            {
                var riesgos = _riesgoMetodos.ConsultarRiesgosPorPeriodo(idPeriodo);

                return Ok(new ListaRiesgosSalida
                {
                    Codigo = "0",
                    Descripcion = "Exito",
                    DescripcionTécnica = "Exito",
                    Error = false,
                    Riesgos = riesgos.Select(r => new Models.Response.Local.Salida.Riesgo
                    {
                        Accion = r.Accion,
                        Amenaza = r.Amenaza,
                        Causa = r.Causa,
                        Consecuencia = r.Consecuencia,
                        Descripcion = r.Descripcion,
                        FechaCreacion = r.FechaCreacion,
                        Id = r.Id,
                        IdAutomatizacion = r.FkAutomatizacion,
                        IdFactorRiesgo = r.FkFactorRiesgo,
                        IdSubFactorRiesgo = r.FkSubFactorRiesgo,
                        IdFrecuencia = r.FkFrecuencia,
                        IdImpacto = r.FkImpacto,
                        IdProbabilidad = r.FkProbabilidad,
                        IdTiempoEjecucion = r.FkTiempoEjecucion,
                        IdTipo = r.FkTipoRiesgo,
                        IdTratamientoRiesgo = r.FkTratamientoRiesgo,
                        ImpactoValor = r.ImpactoValor,
                        ProbabilidadValor = r.ProbabilidadValor,
                        Responsable = r.Responsable,
                        RiesgoInherente = r.RiesgoInherente,
                        RiesgoResidualValor = r.RiesgoResidualValor,
                        Severidad = r.Severidad,
                        SeveridadValor = r.SeveridadValor,
                        ValoresControl = r.ValoresControl,
                        Vulnerabilidad = r.Vulnerabilidad,
                        IdTipoRiesgo = r.FktipoRiesgoR
                    }).ToList()
                });
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                    //{ nameof(id), JsonSerializer.Serialize(id) }
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al consultar riesgos",
                    data);

                return Ok(new GenericoSalida
                {
                    Codigo = "1",
                    Descripcion = "Error al consultar riesgos",
                    DescripcionTécnica = e.Message,
                    Error = true
                });
            }
        }
        /// <summary>
        /// Servicio de eliminar riesgo
        /// </summary>
        [HttpPost("eliminarRiesgo")]
        [SwaggerResponse(200, type: typeof(GenericoSalida))]
        public IActionResult EliminarRiesgo(int idRiesgo)
        {
            try
            {
                _riesgoMetodos.EliminarRiesgo(idRiesgo);


                return Ok(new GenericoSalida
                {
                    Codigo = "0",
                    Descripcion = "Exito",
                    DescripcionTécnica = "Exito",
                    Error = false,
                });
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                    { nameof(idRiesgo), idRiesgo }
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al eliminar riesgo",
                    data);

                return Ok(new GenericoSalida
                {
                    Codigo = "1",
                    Descripcion = "Error al eliminar riesgo",
                    DescripcionTécnica = e.Message,
                    Error = true
                });
            }
        }
        /// <summary>
        /// Consultar de mapa de calor
        /// </summary>
        [HttpGet("consultarMapaCalor")]
        [SwaggerResponse(200, type: typeof(GenericoSalida))]
        public IActionResult consultarMapaCalor(int idPeriodo, string? estatus = "ALL")
        {
            try
            {
                var riesgos = _riesgoMetodos.ConsultarRiesgosPorPeriodo(idPeriodo, estatus);

                riesgos = riesgos.Where(r => r.FkImpacto != null && r.FkProbabilidad != null).ToList();

                return Ok(new ListaRiesgosMapaCalorSalida
                {
                    Codigo = "0",
                    Descripcion = "Exito",
                    DescripcionTécnica = "Exito",
                    Error = false,
                    Riesgos = riesgos.Select(r => new Models.Response.Local.Salida.RiesgoMapaCalor
                    {
                        Id = r.Id,
                        IdImpacto = r.FkImpacto,
                        IdProbabilidad = r.FkProbabilidad,
                        ImpactoValor = r.ImpactoValor,
                        ProbabilidadValor = r.ProbabilidadValor,
                    }).ToList()
                });
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                    { nameof(idPeriodo), idPeriodo },
                    { nameof(estatus), estatus }
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al consultar mapa de calor de riesgos",
                    data);

                return Ok(new GenericoSalida
                {
                    Codigo = "1",
                    Descripcion = "Error al consultar mapa de calor de riesgos",
                    DescripcionTécnica = e.Message,
                    Error = true
                });
            }
        }
        /// <summary>
        /// Reporte resumen de riesgos por factor
        /// </summary>
        [HttpGet("reporteResumenRiesgos")]
        [SwaggerResponse(200, type: typeof(ReporteResumenRiesgosSalida))]
        public IActionResult reporteResumenRiesgos(int idPeriodo)
        {
            try
            {
                var riesgos = _riesgoMetodos.ConsultarRiesgosPorPeriodo(idPeriodo);

                var selecionableFactorRiesgo = _mstParametrosRiesgo.ConsultarTablaMstParametrosRiego(10);

                return Ok(new ReporteResumenRiesgosSalida
                {
                    Codigo = "0",
                    Descripcion = "Exito",
                    DescripcionTécnica = "Exito",
                    Error = false,
                    Total = riesgos.Where(r => r.FkFactorRiesgo is not null).Count(),
                    FactorRiesgos = selecionableFactorRiesgo
                        .Where(f => f.IdMtsParametrosRiegos == null) // Solo factores principales
                        .Select(f => new FactorRiesgo
                        {
                            Id = f.Id,
                            nombre = f.TipoParametroRiesgo,
                            Cantidad = riesgos.Count(r => r.FkFactorRiesgo == f.Id),
                            SubFactores = selecionableFactorRiesgo
                                .Where(sf => sf.IdMtsParametrosRiegos == f.Id)
                                .Select(sf => new SubFactorRiesgo
                                {
                                    Id = sf.Id,
                                    nombre = sf.TipoParametroRiesgo,
                                    Cantidad = riesgos.Count(r => r.FkSubFactorRiesgo == sf.Id)
                                }).ToList()
                        }).ToList()
                });
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                    //{ nameof(id), JsonSerializer.Serialize(id) }
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al consultar riesgos",
                    data);

                return Ok(new GenericoSalida
                {
                    Codigo = "1",
                    Descripcion = "Error al consultar riesgos",
                    DescripcionTécnica = e.Message,
                    Error = true
                });

            }
        }

        [HttpGet("reporteGlobalRiesgo")]
        public IActionResult reporteRiesgoGlobal([FromQuery] int periodoId)
        {
            var respuesta = new 
            {
                RiesgoPorZonas = new List<GenericListDTO> { 
                    new GenericListDTO { Nombre= "Zona 1", Cantidad = 4},
                    new GenericListDTO { Nombre= "Zona 2", Cantidad = 4},
                    new GenericListDTO { Nombre= "Zona 3", Cantidad = 5},
                    new GenericListDTO { Nombre= "Zona 4", Cantidad = 6},
                },
                RiesgoPorCanal = new List<GenericListDTO> {
                    new GenericListDTO { Nombre= "Canal 1", Cantidad = 8},
                    new GenericListDTO { Nombre= "Canal 2", Cantidad = 13},
                    new GenericListDTO { Nombre= "Canal 3", Cantidad = 17},
                },
                RiesgoPorProductoServicio = new List<GenericListDTO> {
                    new GenericListDTO { Nombre= "Canal 1", Cantidad = 8},
                    new GenericListDTO { Nombre= "Canal 2", Cantidad = 13},
                    new GenericListDTO { Nombre= "Canal 3", Cantidad = 17},
                },

                RiesgoPorCliente = new ListaGenerica
                {
                    Nombre = "Reporte de Riesgo por clientes", 
                    items = new List<ReporteDTO>
                    {
                        new ReporteDTO { Nombre = "PEP", Cantidad = 2, Porcentaje = 20 },
                        new ReporteDTO { Nombre = "Profesion u Oficio", Cantidad = 2, Porcentaje = 20 },
                        new ReporteDTO { Nombre = "Situacion Laboral", Cantidad = 2, Porcentaje = 20 },
                        new ReporteDTO { Nombre = "Positivo en lista de sanciones", Cantidad = 2, Porcentaje = 20 }
                    }
                }
               
            };

            return Ok(respuesta);
        }
    }

    public class GenericListDTO { 
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
    }
}
