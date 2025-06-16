using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.DB;
using WebApplication1.Utils;

namespace WebApplication1.Connections.BD
{
    public class RiesgoMetodos
    {
        private readonly MonitoreopyaContext _context;
        private readonly ManagementLogs _managementLogs;

        public RiesgoMetodos(MonitoreopyaContext context, ILogger<ControllerBase> logger)
        {
            _context = context;
            _managementLogs = new ManagementLogs(logger);
        }
        /// <summary>
        /// Registrar Periodo
        /// </summary>
        /// <returns><see cref="Riesgo"/></returns>
        public Riesgo RegistrarRiesgoInicial(int periodo)
        {
            try
            {
                var riesgo = _context.Riesgos.Add(new Riesgo
                {
                    FkPeriodo = periodo,
                    FechaCreacion = DateTime.Now
                });

                _context.SaveChanges();

                return riesgo.Entity;
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                    { nameof(periodo), periodo }
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al conectar a BD",
                    data);

                throw e.InnerException ?? e;
            }
        }
        /// <summary>
        /// Actualizar riesgo
        /// </summary>
        /// <returns><see cref="Riesgo"/></returns>
        public void ActualizarRiesgo(int identificador, string? amenaza, string? vulnerabilidad, string? consecuencia, int? idFactorRiesgo, int? idSubFactorRiesgo, int? idTipoRiesgo, string? causa, int? idProbabilidad, int? probabilidadValor, int? idImpacto, int? impactoValor, int? riesgoInherente, string? severidad, string? descripcion, int? idTipo, int? idAutomatizacion, int? idFrecuencia, int? valoresControl, double? riesgoResidualValor, int? severidadValor, string? accion, string? resonsable, int? idTiempoEjecucion, int? idTratamientoRiesgo)
        {
            try
            {
                var riesgo = _context.Riesgos.Find(identificador);

                riesgo.Accion = accion;
                riesgo.Amenaza = amenaza;
                riesgo.Causa = causa;
                riesgo.Consecuencia = consecuencia;
                riesgo.Descripcion = descripcion;
                riesgo.FkAutomatizacion = idAutomatizacion;
                riesgo.FkFactorRiesgo = idFactorRiesgo;
                riesgo.FkSubFactorRiesgo = idSubFactorRiesgo;
                riesgo.FkFrecuencia = idFrecuencia;
                riesgo.FkImpacto = idImpacto;
                riesgo.FkProbabilidad = idProbabilidad;
                riesgo.FkTiempoEjecucion = idTiempoEjecucion;
                riesgo.FkTipoRiesgo = idTipo;
                riesgo.FkTratamientoRiesgo = idTratamientoRiesgo;
                riesgo.ImpactoValor = impactoValor.ToString();
                riesgo.ProbabilidadValor = probabilidadValor.ToString();
                riesgo.Responsable = resonsable;
                riesgo.RiesgoInherente = riesgoInherente.ToString();
                riesgo.RiesgoResidualValor = riesgoResidualValor.ToString();
                riesgo.Severidad = severidad;
                riesgo.SeveridadValor = severidadValor.ToString();
                riesgo.ValoresControl = valoresControl.ToString();
                riesgo.Vulnerabilidad = vulnerabilidad;
                riesgo.FktipoRiesgoR = idTipoRiesgo;

                _context.Update(riesgo);

                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                    { nameof(identificador), identificador },
                    { nameof(accion), accion },
                    { nameof(amenaza), amenaza },
                    { nameof(causa), causa },
                    { nameof(consecuencia), consecuencia },
                    { nameof(descripcion), descripcion },
                    { nameof(idAutomatizacion), idAutomatizacion },
                    { nameof(idFactorRiesgo), idFactorRiesgo },
                    { nameof(idSubFactorRiesgo), idSubFactorRiesgo },
                    { nameof(idFrecuencia), idFrecuencia },
                    { nameof(idImpacto), idImpacto },
                    { nameof(idProbabilidad), idProbabilidad },
                    { nameof(idTiempoEjecucion), idTiempoEjecucion },
                    { nameof(idTipo), idTipo },
                    { nameof(idTratamientoRiesgo), idTratamientoRiesgo },
                    { nameof(impactoValor), impactoValor },
                    { nameof(probabilidadValor), probabilidadValor },
                    { nameof(resonsable), resonsable },
                    { nameof(riesgoInherente), riesgoInherente },
                    { nameof(riesgoResidualValor), riesgoResidualValor },
                    { nameof(severidad), severidad },
                    { nameof(severidadValor), severidadValor },
                    { nameof(valoresControl), valoresControl },
                    { nameof(vulnerabilidad), vulnerabilidad },
                    { nameof(idTipoRiesgo), idTipoRiesgo },
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al conectar a BD",
                    data);

                throw e.InnerException ?? e;
            }
        }
        /// <summary>
        /// Consultar Riesgos
        /// </summary>
        /// <returns><see cref="List{Riesgo}"/></returns>
        public List<Riesgo> ConsultarRiesgosPorPeriodo(int periodo, string? estatus = "ALL")
        {
            try
            {
                var riesgos = _context.Riesgos.Where(r => r.FkPeriodo == periodo).Include(r => r.FkPeriodoNavigation).Include(r => r.FkFactorRiesgoNavigation).ToList();

                if(!estatus.Equals("ALL"))
                {
                    riesgos = riesgos.Where(r => r.FkPeriodoNavigation.Estatus == estatus).ToList();
                }

                return riesgos;
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                    { nameof(periodo), periodo }
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al conectar a BD",
                    data);

                throw e.InnerException ?? e;
            }
        }
        /// <summary>
        /// Eliminar Riesgo
        /// </summary>
        public void EliminarRiesgo(int idRiesgo)
        {
            try
            {
                var riesgo = _context.Riesgos.Find(idRiesgo);

                _context.Riesgos.Remove(riesgo);

                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                    { nameof(idRiesgo), idRiesgo }
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al conectar a BD",
                    data);

                throw e.InnerException ?? e;
            }
        }
    }
}
