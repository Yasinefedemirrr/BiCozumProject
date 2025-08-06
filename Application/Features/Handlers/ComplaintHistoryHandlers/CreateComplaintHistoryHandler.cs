using Application.Features.Commands.ComplaintHistoryCommands;
using Application.interfaces;
using Domain.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Handlers.ComplaintHistoryHandlers
{
    public class CreateComplaintHistoryHandler : IRequestHandler<CreateComplaintHistoryCommand, int>
    {
        private readonly IComplaintHistoryRepository _complaintHistoryRepository;

        public CreateComplaintHistoryHandler(IComplaintHistoryRepository complaintHistoryRepository)
        {
            _complaintHistoryRepository = complaintHistoryRepository;
        }

        public async Task<int> Handle(CreateComplaintHistoryCommand request, CancellationToken cancellationToken)
        {
            var history = new ComplaintHistory
            {
                ComplaintId = request.ComplaintId,
                Status = request.Status,
                Note = request.Note,
                Date = request.Date
            };

            await _complaintHistoryRepository.AddAsync(history);
            return history.Id;
        }
    }

}
