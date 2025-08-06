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
    public class GetAllAssignmentsHandler : IRequestHandler<GetAllAssignmentsQuery, List<GetAllAssignmentsResult>>
    {
        private readonly IAssignmentRepository _assignmentRepository;

        public GetAllAssignmentsHandler(IAssignmentRepository assignmentRepository)
        {
            _assignmentRepository = assignmentRepository;
        }

        public async Task<List<GetAllAssignmentsResult>> Handle(GetAllAssignmentsQuery request, CancellationToken cancellationToken)
        {
            var assignments = await _assignmentRepository.GetAllWithIncludesAsync();

            return assignments.Select(a => new GetAllAssignmentsResult
            {
                Id = a.Id,
                AssignedAt = a.AssignedAt,
                Progress = a.Progress,
                ComplaintId = a.ComplaintId,
                ComplaintTitle = a.Complaint?.Title ?? "",
                UserId = a.UserId,
                UserFullName = a.User != null ? a.User.FullName : ""
            }).ToList();
        }
    }
}
