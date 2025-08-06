using Application.Features.Queries.ComplaintHistoryQueries;
using Application.Features.Results.ComplaintHistoryResult;
using Application.interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Handlers.ComplaintHistoryHandlers
{
    public class GetComplaintHistoryByIdHandler : IRequestHandler<GetComplaintHistoryByIdQuery, GetComplaintHistoryByIdResult>
    {
        private readonly IComplaintHistoryRepository _complaintHistoryRepository;

        public GetComplaintHistoryByIdHandler(IComplaintHistoryRepository complaintHistoryRepository)
        {
            _complaintHistoryRepository = complaintHistoryRepository;
        }

        public async Task<GetComplaintHistoryByIdResult> Handle(GetComplaintHistoryByIdQuery request, CancellationToken cancellationToken)
        {
            var history = await _complaintHistoryRepository.GetByIdWithIncludesAsync(request.Id);

            if (history == null) return null;

            return new GetComplaintHistoryByIdResult
            {
                Id = history.Id,
                ComplaintId = history.ComplaintId,
                ComplaintTitle = history.Complaint?.Title ?? "",
                Status = history.Status,
                Note = history.Note,
                Date = history.Date
            };
        }
    }
}
