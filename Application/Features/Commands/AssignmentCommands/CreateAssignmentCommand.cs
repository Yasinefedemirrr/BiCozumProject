using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Commands.AssignmentCommands
{
    public class CreateAssignmentCommand : IRequest<int>
    {
        public int ComplaintId { get; set; }
        public int UserId { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.Now;
        public string Progress { get; set; } = "Beklemede";
    }
}
