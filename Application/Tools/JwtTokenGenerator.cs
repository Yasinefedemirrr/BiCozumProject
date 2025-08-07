using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tools
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Application.Dtos;
    using Application.Features.Results.AppUserResults;
    using Microsoft.IdentityModel.Tokens;

    public class JwtTokenGenerator
    {
        public static TokenResponseDto GenerateToken(GetCheckAppUserQueryResult result)
        {
            var claims = new List<Claim>();

            if (!string.IsNullOrWhiteSpace(result.Role))
                claims.Add(new Claim(ClaimTypes.Role, result.Role));

            claims.Add(new Claim(ClaimTypes.NameIdentifier, result.Id.ToString()));

            if (!string.IsNullOrWhiteSpace(result.Username))
                claims.Add(new Claim("Username", result.Username));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtTokenDefaults.Key));
            var signinCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expireDate = DateTime.UtcNow.AddMinutes(JwtTokenDefaults.AccessTokenExpireMinutes);

            var token = new JwtSecurityToken(
                issuer: JwtTokenDefaults.ValidIssuer,
                audience: JwtTokenDefaults.ValidAudience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expireDate,
                signingCredentials: signinCredentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var accessToken = tokenHandler.WriteToken(token);


            var refreshToken = Guid.NewGuid().ToString();
            var refreshTokenExpireDate = DateTime.UtcNow.AddDays(JwtTokenDefaults.RefreshTokenExpireDays);

            return new TokenResponseDto(accessToken, expireDate, refreshToken, refreshTokenExpireDate);
        }
    }

}
