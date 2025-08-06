using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Commands.ComplaintCommands
{
    public class CreateComplaintCommand :IRequest<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int DepartmentId { get; set; }
        public int UserId { get; set; }
    }
}
