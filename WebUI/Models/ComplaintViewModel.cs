using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class ComplaintCreateViewModel
    {
        [Required(ErrorMessage = "Başlık zorunludur")]
        [Display(Name = "Talep Başlığı")]
        [StringLength(100, ErrorMessage = "Başlık en fazla 100 karakter olabilir")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur")]
        [Display(Name = "Talep Açıklaması")]
        [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Müdürlük seçimi zorunludur")]
        [Display(Name = "İlgili Müdürlük")]
        public int DepartmentId { get; set; }

        public List<DepartmentViewModel> Departments { get; set; } = new List<DepartmentViewModel>();
    }

    public class ComplaintViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public string DepartmentName { get; set; }
        public string AssignedPersonnel { get; set; }
        public DateTime? LastUpdate { get; set; }
    }

    public class ComplaintDetailViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public string DepartmentName { get; set; }
        public string AssignedPersonnel { get; set; }
        public DateTime? LastUpdate { get; set; }
        public List<ComplaintHistoryViewModel> History { get; set; } = new List<ComplaintHistoryViewModel>();
    }

    public class ComplaintHistoryViewModel
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class DepartmentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
} 