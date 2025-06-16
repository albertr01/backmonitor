using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.DB;
using WebApplication1.Utils;

namespace WebApplication1.Connections.BD
{
    public class PeriodoMetodos
    {
        private readonly MonitoreopyaContext _context;
        private readonly ManagementLogs _managementLogs;

        public PeriodoMetodos(MonitoreopyaContext context, ILogger<ControllerBase> logger)
        {
            _context = context;
            _managementLogs = new ManagementLogs(logger);
        }
        /// <summary>
        /// Cosulta Periodos
        /// </summary>
        /// <returns><see cref="List{Periodo}"/></returns>
        public List<Periodo> ConsultaPeriodos(string estatus)
        {
            try
            {
                if(estatus == "ALL")
                {
                    return _context.Periodos.ToList();
                }
                var respuesta = _context.Periodos.Where(p => p.Estatus == estatus).ToList();

                return respuesta;
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                    { nameof(estatus), estatus }
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al conectar a BD",
                    data);

                throw e.InnerException ?? e;
            }
        }
        /// <summary>
        /// Actualizar estado de periodo
        /// </summary>
        public void ActualizarEstadoPeriodo(int id, string estatus)
        {
            try
            {
                var respuesta = _context.Periodos.Find(id);

                respuesta.Estatus = estatus;
                respuesta.FechaCierre = DateTime.Now;

                _context.Update(respuesta);

                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                    { nameof(id), id }
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al conectar a BD",
                    data);

                throw e.InnerException ?? e;
            }
        }
        /// <summary>
        /// Registrar Periodo
        /// </summary>
        /// <returns><see cref="bool"/></returns>
        public Periodo RegistrarPeriodo(string periodo, string tipoPeriodo, DateTime fechaInicio, DateTime fechaFin, string nombreEvaluacion)
        {
            try
            {
                //if(!_context.Periodos.Any(p => p.Periodo1 == periodo && p.TipoPeriodo == tipoPeriodo))
                //{
                    var periodoBD = _context.Periodos.Add(new Periodo
                    {
                        Estatus = "ACTIVO",
                        FechaCierre = null,
                        FechaCreacion = DateTime.Now,
                        Periodo1 = periodo,
                        TipoPeriodo = tipoPeriodo,
                        FechaInicio = fechaInicio,
                        FechaFin = fechaFin,
                        Nombre = nombreEvaluacion
                    });

                    _context.SaveChanges();

                    return periodoBD.Entity;
                //}

                //return null;
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                    {nameof(periodo), periodo},
                    {nameof(tipoPeriodo), tipoPeriodo},
                    {nameof(fechaInicio), fechaInicio},
                    {nameof(fechaFin), fechaFin},
                    {nameof(nombreEvaluacion), nombreEvaluacion}
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al conectar a BD",
                    data);

                throw e.InnerException ?? e;
            }
        }
    }
}
