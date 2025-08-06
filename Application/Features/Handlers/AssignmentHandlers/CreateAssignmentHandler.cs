using Application.Features.Commands.AssignmentCommands;
using Application.interfaces;
using Domain.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Handlers.AssignmentHandlers
{
    public class CreateAssignmentHandler : IRequestHandler<CreateAssignmentCommand, int>
    {
        private readonly IAssignmentRepository _assignmentRepository;

        public CreateAssignmentHandler(IAssignmentRepository assignmentRepository)
        {
            _assignmentRepository = assignmentRepository;
        }

        public async Task<int> Handle(CreateAssignmentCommand request, CancellationToken cancellationToken)
        {
            var assignment = new Assignment
            {
                ComplaintId = request.ComplaintId,
                UserId = request.UserId,
                AssignedAt = request.AssignedAt,
                Progress = request.Progress
            };

            await _assignmentRepository.AddAsync(assignment);
            return assignment.Id;
        }
    }
}