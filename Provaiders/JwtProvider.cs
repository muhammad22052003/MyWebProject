using WebProject.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;
using SHA3.Net;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Security.Claims;
using WebProject.interfaces.auth;
using WebProject.Provaiders.options;

namespace WebProject.Provaiders
{
    public struct TokenDecodeResult
    {
        public string Token {  get; set; }

        public string Email { get; set; }

        public string UserId { get; set; }
    }

    public class JwtProvider : IJwtProvider
    {
        private IConfiguration _configuration;

        public JwtProvider(IConfiguration configuration) => _configuration = configuration;

        public string GenerateToken(User user)
        {
            List<Claim> claims = [new("id", user.Id),new("empty", ""), new("email", user.Email)];

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JwtOptions:SecretKey"))),
                                                  SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(signingCredentials: signingCredentials,
                                             expires: DateTime.Now.AddHours(int.Parse(_configuration.GetValue<string>("JwtOptions:ExperiseHours"))),
                                             claims: claims);

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }

        public TokenDecodeResult DecodeJwtToken(string token)
        {
            TokenDecodeResult result = new TokenDecodeResult();

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);

            // Получение данных из токена
            var userId = jsonToken.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            var email = jsonToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

            result.Token = token;
            result.Email = email;
            result.UserId = userId;

            return result;
        }
    }
}
