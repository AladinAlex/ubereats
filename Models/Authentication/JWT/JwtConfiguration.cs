using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Unicode;

namespace ubereats.Models.Authentication.JWT
{
    public class JwtConfiguration : IJwtConfiguration
    {
        readonly IConfiguration _configuration;
        private readonly byte[] Key;

        public JwtConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
            Key = Encoding.UTF8.GetBytes(_configuration["JWTConfig:Key"]!);
        }

        public TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters()
            {
                // укзывает, будет ли валидироваться издатель при валидации токена
                ValidateIssuer = true,
                // строка, представляющая издателя
                ValidIssuer = _configuration["JWTConfig:Issuer"]!,
                // будет ли валидироваться потребитель токена
                ValidateAudience = true,
                // установка потребителя токена
                ValidAudience = _configuration["JWTConfig:Audience"]!,
                // будет ли валидироваться время существования
                ValidateLifetime = true,
                // установка ключа безопасности
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                // валидация ключа безопасности
                ValidateIssuerSigningKey = true
            };
        }

        public string GenerateToken(int UserID, string loginname, string password)
        {
            var claims = new List<Claim> {
                new Claim(_configuration["TokenClaims:UserIDClaimType"], UserID.ToString()),
                new Claim(_configuration["TokenClaims:LoginnameClaimType"], loginname),
                new Claim(_configuration["TokenClaims:PasswordClaimType"], password)
            };

            var tokenDescriptor = new JwtSecurityToken(_configuration["JWTConfig:Issuer"]!,
                _configuration["JWTConfig:Audience"]!,
                claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(60), // токен действителен час с момента создания
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha256)  // алгоритм шифрования
                );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public JwtSecurityToken GetJwtSecurityToken(string token)
        {
            token = token.Replace("Bearer ", "");
            var jwtToken = new JwtSecurityToken(token);
            return jwtToken;
        }

        public bool IsTokenValid(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(
                    token,
                    new JwtConfiguration(_configuration).GetTokenValidationParameters(),
                    out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}