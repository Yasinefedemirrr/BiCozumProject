using Application.Features.Commands.AssignmentCommands;
using Application.interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Handlers.AssignmentHandlers
{
    public class DeleteAssignmentHandler : IRequestHandler<DeleteAssignmentCommand, Unit>
    {
        private readonly IAssignmentRepository _assignmentRepository;

        public DeleteAssignmentHandler(IAssignmentRepository assignmentRepository)
        {
            _assignmentRepository = assignmentRepository;
        }

        public async Task<Unit> Handle(DeleteAssignmentCommand request, CancellationToken cancellationToken)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(request.Id);
            if (assignment != null)
            {
                await _assignmentRepository.DeleteAsync(assignment);
            }
            return Unit.Value;
        }
    }
}
