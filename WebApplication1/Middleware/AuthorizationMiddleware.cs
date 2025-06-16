namespace WebApplication1.Middleware
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Models.DB;

    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthorizationMiddleware> _logger;

        public AuthorizationMiddleware(RequestDelegate next, ILogger<AuthorizationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, MonitoreopyaContext dbContext)
        {
            // Obtener la metadata del endpoint actual
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                var descriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();

                if (descriptor != null)
                {
                    // Verificar si el endpoint NO tiene el atributo [Authorize], lo que significa que es público
                    bool isPublic = descriptor.MethodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any() ||
                                    !descriptor.MethodInfo.GetCustomAttributes(typeof(AuthorizeAttribute), true).Any();

                    if (isPublic)
                    {
                        await _next(context);
                        return;
                    }
                }
            }

            // Obtener el token del encabezado Authorization
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Token de autenticación no proporcionado.");
                return;
            }

            // Extraer información del JWT
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
            if (jwtToken == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Token inválido.");
                return;
            }

            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            var rolIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "RolId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || string.IsNullOrEmpty(rolIdClaim))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("No se pudo obtener la información del usuario o rol.");
                return;
            }

            int rolId = int.Parse(rolIdClaim);
            string requestedEndpoint = context.Request.Path.Value.ToLower();

            _logger.LogInformation($"Validando acceso del rol {rolId} al endpoint {requestedEndpoint}");

            // Verificar si el rol tiene autorización para acceder al endpoint
            var tieneAcceso = await dbContext.Autorizaciones
                .Include(a => a.Accion)
                .AnyAsync(a => a.RolId == rolId && a.Accion.Endpoint.ToUpper() == requestedEndpoint.ToUpper());

            if (!tieneAcceso)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Acceso denegado: No tienes permisos para este recurso.");
                return;
            }

            await _next(context);
        }
    }
}
