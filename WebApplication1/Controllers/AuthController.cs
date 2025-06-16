using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Security.Claims;
using WebApplication1.Models.DB;
using WebApplication1.Models.Request;
using WebApplication1.Services;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly LdapService _ldapService;
        private readonly MonitoreopyaContext _context;
        private readonly JwtService _jwtService;

        public AuthController(LdapService ldapService, MonitoreopyaContext db, JwtService jwtService)
        {
            _ldapService = ldapService;
            _context = db;
            _jwtService = jwtService;
        }

        // POST api/<AuthController>
        [HttpPost("login")]
        public IActionResult Login([FromBody] UsuarioLoginRequest model)
        {
            //Valida que los campos no sean nullos 
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Username and password are ");
            }

            // Obtener el usuario y su rol desde la base de datos
            var usuario = _context.UsuariosAutorizados
                .Include(u => u.Rol) // Incluir la información del rol
                .FirstOrDefault(u => u.UsuarioAd == model.Username);

            // Validar si el usuario existe y está autorizado
            if (usuario == null)
            {

                GuardarLog(model.Username, "N/A", "Intento de ingreso.");
                return Unauthorized(); // Usuario no encontrado o no autorizado
            }
            else {
                if (usuario.Estatus != 1) {
                    GuardarLog(model.Username, "N/A", "Usuario bloqueado");
                    return Unauthorized("Usuario se encuentra bloqueado"); // Usuario no encontrado o no autorizado
                }
            }

            //valida las credenciales contra LDAP
            if (_ldapService.AutenticarUsuario(model.Username, model.Password))
            {
                // Autenticación exitosa
                //Genera un token JWT u otro mecanismo de autenticación

                //Guardar auditoria
                GuardarLog(usuario.UsuarioAd, usuario.Rol.Nombre, "Login Correcto");
                // Generar Claims para el token
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.UsuarioAd),
                    new Claim(ClaimTypes.Role, usuario.Rol.Nombre),
                    new Claim("RolId", usuario.Rol.Id.ToString()),
                    new Claim("UserId", usuario.Id.ToString())
                };

                // Generar el nuevo token usando GenerateAccessToken
                var accessToken = _jwtService.GenerateAccessToken(claims);
                var refreshToken = _jwtService.GenerateRefreshToken(usuario.UsuarioAd);

                // Guardar el refreshToken en la base de datos
                var newRefreshToken = new RefreshToken
                {
                    UserId = usuario.Id,
                    Token = refreshToken,
                    Expiration = DateTime.UtcNow.AddDays(7),
                    CreatedAt = DateTime.UtcNow
                };
                _context.RefreshTokens.Add(newRefreshToken);
                _context.SaveChanges();


                return Ok(new
                {
                    accessToken = accessToken,
                    refreshToken = refreshToken
                });
            }
             else
             {
                // Autenticación fallida
                //Guardar auditoria
                GuardarLog(usuario.UsuarioAd, usuario.Rol.Nombre, "Login Fallido");
                return Unauthorized(); // Código 401
             }
        }

        [HttpPost("logout")]
        [Authorize] 
        public async Task<IActionResult> Logout()
        {
            try
            {
                // Obtener el token del encabezado Authorization
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest(new { message = "Token no proporcionado" });
                }

                // Extraer el UserId del token JWT
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return BadRequest(new { message = "No se pudo obtener el UserId del token" });
                }

                // Verificar si el token ya ha sido revocado
                var existingRevokedToken = await _context.RevokedTokens.FirstOrDefaultAsync(rt => rt.Token == token);
                if (existingRevokedToken != null)
                {
                    return Ok(new { message = "El token ya ha sido revocado" });
                }

                var revokedToken = new RevokedToken
                {
                    Token = token,
                    RevokedAt = DateTime.UtcNow,
                    UserId = userId // Asegurar que se asocia al usuario correcto
                };

                _context.RevokedTokens.Add(revokedToken);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Sesión cerrada correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        // GET api/<AuthController>
        [HttpGet("users")]
        [Authorize]
        public IActionResult Users() // Modelo para username/password
        {

            var result = _ldapService.BuscarUsuarios(null, null);
            if (result != null)
            {
                return Ok(new { data = result }); // O devuelve un token
            }
            else
            {
                // Autenticación fallida
                return Unauthorized(); // Código 401
            }
        }

        [HttpPost("refreshToken")]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var principal = _jwtService.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal == null)
            {
                return Unauthorized(new { message = "Token inválido o expirado" });
            }

            string username = principal.Identity.Name;

            // Validar si el refresh token está en la base de datos (debes implementar esta validación)
            //bool isValidRefreshToken = _jwtService.ValidateRefreshToken(username, request.RefreshToken);
            //if (!isValidRefreshToken)
            //{
            //    return Unauthorized(new { message = "Refresh Token inválido o expirado" });
            //}

            // Generar nuevos tokens
            var newAccessToken = _jwtService.GenerateAccessToken(principal.Claims.ToList());
            var newRefreshToken = _jwtService.GenerateRefreshToken(username); // Genera y guarda el nuevo Refresh Token

            return Ok(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken
            });
        }

        private void GuardarLog(string username, string rol, string evento) 
        {
            //TODO: Guardar traza de Acceso en auditoria
            var log = new LogsAuditorium
            {
                IdUsuario = username,
                IdOperacion = "LOGIN"!,
                TpUsuario = rol,
                FechaConexion = DateTime.Now,
                FechaIngreso = DateTime.Now,
                FechaDesconexion = null!,
                IpDispositivo = GetUserIPAddress(),
                BdTabla = "UsuariosAutorizados"!,
                TpEvento = evento,
                Modulo = "SEGURIDAD"
            };
            _context.LogsAuditoria.Add(log);
            _context.SaveChanges();

        }

        // Función auxiliar para obtener la dirección IP del usuario
        private string GetUserIPAddress()
        {
            // 1. Desde el Request (puede ser null si está detrás de un proxy)
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            // 2. Desde los headers (si estás detrás de un proxy)
            if (string.IsNullOrEmpty(ipAddress) && Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                ipAddress = Request.Headers["X-Forwarded-For"];
            }

            // 3. Si usas Kestrel y no estás detrás de un proxy
            if (string.IsNullOrEmpty(ipAddress) && Request.HttpContext.Connection.LocalIpAddress != null)
            {
                ipAddress = Request.HttpContext.Connection.LocalIpAddress.ToString();
            }

            return ipAddress;
        }
    }

    public class RefreshTokenRequest
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}

