using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.DB;
using WebApplication1.Utils;

namespace WebApplication1.Connections.BD
{
    public class MstParametrosRiesgoMetodos
    {
        private readonly MonitoreopyaContext _context;
        private readonly ManagementLogs _managementLogs;

        public MstParametrosRiesgoMetodos(MonitoreopyaContext context, ILogger<ControllerBase> logger)
        {
            _context = context;
            _managementLogs = new ManagementLogs(logger);
        }
        
        /// <summary>
        /// Consultar Tablas de Riesgos
        /// </summary>
        /// <returns><see cref="List{Riesgo}"/></returns>
        public List<MstParametrosRiesgo> ConsultarTablaMstParametrosRiego(int idTabla)
        {
            try
            {
                var riesgos = _context.MstParametrosRiesgos.Where(r => r.IdTblParametro == idTabla).ToList();

                return riesgos;
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                    { nameof(idTabla), idTabla }
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al conectar a BD",
                    data);

                throw e.InnerException ?? e;
            }
        }
    }
}
