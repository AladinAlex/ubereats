using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ubereats.Models.Authentication.JWT
{
    public interface IJwtConfiguration
    {
        string GenerateToken(int UserID, string loginname, string password);
        TokenValidationParameters GetTokenValidationParameters();
        JwtSecurityToken GetJwtSecurityToken(string token);
    }
}
