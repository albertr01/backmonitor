namespace WebApplication1.Services
{

    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using WebApplication1.Models.DB;

    public class JwtService
    {
        private readonly IConfiguration _configuration;
        private static readonly Dictionary<string, string> _refreshTokens = new(); // Simulación en memoria
        private readonly MonitoreopyaContext _context; 
        public JwtService(IConfiguration configuration,  MonitoreopyaContext db)
        {
            _configuration = configuration;
            _context = db;
        }

        public string GenerateAccessToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15), // Expira en 15 min
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken(string username)
        {
            var refreshToken = Guid.NewGuid().ToString();
            _refreshTokens[username] = refreshToken; // Guardar en BD en un sistema real
            return refreshToken;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false, // Importante: No validar la expiración aquí
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Token inválido");
            }

            return principal;
        }

        public bool ValidateRefreshToken(int userId, string refreshToken)
        {
            var tokenRecord = _context.RefreshTokens
                .FirstOrDefault(rt => rt.UserId == userId && rt.Token == refreshToken && rt.Expiration > DateTime.UtcNow);

            return tokenRecord != null;
            return true;
        }
    }

}

