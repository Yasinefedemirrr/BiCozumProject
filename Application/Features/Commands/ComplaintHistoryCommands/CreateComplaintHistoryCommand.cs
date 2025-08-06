using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Commands.ComplaintHistoryCommands
{
    public class CreateComplaintHistoryCommand : IRequest<int>
    {
        public int ComplaintId { get; set; }
        public string Status { get; set; }
        public string? Note { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }

}
