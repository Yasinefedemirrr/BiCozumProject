using Application.Features.Queries.AssignmentQeries;
using Application.Features.Results.AssignmentResults;
using Application.interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Handlers.AssignmentHandlers
{
    public class GetAssignmentByIdHandler : IRequestHandler<GetAssignmentByIdQuery, GetAssignmentByIdResult>
    {
        private readonly IAssignmentRepository _assignmentRepository;

        public GetAssignmentByIdHandler(IAssignmentRepository assignmentRepository)
        {
            _assignmentRepository = assignmentRepository;
        }

        public async Task<GetAssignmentByIdResult> Handle(GetAssignmentByIdQuery request, CancellationToken cancellationToken)
        {
            var assignment = await _assignmentRepository.GetByIdWithIncludesAsync(request.Id);

            if (assignment == null) return null;

            return new GetAssignmentByIdResult
            {
                Id = assignment.Id,
                AssignedAt = assignment.AssignedAt,
                Progress = assignment.Progress,
                ComplaintId = assignment.ComplaintId,
                ComplaintTitle = assignment.Complaint?.Title ?? "",
                UserId = assignment.UserId,
                UserFullName = assignment.User != null ? assignment.User.FullName : ""
            };
        }
    }
}
