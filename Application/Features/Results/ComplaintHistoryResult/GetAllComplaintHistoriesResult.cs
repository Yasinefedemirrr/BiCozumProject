using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Results.ComplaintHistoryResult
{
    public class GetAllComplaintHistoriesResult
    {
        public int Id { get; set; }
        public int ComplaintId { get; set; }
        public string ComplaintTitle { get; set; }
        public string Status { get; set; }
        public string? Note { get; set; }
        public DateTime Date { get; set; }
    }
}
