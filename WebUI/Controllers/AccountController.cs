using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using WebUI.Models;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using Domain.Entity;

namespace WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly BiCozumContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AccountController(BiCozumContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Debug için log ekleyelim
                System.Diagnostics.Debug.WriteLine($"Login attempt: {model.Username}");

                // Veritabanından kullanıcıyı kontrol et
                var user = await _context.Users
                    .Include(u => u.AppRole)
                    .FirstOrDefaultAsync(u => u.Username == model.Username);

                if (user == null || user.PasswordHash != model.Password)
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı!");
                    return View(model);
                }

                // Session'a bilgileri kaydet
                HttpContext.Session.SetString("JWTToken", $"token-{user.Id}");
                HttpContext.Session.SetString("UserRole", user.AppRole.AppRoleName);
                HttpContext.Session.SetString("UserName", user.Username);
                HttpContext.Session.SetString("CurrentUserId", user.Id.ToString());
                HttpContext.Session.SetString("UserFullName", user.FullName);

                // Authentication cookie oluştur
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.AppRole.AppRoleName),
                    new Claim("UserName", user.Username),
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("UserFullName", user.FullName)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties);

                TempData["Success"] = "Başarıyla giriş yaptınız!";
                System.Diagnostics.Debug.WriteLine($"Login successful for {user.Username} with role {user.AppRole.AppRoleName}");

                // Role ID'sine göre yönlendirme
                return user.AppRoleId switch
                {
                    1 => RedirectToAction("Dashboard", "Admin"),        // Admin
                    2 => RedirectToAction("Create", "Complaint"),       // User
                    3 => RedirectToAction("MyAssignments", "Assignment"), // Personnel
                    _ => RedirectToAction("Index", "Home")
                };
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Giriş yapılırken bir hata oluştu: " + ex.Message);
                System.Diagnostics.Debug.WriteLine($"Login error: {ex.Message}");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["Success"] = "Başarıyla çıkış yaptınız!";
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            try
            {
                // Validation
                if (string.IsNullOrEmpty(model.FullName) || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                {
                    return Json(new { success = false, message = "Tüm alanları doldurunuz." });
                }

                // Username kontrolü
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
                if (existingUser != null)
                {
                    return Json(new { success = false, message = "Bu kullanıcı adı zaten kullanılıyor." });
                }

                // User rolünü bul (AppRoleId = 2)
                var userRole = await _context.AppRoles.FirstOrDefaultAsync(r => r.AppRoleId == 2);
                if (userRole == null)
                {
                    return Json(new { success = false, message = "Sistem hatası: User rolü bulunamadı." });
                }

                // Yeni kullanıcı oluştur
                var newUser = new User
                {
                    FullName = model.FullName,
                    Username = model.Username,
                    PasswordHash = model.Password, // Gerçek uygulamada hash'lenmeli
                    AppRoleId = 2, // User rolü
                    DepartmentId = null // Yeni kayıtlarda müdürlük yok
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Kayıt başarıyla tamamlandı! Artık giriş yapabilirsiniz." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Kayıt işlemi sırasında bir hata oluştu: " + ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Profile()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }

            var profileModel = new ProfileViewModel
            {
                Username = HttpContext.Session.GetString("UserName"),
                Role = HttpContext.Session.GetString("UserRole"),
                FullName = HttpContext.Session.GetString("UserFullName")
            };

            return View(profileModel);
        }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public string Username { get; set; }
    }
} 