using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Commands.AssignmentCommands
{
    public class DeleteAssignmentCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
