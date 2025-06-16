using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.DB;
using WebApplication1.Utils;

namespace WebApplication1.Connections.BD
{
    public class DebidaDiligenciaMetodos
    {
        private readonly MonitoreopyaContext _context;
        private readonly ManagementLogs _managementLogs;

        public DebidaDiligenciaMetodos(MonitoreopyaContext context, ILogger<ControllerBase> logger)
        {
            _context = context;
            _managementLogs = new ManagementLogs(logger);
        }
        /// <summary>
        /// Cosulta Debida Diligencia con la identificacion
        /// </summary>
        /// <param name="identificacion">Identificacion. Ejemplo: V55161565</param>
        /// <returns><see cref="DebidaDiligencium"/></returns>
        public DebidaDiligencium? ConsultaPorIdentificacion(string identificacion)
        {
            try
            {
                var respuesta = _context.DebidaDiligencia.Where(d => d.IdentificacionCliente == identificacion).FirstOrDefault();

                return respuesta;
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                    { nameof(identificacion), identificacion },
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al conectar a BD",
                    data);

                throw e.InnerException ?? e;
            }
        }
        /// <summary>
        /// Registra una nueva Debida Diligencia
        /// </summary>
        /// <param name="identificacion">Identificacion V55161565</param>
        /// <param name="debidaDiligencia">Indica si tiene debida diligencia</param>
        /// <param name="debidaDiligenciaVencida">Indica si tiene debida diligencia vencida</param>
        /// <param name="fechaDebidaDiligencia">Fecha de debida diligencia</param>
        /// <param name="observaciones">Observaciones</param>
        /// <param name="tipoDebidaDiligencia">Tipo de debida diligencia</param>
        public void RegistrarDebidaDiligencia(string identificacion, bool debidaDiligencia, bool debidaDiligenciaVencida, DateTime fechaDebidaDiligencia, string observaciones, string tipoDebidaDiligencia)
        {
            try
            {
                _context.DebidaDiligencia.Add(new DebidaDiligencium
                {
                    DebidaDiligencia = debidaDiligencia,
                    DebidaDiligenciaVencida = debidaDiligenciaVencida,
                    FechaDebidaDiligencia = fechaDebidaDiligencia,
                    IdentificacionCliente = identificacion,
                    Observaciones = observaciones,
                    TipoDebidaDiligencia = tipoDebidaDiligencia,
                });

                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                    { nameof(identificacion), identificacion },
                    { nameof(debidaDiligencia), debidaDiligencia },
                    { nameof(debidaDiligenciaVencida), debidaDiligenciaVencida },
                    { nameof(fechaDebidaDiligencia), fechaDebidaDiligencia },
                    { nameof(observaciones), observaciones },
                    { nameof(tipoDebidaDiligencia), tipoDebidaDiligencia },
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al conectar a BD",
                    data);

                throw e.InnerException ?? e;
            }
        }
        /// <summary>
        /// Edita una nueva Debida Diligencia
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="identificacion">Identificacion V55161565</param>
        /// <param name="debidaDiligencia">Indica si tiene debida diligencia</param>
        /// <param name="debidaDiligenciaVencida">Indica si tiene debida diligencia vencida</param>
        /// <param name="fechaDebidaDiligencia">Fecha de debida diligencia</param>
        /// <param name="observaciones">Observaciones</param>
        /// <param name="tipoDebidaDiligencia">Tipo de debida diligencia</param>
        public void ModificarDebidaDiligencia(int id, string identificacion, bool debidaDiligencia, bool debidaDiligenciaVencida, DateTime fechaDebidaDiligencia, string observaciones, string tipoDebidaDiligencia)
        {
            try
            {
                var respuesta = _context.DebidaDiligencia.Find(id);

                respuesta.DebidaDiligencia = debidaDiligencia;
                respuesta.DebidaDiligenciaVencida = debidaDiligenciaVencida;
                respuesta.FechaDebidaDiligencia = fechaDebidaDiligencia;
                respuesta.Observaciones = observaciones;
                respuesta.TipoDebidaDiligencia = tipoDebidaDiligencia;

                _context.Update(respuesta);

                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                    { nameof(identificacion), identificacion },
                    { nameof(debidaDiligencia), debidaDiligencia },
                    { nameof(debidaDiligenciaVencida), debidaDiligenciaVencida },
                    { nameof(fechaDebidaDiligencia), fechaDebidaDiligencia },
                    { nameof(observaciones), observaciones },
                    { nameof(tipoDebidaDiligencia), tipoDebidaDiligencia },
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al conectar a BD",
                    data);

                throw e.InnerException ?? e;
            }
        }
    }
}
