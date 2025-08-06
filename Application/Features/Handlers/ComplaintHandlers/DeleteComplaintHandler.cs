using Application.Features.Commands.ComplaintCommands;
using Application.interfaces;
using MediatR;


namespace Application.Features.Handlers.ComplaintHandlers
{
    public class DeleteComplaintHandler : IRequestHandler<DeleteComplaintCommand, Unit>
    {
        private readonly IComplaintRepository _complaintRepository;

        public DeleteComplaintHandler(IComplaintRepository complaintRepository)
        {
            _complaintRepository = complaintRepository;
        }

        public async Task<Unit> Handle(DeleteComplaintCommand request, CancellationToken cancellationToken)
        {
            var complaint = await _complaintRepository.GetByIdAsync(request.Id);
            if (complaint != null)
            {
                await _complaintRepository.DeleteAsync(complaint);
            }

            return Unit.Value;
        }
    }
}
