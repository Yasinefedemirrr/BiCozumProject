using Application.Features.Queries.AssignmentQeries;
using Application.Features.Queries.ComplaintQueries;
using Application.Features.Results.AssignmentResults;
using Application.Features.Results.ComplaintResults;
using Application.interfaces;
using MediatR;


namespace Application.Features.Handlers.ComplaintHandlers
{
    public class GetComplaintByIdHandler : IRequestHandler<GetComplaintByIdQuery, GetComplaintByIdResult>
    {
        private readonly IComplaintRepository _complaintRepository;

        public GetComplaintByIdHandler(IComplaintRepository complaintRepository)
        {
            _complaintRepository = complaintRepository;
        }

        public async Task<GetComplaintByIdResult> Handle(GetComplaintByIdQuery request, CancellationToken cancellationToken)
        {
            var complaint = await _complaintRepository.GetByIdWithIncludesAsync(request.Id);

            if (complaint == null) return null;

            return new GetComplaintByIdResult
            {
                Id = complaint.Id,
                Title = complaint.Title,
                Description = complaint.Description,
                DepartmentName = complaint.Department?.Name ?? "",
                UserFullName = complaint.User != null ? $"{complaint.User.FullName}" : "",
                Status = complaint.Status,
                CreatedAt = complaint.CreatedAt
            };
        }
    }
}