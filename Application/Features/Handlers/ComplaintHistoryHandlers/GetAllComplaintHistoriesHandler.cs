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
    public class GetAllComplaintHistoriesHandler : IRequestHandler<GetAllComplaintHistoriesQuery, List<GetAllComplaintHistoriesResult>>
    {
        private readonly IComplaintHistoryRepository _complaintHistoryRepository;

        public GetAllComplaintHistoriesHandler(IComplaintHistoryRepository complaintHistoryRepository)
        {
            _complaintHistoryRepository = complaintHistoryRepository;
        }

        public async Task<List<GetAllComplaintHistoriesResult>> Handle(GetAllComplaintHistoriesQuery request, CancellationToken cancellationToken)
        {
            var histories = await _complaintHistoryRepository.GetAllWithIncludesAsync();

            return histories.Select(h => new GetAllComplaintHistoriesResult
            {
                Id = h.Id,
                ComplaintId = h.ComplaintId,
                ComplaintTitle = h.Complaint?.Title ?? "",
                Status = h.Status,
                Note = h.Note,
                Date = h.Date
            }).ToList();
        }
    }
}
