namespace WebApplication1.Middleware
{
    using Microsoft.AspNetCore.Http;
    using System.Linq;
    using System.Threading.Tasks;
    using WebApplication1.Models.DB;

    public class TokenRevocationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;

        public TokenRevocationMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _scopeFactory = scopeFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrEmpty(token))
            {
                // Crear un nuevo scope para obtener una instancia de MonitoreopyaContext
                using (var scope = _scopeFactory.CreateScope())
                {
                    var _context = scope.ServiceProvider.GetRequiredService<MonitoreopyaContext>();

                    var revokedToken = _context.RevokedTokens.FirstOrDefault(rt => rt.Token == token);
                    if (revokedToken != null)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Token inválido o revocado.");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }

}
