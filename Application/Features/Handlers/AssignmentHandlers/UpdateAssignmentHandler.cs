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
    public class UpdateAssignmentHandler : IRequestHandler<UpdateAssignmentCommand, Unit>
    {
        private readonly IAssignmentRepository _assignmentRepository;

        public UpdateAssignmentHandler(IAssignmentRepository assignmentRepository)
        {
            _assignmentRepository = assignmentRepository;
        }

        public async Task<Unit> Handle(UpdateAssignmentCommand request, CancellationToken cancellationToken)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(request.Id);
            if (assignment != null)
            {
                assignment.Progress = request.Progress;
                await _assignmentRepository.UpdateAsync(assignment);
            }
            return Unit.Value;
        }
    }
}
