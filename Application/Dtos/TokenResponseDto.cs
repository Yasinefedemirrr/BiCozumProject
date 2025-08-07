using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class TokenResponseDto
    {
        public string AccessToken { get; set; }
        public DateTime ExpireDate { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpireDate { get; set; }

        public TokenResponseDto(string token, DateTime expireDate, string refreshToken, DateTime refreshTokenExpireDate)
        {
            AccessToken = token;
            ExpireDate = expireDate;
            RefreshToken = refreshToken;
            RefreshTokenExpireDate = refreshTokenExpireDate;
        }
    }
}
