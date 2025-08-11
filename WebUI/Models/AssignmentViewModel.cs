using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class AssignmentViewModel
    {
        public int Id { get; set; }
        public int ComplaintId { get; set; }
        public string ComplaintTitle { get; set; }
        public string ComplaintDescription { get; set; }
        public DateTime ComplaintCreatedAt { get; set; }
        public string DepartmentName { get; set; }
        public string Progress { get; set; }
        public DateTime AssignedAt { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string Status { get; set; }
    }

    public class AssignmentDetailViewModel
    {
        public int Id { get; set; }
        public int ComplaintId { get; set; }
        public string ComplaintTitle { get; set; }
        public string ComplaintDescription { get; set; }
        public DateTime ComplaintCreatedAt { get; set; }
        public string DepartmentName { get; set; }
        public string Progress { get; set; }
        public DateTime AssignedAt { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string Status { get; set; }
        public string UserFullName { get; set; }
        public List<AssignmentHistoryViewModel> History { get; set; } = new List<AssignmentHistoryViewModel>();
    }

    public class AssignmentUpdateViewModel
    {
        public int AssignmentId { get; set; }
        public int ComplaintId { get; set; }
        public string CurrentProgress { get; set; }

        [Required(ErrorMessage = "Yeni ilerleme durumu seçimi zorunludur")]
        [Display(Name = "Yeni İlerleme Durumu")]
        public string NewProgress { get; set; }

        [Display(Name = "Notlar")]
        [StringLength(500, ErrorMessage = "Notlar en fazla 500 karakter olabilir")]
        public string Notes { get; set; }

        public List<SelectListItem> ProgressOptions { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Atandı", Text = "Atandı" },
            new SelectListItem { Value = "İnceleniyor", Text = "İnceleniyor" },
            new SelectListItem { Value = "Test Ediliyor", Text = "Test Ediliyor" },
            new SelectListItem { Value = "Tamamlandı", Text = "Tamamlandı" },
            new SelectListItem { Value = "İptal", Text = "İptal" }
        };
    }

    public class AssignmentHistoryViewModel
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }

    public class SelectListItem
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
} 