using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.DB;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsAuditoriaController : ControllerBase
    {
        private readonly MonitoreopyaContext _context;

        public LogsAuditoriaController(MonitoreopyaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LogsAuditorium>>> GetLogs(
            [FromQuery] string? usuario,
            [FromQuery] string? tipoEvento,
            [FromQuery] DateTime? fechaDesde,
            [FromQuery] DateTime? fechaHasta,
            [FromQuery] string? Modulo,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 1000)
        {
            var query = _context.LogsAuditoria.AsQueryable();

            if (!string.IsNullOrEmpty(usuario))
            {
                query = query.Where(log => log.IdUsuario == usuario);
            }
            if (!string.IsNullOrEmpty(tipoEvento))
            {
                query = query.Where(log => log.TpEvento == tipoEvento);
            }
            if (fechaDesde.HasValue)
            {
                var fechaDesdeAdjusted = fechaDesde.Value.AddDays(-1);
                query = query.Where(log => log.FechaConexion >= fechaDesdeAdjusted);
            }
            if (fechaHasta.HasValue)
            {
                var fechaHastaInclusive = fechaHasta.Value.AddDays(1);
                query = query.Where(log => log.FechaConexion < fechaHastaInclusive);
            }
            if (!string.IsNullOrEmpty(Modulo))
            {
                query = query.Where(log => log.Modulo == Modulo);
            }

            var totalRegistros = await query.CountAsync();
            var logs = await query
                .OrderByDescending(log => log.FechaConexion)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new { totalRegistros, logs });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LogsAuditorium>> GetLogById(int id)
        {
            var log = await _context.LogsAuditoria.FindAsync(id);
            if (log == null)
            {
                return NotFound();
            }
            return log;
        }
    }
}
