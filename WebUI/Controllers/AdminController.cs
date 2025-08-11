using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using WebUI.Models;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using Domain.Entity;

namespace WebUI.Controllers
{
    public class AdminController : Controller
    {
        private readonly BiCozumContext _context;

        public AdminController(BiCozumContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                // Get data from database
                var totalComplaints = await _context.Complaints.CountAsync();
                var pendingComplaints = await _context.Complaints.CountAsync(c => c.Status == "Talep Alındı");
                var completedComplaints = await _context.Complaints.CountAsync(c => c.Status == "İşlem Tamamlandı");
                var totalUsers = await _context.Users.CountAsync(u => u.AppRole.AppRoleName == "User");
                var totalPersonnel = await _context.Users.CountAsync(u => u.AppRole.AppRoleName == "Personnel");

                var recentComplaints = await _context.Complaints
                    .Include(c => c.Department)
                    .OrderByDescending(c => c.CreatedAt)
                    .Take(5)
                    .Select(c => new RecentComplaintViewModel
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Status = c.Status,
                        CreatedAt = c.CreatedAt,
                        DepartmentName = c.Department.Name
                    })
                    .ToListAsync();

                var dashboardViewModel = new DashboardViewModel
                {
                    TotalComplaints = totalComplaints,
                    PendingComplaints = pendingComplaints,
                    CompletedComplaints = completedComplaints,
                    TotalUsers = totalUsers,
                    TotalPersonnel = totalPersonnel,
                    MonthlyComplaints = new List<ChartData>(),
                    RecentComplaints = recentComplaints
                };

                return View("Dashboard", dashboardViewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Dashboard verileri yüklenirken bir hata oluştu: " + ex.Message;
                return View("Dashboard", new DashboardViewModel());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Complaints()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var complaints = await _context.Complaints
                    .Include(c => c.Department)
                    .Include(c => c.User)
                    .Include(c => c.Assignments)
                        .ThenInclude(a => a.User)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToListAsync();

                var viewModel = complaints.Select(c => new AdminComplaintViewModel
                {
                    Id = c.Id,
                    Title = c.Title ?? "Başlık Yok",
                    Description = c.Description ?? "Açıklama Yok",
                    CreatedAt = c.CreatedAt,
                    Status = c.Status ?? "Durum Belirsiz",
                    DepartmentName = c.Department?.Name ?? "Müdürlük Belirsiz",
                    UserFullName = c.User?.FullName ?? "Kullanıcı Belirsiz",
                    AssignedPersonnel = c.Assignments?.FirstOrDefault()?.User?.FullName ?? "Atanmamış",
                    LastUpdate = c.Assignments?.OrderByDescending(a => a.AssignedAt).FirstOrDefault()?.AssignedAt
                }).ToList();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Talepler yüklenirken bir hata oluştu: " + ex.Message;
                return View(new List<AdminComplaintViewModel>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Assignments()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                // Get all complaints from database
                var complaints = await _context.Complaints
                    .Include(c => c.Department)
                    .Include(c => c.User)
                    .Include(c => c.Assignments)
                        .ThenInclude(a => a.User)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToListAsync();

                // Get all personnel from database
                var personnel = await _context.Users
                    .Include(u => u.Department)
                    .Include(u => u.AppRole)
                    .Where(u => u.AppRole.AppRoleName == "Personnel")
                    .Select(u => new AdminPersonnelViewModel
                    {
                        Id = u.Id,
                        FullName = u.FullName,
                        Department = u.Department != null ? u.Department.Name : "Bilinmeyen Müdürlük"
                    })
                    .ToListAsync();

                var adminAssignments = new AdminAssignmentViewModel
                {
                    Complaints = complaints.Select(c => new AdminComplaintViewModel
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Description = c.Description,
                        Status = c.Status,
                        CreatedAt = c.CreatedAt,
                        DepartmentName = c.Department?.Name ?? "Bilinmeyen Müdürlük",
                        UserFullName = c.User?.FullName ?? "Bilinmeyen Kullanıcı",
                        AssignedPersonnel = c.Assignments != null && c.Assignments.Any() ? c.Assignments.First().User?.FullName ?? "Atanmamış" : "Atanmamış"
                    }).ToList(),
                    Personnel = personnel
                };

                return View(adminAssignments);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Atama verileri yüklenirken bir hata oluştu: " + ex.Message;
                return View(new AdminAssignmentViewModel());
            }
        }

