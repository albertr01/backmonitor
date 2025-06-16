using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models.DB;
using WebApplication1.Services;


namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly LdapService _ldapService;
        private readonly MonitoreopyaContext _context;

        public UsuariosController(LdapService ldapService, MonitoreopyaContext context)
        {
            _ldapService = ldapService;
            _context = context;
        }

        // Obtener lista de usuarios desde Active Directory
        [HttpGet("listarAD")]
        //[Authorize(Roles = "Administrador, , Oficial de Cumplimiento")]
        [Authorize]
        public IActionResult BuscarUsuarios()
        {
            var result = _ldapService.BuscarUsuarios(null, null);
            if (result != null)
            {
                return Ok(new { data = result });
            }
            return NotFound("No se encontraron usuarios.");
        }


        // Obtener lista de usuarios desde Active Directory
        [HttpGet("buscarUsuarioAD")]
        //[Authorize(Roles = "Administrador, , Oficial de Cumplimiento")]
        [Authorize]
        public IActionResult BuscarUsuarios([FromQuery] string? nombre = null, [FromQuery] string? email = null)
        {
            var result = _ldapService.BuscarUsuarios(nombre, email);
            if (result != null)
            {
                return Ok(new { data = result });
            }
            return NotFound("No se encontraron usuarios.");
        }

        // Autorizar usuario
        [HttpPost("actualizarUsuario")]
        [Authorize]
        public async Task<IActionResult> AutorizarUsuario([FromBody] AutorizarUsuarioRequest request)
        {
            var usuarioExistente = _context.UsuariosAutorizados.FirstOrDefault(u => u.UsuarioAd == request.UsuarioAD);
            if (usuarioExistente != null)
            {

                usuarioExistente.Estatus = (byte)request.Estatus;
                usuarioExistente.RolId = request.RolId;
                _context.UsuariosAutorizados.Update(usuarioExistente);
                await _context.SaveChangesAsync();

                return Ok("El usuario se ha actualizado.");
            }

            var nuevoUsuario = new UsuariosAutorizado
            {
                UsuarioAd = request.UsuarioAD,
                RolId = request.RolId
            };

            _context.UsuariosAutorizados.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            return Ok("Usuario autorizado exitosamente.");
        }

        // Revocar autorización de un usuario
        [HttpDelete("revocar/{usuarioAD}")]
        [Authorize]
        public async Task<IActionResult> RevocarUsuario(string usuarioAD)
        {
            var usuario = _context.UsuariosAutorizados.FirstOrDefault(u => u.UsuarioAd == usuarioAD);
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }
            usuario.Estatus = 0; 

            _context.UsuariosAutorizados.Update(usuario);
            await _context.SaveChangesAsync();

            return Ok("Acceso revocado exitosamente.");
        }

        // Obtener lista de usuarios autorizados con datos de Active Directory
        [HttpGet("usuarios-autorizados")]
        [Authorize]
        public IActionResult ObtenerUsuariosAutorizados()
        {
            var usuariosAutorizados = _context.UsuariosAutorizados.ToList();
            var usuariosAD = _ldapService.BuscarUsuarios();

            var resultado = usuariosAutorizados.Select(u => new
            {
                UsuarioAD = u.UsuarioAd,
                Nombre = usuariosAD.FirstOrDefault(ad => ad.Usuario == u.UsuarioAd)?.Nombre ?? "Desconocido",
                Email = usuariosAD.FirstOrDefault(ad => ad.Usuario == u.UsuarioAd)?.Email ?? "Desconocido",
                RolId = u.RolId,
                Estatus  = u.Estatus
            });

            return Ok(new { data = resultado });
        }
    }

    public class AutorizarUsuarioRequest
    {
        public string UsuarioAD { get; set; } // Nombre de usuario en Active Directory
        public int RolId { get; set; } // ID del rol asignado
        public int Estatus { get; set;  }
    }

}
