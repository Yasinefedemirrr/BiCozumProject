using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            // Role göre yönlendirme
            var userRole = HttpContext.Session.GetString("UserRole");
            
            return userRole switch
            {
                "Admin" => RedirectToAction("Dashboard", "Admin"),
                "Personnel" => RedirectToAction("MyAssignments", "Assignment"),
                "User" => RedirectToAction("Create", "Complaint"),
                _ => RedirectToAction("Login", "Account")
            };
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
