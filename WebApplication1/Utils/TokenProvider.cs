using System.Security.Claims;

namespace WebApplication1.Utils
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public List<Claim> GetTokenList()
        {
            var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            return identity.Claims.ToList();

        }
        public string GetUserName()
        {
            return GetTokenList()[0].Value;
        }
        public string GetRol()
        {
            return GetTokenList()[1].Value;
        }
        public string GetUserId()
        {
            return GetTokenList()[2].Value;
        }
        public List<string> GetAllVariables()
        {
            return GetTokenList().Select(x => x.Value).ToList();
        }

        
    }
}