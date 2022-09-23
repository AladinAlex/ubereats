using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ubereats
{
    public class JwtConfiguration
    {
        /// <summary>
        /// Default token configuration
        /// </summary>
        /// <returns></returns>
        public static async Task<TokenValidationParameters> GetTokenValidationParameters()
        {
            return new TokenValidationParameters()
            {
                // укзывает, будет ли валидироваться издатель при валидации токена
                ValidateIssuer = true,
                // строка, представляющая издателя
                ValidIssuer = Constants.issuer,
                // будет ли валидироваться потребитель токена
                ValidateAudience = true,
                // установка потребителя токена
                ValidAudience = Constants.audience,
                // будет ли валидироваться время существования
                ValidateLifetime = true,
                // установка ключа безопасности
                IssuerSigningKey = Constants.GetSymmetricSecurityKey(),
                // валидация ключа безопасности
                ValidateIssuerSigningKey = true
            };
        }
        public static async Task<TokenValidationParameters> GetTokenValidationParameters(bool _validateIssuer, string _validIssuer, bool _validateAudience,
                                                                                string _validAudience, bool _validateLifetime, bool _validateIssuerSigningKey)
        {
            return new TokenValidationParameters()
            {
                // укзывает, будет ли валидироваться издатель при валидации токена
                ValidateIssuer = _validateIssuer,
                // строка, представляющая издателя
                ValidIssuer = _validIssuer,
                // будет ли валидироваться потребитель токена
                ValidateAudience = _validateAudience,
                // установка потребителя токена
                ValidAudience = _validAudience,
                // будет ли валидироваться время существования
                ValidateLifetime = _validateLifetime,
                // установка ключа безопасности
                IssuerSigningKey = Constants.GetSymmetricSecurityKey(),
                // валидация ключа безопасности
                ValidateIssuerSigningKey = _validateIssuerSigningKey
            };
        }

        public static async Task<string?> GetJwtSecurityToken()
        {
            // claim - требование, что требуется для jwt-токена
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, "admin"), // пользователь
                new Claim(JwtRegisteredClaimNames.Email, "admin@mail.ru")
            };

            var token = new JwtSecurityToken(Constants.issuer,
                Constants.audience,
                claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(60), // токен действителен час с момента создания
                signingCredentials: await GetSigningCredentials()  // алгоритм шифрования
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public static async Task<string?> GetJwtSecurityToken(string _subject, string _email, string _issuer = Constants.issuer, string _audience = Constants.audience,
                                            DateTime? _notBefore = null, DateTime? _expires = null, SigningCredentials _signingCredentials = null)
        {
            // claim - требование, что требуется для jwt-токена
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _subject), // пользователь
                new Claim(JwtRegisteredClaimNames.Email, _email)
            };

            var token = new JwtSecurityToken(_issuer,
                _audience,
                claims,
                notBefore: _notBefore ?? DateTime.Now,
                expires: _expires ?? DateTime.Now.AddMinutes(60), // токен действителен час с момента создания
                signingCredentials: _signingCredentials ?? await GetSigningCredentials() // алгоритм шифрования
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static async Task<SigningCredentials> GetSigningCredentials()
        {
            return new SigningCredentials(Constants.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);
        }
    }
}