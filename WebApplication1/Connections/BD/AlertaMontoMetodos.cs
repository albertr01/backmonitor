using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.DB;
using WebApplication1.Models.Response.Local.Salida;
using WebApplication1.Utils;

namespace WebApplication1.Connections.BD
{
    public class AlertaMontoMetodos
    {
        private readonly MonitoreopyaContext _context;
        private readonly ManagementLogs _managementLogs;

        public AlertaMontoMetodos(MonitoreopyaContext context, ILogger<ControllerBase> logger)
        {
            _context = context;
            _managementLogs = new ManagementLogs(logger);
        }
        /// <summary>
        /// Registrar Alerta Monto
        /// </summary>
        /// <returns><see cref="bool"/></returns>
        public Models.DB.AlertaMonto RegistrarAlertaMonto(DateTime fecha, int idTipoOperacion, string codigoTipoOperacion, int idTipoMoneda, decimal umbral, string comentario, int idAgencia, int idCliente)
        {
            try
            {
                var ultimaAlerta = _context.AlertaMontos.OrderByDescending(a => a.Id).ToList().FirstOrDefault();

                string codigo = String.Empty;

                if (ultimaAlerta == null)
                {
                    codigo = $"{codigoTipoOperacion}-1";
                }
                else
                {
                    codigo = $"{codigoTipoOperacion}-{ultimaAlerta.Id+1}";
                }

                var alertaMonto = _context.AlertaMontos.Add(new Models.DB.AlertaMonto
                {
                    Comentario = comentario,
                    FechaConfiguracion = fecha,
                    IdTipoMoneda = idTipoMoneda,
                    IdTipoOperacion = idTipoOperacion,
                    Umbral = umbral,
                    Codigo = codigo,
                    IdAgencia = idAgencia,
                    IdCliente = idCliente
                });

                _context.SaveChanges();

                return alertaMonto.Entity;
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                    {nameof(fecha), fecha},
                    {nameof(idTipoOperacion), idTipoOperacion},
                    {nameof(codigoTipoOperacion), codigoTipoOperacion},
                    {nameof(idTipoMoneda), idTipoMoneda},
                    {nameof(umbral), umbral},
                    {nameof(comentario), comentario},
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al conectar a BD",
                    data);

                throw e.InnerException ?? e;
            }
        }
        /// <summary>
        /// Consultar Tipos Operacion
        /// </summary>
        /// <returns><see cref="List{TipoOperacion}"/></returns>
        public List<Models.DB.TipoOperacion> ConsultarListaOperacion()
        {
            try
            {
                var tipoOperaciones = _context.TipoOperacions.ToList();

                return tipoOperaciones;
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al conectar a BD",
                    data);

                throw e.InnerException ?? e;
            }
        }
        /// <summary>
        /// Consultar Tipos Monedas
        /// </summary>
        /// <returns><see cref="List{TipoMonedum}"/></returns>
        public List<Models.DB.TipoMonedum> ConsultarListaMonedas()
        {
            try
            {
                var tipoMonedas = _context.TipoMoneda.ToList();

                return tipoMonedas;
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al conectar a BD",
                    data);

                throw e.InnerException ?? e;
            }
        }
        /// <summary>
        /// Consultar Alerta de montos
        /// </summary>
        /// <returns><see cref="List{AlertaMonto}"/></returns>
        public List<Models.DB.AlertaMonto> ConsultarAlertasMonto()
        {
            try
            {
                var alertaMontos = _context.AlertaMontos.ToList();

                return alertaMontos;
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al conectar a BD",
                    data);

                throw e.InnerException ?? e;
            }
        }
        /// <summary>
        /// Consultar Agencias
        /// </summary>
        /// <returns><see cref="List{Agencium}"/></returns>
        public List<Models.DB.Agencium> ConsultarAgencias()
        {
            try
            {
                var agencias = _context.Agencia.ToList();

                return agencias;
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al conectar a BD",
                    data);

                throw e.InnerException ?? e;
            }
        }
        /// <summary>
        /// Consultar Alerta de monto
        /// </summary>
        /// <returns><see cref="AlertaMonto"/></returns>
        public Models.DB.AlertaMonto ConsultarAlertasMonto(int id)
        {
            try
            {
                var alertaMonto = _context.AlertaMontos.Include(a => a.IdTipoOperacionNavigation).Include(a => a.IdTipoMonedaNavigation).FirstOrDefault(a => a.Id == id);

                return alertaMonto;
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al conectar a BD",
                    data);

                throw e.InnerException ?? e;
            }
        }
        /// <summary>
        /// Modificar Alerta Monto
        /// </summary>
        /// <returns><see cref="bool"/></returns>
        public void ModificarAlertaMonto(int id, DateTime fecha, int idTipoOperacion, int idTipoMoneda, decimal umbral, string comentario, 
                                        int idCliente, int idAgencia)
        {
            try
            {
                var alertaMonto = _context.AlertaMontos.FirstOrDefault(x => x.Id == id);

                alertaMonto.FechaConfiguracion = fecha;
                alertaMonto.IdTipoMoneda = idTipoMoneda;
                alertaMonto.IdTipoOperacion = idTipoOperacion;
                alertaMonto.Umbral = umbral;
                alertaMonto.Comentario = comentario;
                alertaMonto.IdCliente = idCliente;
                alertaMonto.IdAgencia = idAgencia;

                _context.AlertaMontos.Update(alertaMonto);

                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                    {nameof(id), id},
                    {nameof(fecha), fecha},
                    {nameof(idTipoOperacion), idTipoOperacion},
                    {nameof(idTipoMoneda), idTipoMoneda},
                    {nameof(umbral), umbral},
                    {nameof(idCliente), idCliente},
                    {nameof(idAgencia), idAgencia},
                    {nameof(comentario), comentario},
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al conectar a BD",
                    data);

                throw e.InnerException ?? e;
            }
        }
        /// <summary>
        /// Eliminar Alerta Monto
        /// </summary>
        public void EliminarAlertaMonto(int id)
        {
            try
            {
                var alertaMonto = _context.AlertaMontos.FirstOrDefault(x => x.Id == id);

                _context.Remove(alertaMonto);

                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                    {nameof(id), id},
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al conectar a BD",
                    data);

                throw e.InnerException ?? e;
            }
        }
    }
}
