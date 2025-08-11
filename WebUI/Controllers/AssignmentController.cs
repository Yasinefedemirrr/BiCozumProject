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
    public class AssignmentController : Controller
    {
        private readonly BiCozumContext _context;

        public AssignmentController(BiCozumContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> MyAssignments()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                // Get current personnel ID from session
                var currentUserId = HttpContext.Session.GetString("CurrentUserId");
                if (string.IsNullOrEmpty(currentUserId))
                {
                    TempData["Error"] = "Kullanıcı bilgisi bulunamadı!";
                    return View(new List<AssignmentViewModel>());
                }

                // Get assignments for current personnel from database
                var assignments = await _context.Assignments
                    .Include(a => a.Complaint)
                        .ThenInclude(c => c.Department)
                    .Include(a => a.Complaint)
                        .ThenInclude(c => c.User)
                    .Where(a => a.UserId == int.Parse(currentUserId))
                    .OrderByDescending(a => a.AssignedAt)
                    .ToListAsync();

                var assignmentViewModels = assignments.Select(a => new AssignmentViewModel
                {
                    Id = a.Id,
                    ComplaintId = a.ComplaintId,
                    ComplaintTitle = a.Complaint.Title,
                    ComplaintDescription = a.Complaint.Description,
                    ComplaintCreatedAt = a.Complaint.CreatedAt,
                    DepartmentName = a.Complaint.Department?.Name ?? "Bilinmeyen Müdürlük",
                    Progress = a.Progress,
                    AssignedAt = a.AssignedAt,
                    Status = a.Complaint.Status
                }).ToList();

                return View(assignmentViewModels);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Atanan talepler yüklenirken bir hata oluştu: " + ex.Message;
                return View(new List<AssignmentViewModel>());
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
                // Get assignment from database
                var assignment = await _context.Assignments
                    .Include(a => a.Complaint)
                        .ThenInclude(c => c.Department)
                    .Include(a => a.User)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (assignment == null)
                {
                    TempData["Error"] = "Atama bulunamadı!";
                    return RedirectToAction("MyAssignments");
                }

                var detailViewModel = new AssignmentDetailViewModel
                {
                    Id = assignment.Id,
                    ComplaintId = assignment.ComplaintId,
                    ComplaintTitle = assignment.Complaint.Title,
                    ComplaintDescription = assignment.Complaint.Description,
                    DepartmentName = assignment.Complaint.Department?.Name ?? "Bilinmeyen Müdürlük",
                    Progress = assignment.Progress,
                    AssignedAt = assignment.AssignedAt,
                    UserFullName = assignment.User.FullName
                };

                return View(detailViewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Atama detayları yüklenirken bir hata oluştu: " + ex.Message;
                return RedirectToAction("MyAssignments");
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProgress(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                // Get assignment from database
                var assignment = await _context.Assignments
                    .Include(a => a.Complaint)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (assignment == null)
                {
                    TempData["Error"] = "Atama bulunamadı!";
                    return RedirectToAction("MyAssignments");
                }

                var updateModel = new AssignmentUpdateViewModel
                {
                    AssignmentId = assignment.Id,
                    ComplaintId = assignment.ComplaintId,
                    CurrentProgress = assignment.Progress,
                    Notes = ""
                };

                return View(updateModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Atama bilgileri yüklenirken bir hata oluştu: " + ex.Message;
                return RedirectToAction("MyAssignments");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProgress(AssignmentUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Get assignment from database
                var assignment = await _context.Assignments
                    .Include(a => a.Complaint)
                    .FirstOrDefaultAsync(a => a.Id == model.AssignmentId);

                if (assignment == null)
                {
                    ModelState.AddModelError("", "Atama bulunamadı!");
                    return View(model);
                }

                // Update assignment progress
                assignment.Progress = model.NewProgress;

                // Update complaint status based on progress
                switch (model.NewProgress)
                {
                    case "İnceleniyor":
                        assignment.Complaint.Status = "İnceleniyor";
                        break;
                    case "Test Ediliyor":
                        assignment.Complaint.Status = "Test Ediliyor";
                        break;
                    case "Tamamlandı":
                        assignment.Complaint.Status = "İşlem Tamamlandı";
                        break;
                }

                await _context.SaveChangesAsync();

                TempData["Success"] = "İlerleme durumu başarıyla güncellendi!";
                return RedirectToAction("MyAssignments");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "İlerleme durumu güncellenirken bir hata oluştu: " + ex.Message);
                return View(model);
            }
        }
    }
} 