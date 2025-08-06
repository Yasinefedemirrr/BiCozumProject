using Application.Features.Queries.AssignmentQeries;
using Application.Features.Queries.ComplaintQueries;
using Application.Features.Results.AssignmentResults;
using Application.Features.Results.ComplaintResults;
using Application.interfaces;
using MediatR;


namespace Application.Features.Handlers.ComplaintHandlers
{
    public class GetAllComplaintsHandler : IRequestHandler<GetAllComplaintsQuery, List<GetAllComplaintsResult>>
    {
        private readonly IComplaintRepository _complaintRepository;

        public GetAllComplaintsHandler(IComplaintRepository complaintRepository)
        {
            _complaintRepository = complaintRepository;
        }

        public async Task<List<GetAllComplaintsResult>> Handle(GetAllComplaintsQuery request, CancellationToken cancellationToken)
        {
            var complaints = await _complaintRepository.GetAllWithIncludesAsync();

            return complaints.Select(c => new GetAllComplaintsResult
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                DepartmentName = c.Department?.Name ?? "",
                UserFullName = c.User != null ? $"{c.User.FullName}" : "",
                Status = c.Status,
                CreatedAt = c.CreatedAt
            }).ToList();
        }
    }
}