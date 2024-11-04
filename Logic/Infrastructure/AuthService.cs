using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Logic.DAL.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Logic.Infrastructure;

public class AuthService(Config config)
{
    public string GenerateJwtToken(UserEntity user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.MobilePhone, user.Phone)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("dKt3Y#9^3nTv%2GpB&y8U@C*#w!WqS6D"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "ArtApp",
            audience: "ArtUsers",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);
        

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}