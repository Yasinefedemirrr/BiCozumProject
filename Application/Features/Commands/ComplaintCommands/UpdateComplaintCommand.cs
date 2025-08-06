using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Commands.ComplaintCommands
{
    public class UpdateComplaintCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int DepartmentId { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; } = null!;
    }
}
