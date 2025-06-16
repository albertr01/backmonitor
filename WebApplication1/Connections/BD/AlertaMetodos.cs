using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.DB;
using WebApplication1.Utils;

namespace WebApplication1.Connections.BD
{
    public class AlertaMetodos
    {
        private readonly MonitoreopyaContext _context;
        private readonly ManagementLogs _managementLogs;

        public AlertaMetodos(MonitoreopyaContext context, ILogger<ControllerBase> logger)
        {
            _context = context;
            _managementLogs = new ManagementLogs(logger);
        }
        /// <summary>
        /// Consultar Alertas
        /// </summary>
        /// <returns><see cref="List{Alertum}"/></returns>
        public List<Models.DB.Alertum> ConsultarAlertas()
        {
            try
            {
                var alertas = _context.Alerta.ToList();

                return alertas;
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
    }
}
