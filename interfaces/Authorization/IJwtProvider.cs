using WebProject.Models;
using WebProject.Provaiders;

namespace WebProject.interfaces.auth
{
    public interface IJwtProvider
    {
        public string GenerateToken(User user);

        public TokenDecodeResult DecodeJwtToken(string token);
    }
}