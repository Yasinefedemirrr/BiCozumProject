using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class DashboardViewModel
    {
        public int TotalComplaints { get; set; }
        public int PendingComplaints { get; set; }
        public int CompletedComplaints { get; set; }
        public int TotalUsers { get; set; }
        public int TotalPersonnel { get; set; }
        public List<ChartData> MonthlyComplaints { get; set; } = new List<ChartData>();
        public List<RecentComplaintViewModel> RecentComplaints { get; set; } = new List<RecentComplaintViewModel>();
    }

    public class ChartData
    {
        public string Month { get; set; }
        public int Count { get; set; }
    }

    public class RecentComplaintViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string DepartmentName { get; set; }
    }

    public class AdminComplaintViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public string DepartmentName { get; set; }
        public string UserFullName { get; set; }
        public string AssignedPersonnel { get; set; }
        public DateTime? LastUpdate { get; set; }
    }

    public class AdminAssignmentViewModel
    {
        public List<AdminComplaintViewModel> Complaints { get; set; } = new List<AdminComplaintViewModel>();
        public List<AdminPersonnelViewModel> Personnel { get; set; } = new List<AdminPersonnelViewModel>();
    }

    public class AdminPersonnelViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
    }

    public class UnassignedComplaintViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string DepartmentName { get; set; }
        public string UserFullName { get; set; }
    }

    public class AssignedComplaintViewModel
    {
        public int Id { get; set; }
        public int ComplaintId { get; set; }
        public string ComplaintTitle { get; set; }
        public string DepartmentName { get; set; }
        public string AssignedPersonnel { get; set; }
        public string Progress { get; set; }
        public DateTime AssignedAt { get; set; }
        public DateTime? LastUpdate { get; set; }
        public bool NeedsApproval { get; set; }
    }

    public class PersonnelViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string DepartmentName { get; set; }
        public int ActiveAssignments { get; set; }
    }

    public class AssignPersonnelViewModel
    {
        [Required(ErrorMessage = "Talep seçimi zorunludur")]
        [Display(Name = "Talep")]
        public int ComplaintId { get; set; }

        [Required(ErrorMessage = "Personel seçimi zorunludur")]
        [Display(Name = "Personel")]
        public int UserId { get; set; }

        public List<UnassignedComplaintViewModel> AvailableComplaints { get; set; } = new List<UnassignedComplaintViewModel>();
        public List<PersonnelViewModel> AvailablePersonnel { get; set; } = new List<PersonnelViewModel>();
    }

    public class AdminUserViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string DepartmentName { get; set; }
        public int TotalComplaints { get; set; }
        public int ActiveAssignments { get; set; }
    }

    public class CreateUserViewModel
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int? DepartmentId { get; set; }
    }
} 