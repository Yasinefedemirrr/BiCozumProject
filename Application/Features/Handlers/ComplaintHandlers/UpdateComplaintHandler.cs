using Application.Features.Commands.ComplaintCommands;
using Application.interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Handlers.ComplaintHandlers
{
    public class UpdateComplaintHandler : IRequestHandler<UpdateComplaintCommand, Unit>
    {
        private readonly IComplaintRepository _complaintRepository;

        public UpdateComplaintHandler(IComplaintRepository complaintRepository)
        {
            _complaintRepository = complaintRepository;
        }

        public async Task<Unit> Handle(UpdateComplaintCommand request, CancellationToken cancellationToken)
        {
            var complaint = await _complaintRepository.GetByIdAsync(request.Id);
            if (complaint != null)
            {
                complaint.Title = request.Title;
                complaint.Description = request.Description;
                complaint.DepartmentId = request.DepartmentId;
                complaint.UserId = request.UserId;
                complaint.Status = request.Status;

                await _complaintRepository.UpdateAsync(complaint);
            }

            return Unit.Value;
        }
    }
}
