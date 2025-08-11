using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AccountController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
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

                // Temporary mock login for testing when API is not available
                if (model.Username == "admin" && model.Password == "admin")
                {
                    // Session'a bilgileri kaydet
                    HttpContext.Session.SetString("JWTToken", "mock-token-admin");
                    HttpContext.Session.SetString("UserRole", "Admin");
                    HttpContext.Session.SetString("UserName", "admin");
                    HttpContext.Session.SetString("CurrentUserId", "11"); // Admin user ID
                    
                    // Authentication cookie oluştur
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, "admin"),
                        new Claim(ClaimTypes.Role, "Admin"),
                        new Claim("UserName", "admin")
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
                    System.Diagnostics.Debug.WriteLine("Admin login successful, redirecting to Dashboard");
                    return RedirectToAction("Dashboard", "Admin");
                }
                else if (model.Username == "user" && model.Password == "user")
                {
                    // Session'a bilgileri kaydet
                    HttpContext.Session.SetString("JWTToken", "mock-token-user");
                    HttpContext.Session.SetString("UserRole", "User");
                    HttpContext.Session.SetString("UserName", "user");
                    HttpContext.Session.SetString("CurrentUserId", "12"); // User ID
                    
                    // Authentication cookie oluştur
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, "user"),
                        new Claim(ClaimTypes.Role, "User"),
                        new Claim("UserName", "user")
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
                    System.Diagnostics.Debug.WriteLine("User login successful, redirecting to Create Complaint");
                    return RedirectToAction("Create", "Complaint");
                }
                else if (model.Username == "personnel" && model.Password == "personnel")
                {
                    // Session'a bilgileri kaydet
                    HttpContext.Session.SetString("JWTToken", "mock-token-personnel");
                    HttpContext.Session.SetString("UserRole", "Personnel");
                    HttpContext.Session.SetString("UserName", "personnel");
                    HttpContext.Session.SetString("CurrentUserId", "13"); // Personnel ID
                    
                    // Authentication cookie oluştur
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, "personnel"),
                        new Claim(ClaimTypes.Role, "Personnel"),
                        new Claim("UserName", "personnel")
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
                    System.Diagnostics.Debug.WriteLine("Personnel login successful, redirecting to MyAssignments");
                    return RedirectToAction("MyAssignments", "Assignment");
                }

                // Original API call (commented out for now)
                /*
                var loginData = new
                {
                    Username = model.Username,
                    Password = model.Password
                };

                var json = JsonSerializer.Serialize(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var loginResult = JsonSerializer.Deserialize<LoginResponse>(responseContent);

                    // Store token in session or cookie
                    HttpContext.Session.SetString("JWTToken", loginResult.Token);
                    HttpContext.Session.SetString("UserRole", loginResult.Role);
                    HttpContext.Session.SetString("UserName", loginResult.Username);

                    TempData["Success"] = "Başarıyla giriş yaptınız!";

                    // Redirect based on role
                    return loginResult.Role switch
                    {
                        "Admin" => RedirectToAction("Dashboard", "Admin"),
                        "Personnel" => RedirectToAction("MyAssignments", "Assignment"),
                        "User" => RedirectToAction("Create", "Complaint"),
                        _ => RedirectToAction("Index", "Home")
                    };
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı!");
                    return View(model);
                }
                */

                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı!");
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Giriş yapılırken bir hata oluştu: " + ex.Message);
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
                Role = HttpContext.Session.GetString("UserRole")
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