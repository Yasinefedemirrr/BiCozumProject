using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tools
{
    public class JwtTokenDefaults
    {
        public const string ValidAudience = "http://localhost";
        public const string ValidIssuer = "http://localhost";
        public const string Key = "jwttokenjwttokenjwttokenjwttoken"; // 32+ karakter olmalı
        public const int AccessTokenExpireMinutes = 5; // Access Token süresi
        public const int RefreshTokenExpireDays = 7;   // Refresh Token süresi
    }
}
