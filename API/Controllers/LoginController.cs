using Application.Dtos;
using Application.Features.Results.AppUserResults;
using Application.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistance.Context;
using Microsoft.EntityFrameworkCore;
namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly BiCozumContext _context;

        public LoginController(BiCozumContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _context.Users
                .Include(x => x.AppRole)
                .FirstOrDefaultAsync(x => x.Username == loginDto.Username);

            if (user == null || user.PasswordHash != loginDto.Password)
                return Unauthorized("Geçersiz kullanıcı adı veya şifre.");

            var result = new GetCheckUserQueryResult
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.AppRole?.AppRoleName ?? "Visitor"
            };

            var token = JwtTokenGenerator.GenerateToken(result);

            user.RefreshToken = token.RefreshToken;
            user.RefreshTokenExpireDate = token.RefreshTokenExpireDate;
            await _context.SaveChangesAsync();

            Response.Cookies.Append("access_token", token.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Expires = token.ExpireDate,
                SameSite = SameSiteMode.Strict,
                Secure = true
            });

            return Ok(token);
        }
    }
}
