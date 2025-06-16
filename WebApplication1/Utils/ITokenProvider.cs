using System.Security.Claims;

namespace WebApplication1.Utils
{
    public interface ITokenProvider
    {
        List<Claim> GetTokenList();
        public string GetUserName();
        public string GetRol();
        public string GetUserId();
        public List<string> GetAllVariables();
    }
}