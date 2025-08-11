using Microsoft.AspNetCore.Mvc;
using WebUI.Models;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using Domain.Entity;

namespace WebUI.Controllers
{
    public class ComplaintController : Controller
    {
        private readonly BiCozumContext _context;

        public ComplaintController(BiCozumContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                // Get departments from database
                var departments = await _context.Departments
                    .OrderBy(d => d.Name)
                    .ToListAsync();

                var viewModel = new ComplaintCreateViewModel
                {
                    Departments = departments.Select(d => new DepartmentViewModel
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Description = d.Name
                    }).ToList()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Departman bilgileri yüklenirken hata oluştu: " + ex.Message;
                return View(new ComplaintCreateViewModel());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(ComplaintCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Reload departments for the view
                var departments = await _context.Departments
                    .OrderBy(d => d.Name)
                    .ToListAsync();

                model.Departments = departments.Select(d => new DepartmentViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Name
                }).ToList();

                return View(model);
            }

            try
            {
                // Get current user from session
                var currentUserId = HttpContext.Session.GetString("CurrentUserId");
                if (string.IsNullOrEmpty(currentUserId))
                {
                    ModelState.AddModelError("", "Kullanıcı bilgisi bulunamadı!");
                    return View(model);
                }

                // Validate department exists
                var department = await _context.Departments.FindAsync(model.DepartmentId);
                if (department == null)
                {
                    ModelState.AddModelError("DepartmentId", "Seçilen müdürlük bulunamadı!");
                    return View(model);
                }

                // Create new complaint
                var complaint = new Complaint
                {
                    Title = model.Title,
                    Description = model.Description,
                    Status = "Talep Alındı",
                    CreatedAt = DateTime.Now,
                    DepartmentId = model.DepartmentId,
                    UserId = int.Parse(currentUserId)
                };

                // Add to database
                _context.Complaints.Add(complaint);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Talebiniz başarıyla alınmıştır!";
                return RedirectToAction("MyComplaints");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Talep oluşturulurken bir hata oluştu: " + ex.Message);
                
                // Reload departments for the view
                var departments = await _context.Departments
                    .OrderBy(d => d.Name)
                    .ToListAsync();

                model.Departments = departments.Select(d => new DepartmentViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Name
                }).ToList();

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> MyComplaints()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                // Get current user from session
                var currentUserId = HttpContext.Session.GetString("CurrentUserId");
                if (string.IsNullOrEmpty(currentUserId))
                {
                    TempData["Error"] = "Kullanıcı bilgisi bulunamadı!";
                    return View(new List<ComplaintViewModel>());
                }

                // Get complaints from database with all related data
                var complaints = await _context.Complaints
                    .Include(c => c.Department)
                    .Include(c => c.Assignments)
                        .ThenInclude(a => a.User)
                    .Where(c => c.UserId == int.Parse(currentUserId))
                    .OrderByDescending(c => c.CreatedAt)
                    .ToListAsync();

                var complaintViewModels = complaints.Select(c => new ComplaintViewModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Status = c.Status,
                    CreatedAt = c.CreatedAt,
                    DepartmentName = c.Department?.Name ?? "Bilinmeyen Müdürlük",
                    AssignedPersonnel = c.Assignments?.FirstOrDefault()?.User?.FullName
                }).ToList();

                return View(complaintViewModels);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Talepler yüklenirken bir hata oluştu: " + ex.Message;
                return View(new List<ComplaintViewModel>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                // Get complaint from database with all related data
                var complaint = await _context.Complaints
                    .Include(c => c.Department)
                    .Include(c => c.Assignments)
                        .ThenInclude(a => a.User)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (complaint == null)
                {
                    TempData["Error"] = "Talep bulunamadı!";
                    return RedirectToAction("MyComplaints");
                }

                var detailViewModel = new ComplaintDetailViewModel
                {
                    Id = complaint.Id,
                    Title = complaint.Title,
                    Description = complaint.Description,
                    Status = complaint.Status,
                    CreatedAt = complaint.CreatedAt,
                    DepartmentName = complaint.Department?.Name ?? "Bilinmeyen Müdürlük",
                    AssignedPersonnel = complaint.Assignments?.FirstOrDefault()?.User?.FullName
                };

                return View(detailViewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Talep detayları yüklenirken bir hata oluştu: " + ex.Message;
                return RedirectToAction("MyComplaints");
            }
        }

        [HttpGet]
        public async Task<IActionResult> OldComplaints()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                // Get current user from session
                var currentUserId = HttpContext.Session.GetString("CurrentUserId");
                if (string.IsNullOrEmpty(currentUserId))
                {
                    TempData["Error"] = "Kullanıcı bilgisi bulunamadı!";
                    return View(new List<ComplaintViewModel>());
                }

                // Get old complaints (completed or older than 30 days) from database
                var thirtyDaysAgo = DateTime.Now.AddDays(-30);
                var complaints = await _context.Complaints
                    .Include(c => c.Department)
                    .Include(c => c.Assignments)
                        .ThenInclude(a => a.User)
                    .Where(c => c.UserId == int.Parse(currentUserId) && 
                               (c.Status == "İşlem Tamamlandı" || c.CreatedAt < thirtyDaysAgo))
                    .OrderByDescending(c => c.CreatedAt)
                    .ToListAsync();

                var complaintViewModels = complaints.Select(c => new ComplaintViewModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Status = c.Status,
                    CreatedAt = c.CreatedAt,
                    DepartmentName = c.Department?.Name ?? "Bilinmeyen Müdürlük",
                    AssignedPersonnel = c.Assignments?.FirstOrDefault()?.User?.FullName
                }).ToList();

                return View(complaintViewModels);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Eski talepler yüklenirken bir hata oluştu: " + ex.Message;
                return View(new List<ComplaintViewModel>());
            }
        }
    }


} 