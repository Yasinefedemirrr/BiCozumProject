using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Commands.AssignmentCommands
{
    public class UpdateAssignmentCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Progress { get; set; }
    }
}
