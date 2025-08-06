using Application.Features.Commands.ComplaintHistoryCommands;
using Application.interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Handlers.ComplaintHistoryHandlers
{
    public class DeleteComplaintHistoryHandler : IRequestHandler<DeleteComplaintHistoryCommand, Unit>
    {
        private readonly IComplaintHistoryRepository _complaintHistoryRepository;

        public DeleteComplaintHistoryHandler(IComplaintHistoryRepository complaintHistoryRepository)
        {
            _complaintHistoryRepository = complaintHistoryRepository;
        }

        public async Task<Unit> Handle(DeleteComplaintHistoryCommand request, CancellationToken cancellationToken)
        {
            var history = await _complaintHistoryRepository.GetByIdAsync(request.Id);
            if (history != null)
            {
                await _complaintHistoryRepository.DeleteAsync(history);
            }
            return Unit.Value;
        }
    }

}
