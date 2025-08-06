using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Results.AssignmentResults
{
    public class GetAssignmentByIdResult
    {
        public int Id { get; set; }
        public DateTime AssignedAt { get; set; }
        public string Progress { get; set; }
        public int ComplaintId { get; set; }
        public string ComplaintTitle { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; }
    }
}