        [HttpPost]
        public async Task<IActionResult> AssignPersonnel(int complaintId, int userId)
        {
            try
            {
                var complaint = await _context.Complaints
                    .Include(c => c.Assignments)
                    .FirstOrDefaultAsync(c => c.Id == complaintId);

                var personnel = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (complaint == null || personnel == null)
                {
                    TempData["Error"] = "Talep veya personel bulunamadı!";
                    return RedirectToAction("Complaints");
                }

                // Create new assignment
                var assignment = new Assignment
                {
                    ComplaintId = complaint.Id,
                    UserId = personnel.Id,
                    AssignedAt = DateTime.Now,
                    Progress = "Atandı"
                };

                _context.Assignments.Add(assignment);

                // Update complaint status
                complaint.Status = "Personel Atandı";

                await _context.SaveChangesAsync();

                TempData["Success"] = $"Personel '{personnel.FullName}' başarıyla atandı!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Personel atanırken bir hata oluştu: " + ex.Message;
            }

            return RedirectToAction("Complaints");
        }

        [HttpPost]
        public async Task<IActionResult> ApproveProgress(int assignmentId)
        {
            try
            {
                // Get assignment from database
                var assignment = await _context.Assignments
                    .Include(a => a.Complaint)
                    .FirstOrDefaultAsync(a => a.Id == assignmentId);

                if (assignment == null)
                {
                    TempData["Error"] = "Atama bulunamadı!";
                    return RedirectToAction("Complaints");
                }

                // Update assignment status
                assignment.Progress = "Tamamlandı";

                // Update complaint status
                assignment.Complaint.Status = "İşlem Tamamlandı";

                await _context.SaveChangesAsync();

                TempData["Success"] = "İşlem başarıyla tamamlandı!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "İşlem onaylanırken bir hata oluştu: " + ex.Message;
            }

            return RedirectToAction("Complaints");
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                // Get all users from database with complaint counts
                var users = await _context.Users
                    .Include(u => u.Department)
                    .Include(u => u.AppRole)
                    .Include(u => u.Complaints)
                    .Include(u => u.Assignments)
                    .OrderBy(u => u.FullName)
                    .Select(u => new AdminUserViewModel
                    {
                        Id = u.Id,
                        FullName = u.FullName,
                        Username = u.Username,
                        Role = u.AppRole.AppRoleName,
                        DepartmentName = u.Department != null ? u.Department.Name : "Atanmamış",
                        TotalComplaints = u.Complaints != null ? u.Complaints.Count : 0,
                        ActiveAssignments = u.Assignments != null ? u.Assignments.Count(a => a.Progress != "Tamamlandı") : 0
                    })
                    .ToListAsync();

                return View(users);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Kullanıcılar yüklenirken bir hata oluştu: " + ex.Message;
                return View(new List<AdminUserViewModel>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> UserDetails(int id)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Department)
                    .Include(u => u.AppRole)
                    .Include(u => u.Complaints)
                    .Include(u => u.Assignments)
                        .ThenInclude(a => a.Complaint)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                {
                    return Content("<div class='alert alert-danger'>Kullanıcı bulunamadı!</div>");
                }

                var userDetails = new AdminUserViewModel
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Username = user.Username,
                    Role = user.AppRole.AppRoleName,
                    DepartmentName = user.Department != null ? user.Department.Name : "Atanmamış",
                    TotalComplaints = user.Complaints != null ? user.Complaints.Count : 0,
                    ActiveAssignments = user.Assignments != null ? user.Assignments.Count(a => a.Progress != "Tamamlandı") : 0
                };

                return PartialView("_UserDetails", userDetails);
            }
            catch (Exception ex)
            {
                return Content($"<div class='alert alert-danger'>Hata: {ex.Message}</div>");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            try
            {
                var departments = await _context.Departments
                    .Select(d => new { id = d.Id, name = d.Name })
                    .ToListAsync();

                return Json(departments);
            }
            catch (Exception ex)
            {
                return Json(new List<object>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] CreateUserViewModel model)
        {
            try
            {
                // Check if username already exists
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
                if (existingUser != null)
                {
                    return BadRequest("Bu kullanıcı adı zaten kullanılıyor.");
                }

                // Get role ID
                var role = await _context.AppRoles.FirstOrDefaultAsync(r => r.AppRoleName == model.Role);
                if (role == null)
                {
                    return BadRequest("Geçersiz rol.");
                }

                // Create new user
                var user = new User
                {
                    FullName = model.FullName,
                    Username = model.Username,
                    PasswordHash = model.Password, // In production, hash the password
                    AppRoleId = role.AppRoleId,
                    DepartmentId = model.DepartmentId > 0 ? model.DepartmentId : null
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"Kullanıcı '{model.FullName}' başarıyla eklendi!";
                return Json(new { success = true, message = "Kullanıcı başarıyla eklendi." });
            }
            catch (Exception ex)
            {
                return BadRequest($"Kullanıcı eklenirken bir hata oluştu: {ex.Message}");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.AppRole)
                    .FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    return NotFound("Kullanıcı bulunamadı.");
                }

                if (user.AppRole.AppRoleName == "Admin")
                {
                    return BadRequest("Admin kullanıcıları silinemez.");
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"Kullanıcı '{user.FullName}' başarıyla silindi!";
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Kullanıcı silinirken bir hata oluştu: {ex.Message}");
            }
        }
    }
} 