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

                var dashboard = new DashboardViewModel
                {
                    TotalComplaints = totalComplaints,
                    PendingComplaints = pendingComplaints,
                    CompletedComplaints = completedComplaints,
                    TotalUsers = totalUsers,
                    TotalPersonnel = totalPersonnel,
                    MonthlyComplaints = new List<ChartData>
                    {
                        new ChartData { Month = "Ocak", Count = 5 },
                        new ChartData { Month = "Şubat", Count = 8 },
                        new ChartData { Month = "Mart", Count = 12 },
                        new ChartData { Month = "Nisan", Count = 15 },
                        new ChartData { Month = "Mayıs", Count = 10 },
                        new ChartData { Month = "Haziran", Count = 7 }
                    },
                    RecentComplaints = recentComplaints
                };

                return View(dashboard);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Dashboard verileri yüklenirken bir hata oluştu: " + ex.Message;
                return View(new DashboardViewModel());
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
                // Get all complaints from database
                var complaints = await _context.Complaints
                    .Include(c => c.Department)
                    .Include(c => c.User)
                    .Include(c => c.Assignments)
                        .ThenInclude(a => a.User)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToListAsync();

                var adminComplaints = complaints.Select(c => new AdminComplaintViewModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Status = c.Status,
                    CreatedAt = c.CreatedAt,
                    DepartmentName = c.Department?.Name ?? "Bilinmeyen Müdürlük",
                    UserFullName = c.User?.FullName ?? "Bilinmeyen Kullanıcı",
                    AssignedPersonnel = c.Assignments != null && c.Assignments.Any() ? c.Assignments.First().User?.FullName ?? "Atanmamış" : "Atanmamış"
                }).ToList();

                return View(adminComplaints);
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
                // Get complaint from database
                var complaint = await _context.Complaints
                    .Include(c => c.Department)
                    .FirstOrDefaultAsync(c => c.Id == complaintId);

                if (complaint == null)
                {
                    TempData["Error"] = "Talep bulunamadı!";
                    return RedirectToAction("Complaints");
                }

                // Get personnel from database
                var personnel = await _context.Users
                    .Include(u => u.Department)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (personnel == null)
                {
                    TempData["Error"] = "Personel bulunamadı!";
                    return RedirectToAction("Complaints");
                }

                // Check if assignment already exists
                var existingAssignment = await _context.Assignments
                    .FirstOrDefaultAsync(a => a.ComplaintId == complaintId);

                if (existingAssignment != null)
                {
                    // Update existing assignment
                    existingAssignment.UserId = userId;
                    existingAssignment.AssignedAt = DateTime.Now;
                    existingAssignment.Progress = "Atandı";
                }
                else
                {
                    // Create new assignment
                    var assignment = new Assignment
                    {
                        ComplaintId = complaintId,
                        UserId = userId,
                        AssignedAt = DateTime.Now,
                        Progress = "Atandı"
                    };

                    _context.Assignments.Add(assignment);
                }

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
                // Get all users from database
                var users = await _context.Users
                    .Include(u => u.Department)
                    .Include(u => u.AppRole)
                    .OrderBy(u => u.FullName)
                    .Select(u => new AdminUserViewModel
                    {
                        Id = u.Id,
                        FullName = u.FullName,
                        Username = u.Username,
                        Role = u.AppRole.AppRoleName,
                        DepartmentName = u.Department != null ? u.Department.Name : "Atanmamış"
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
    }
} 