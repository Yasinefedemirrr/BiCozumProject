using Application.Features.Commands.ComplaintCommands;
using Application.interfaces;
using Domain.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Handlers.ComplaintHandlers
{
    public class CreateComplaintHandler : IRequestHandler<CreateComplaintCommand, int>
    {
        private readonly IComplaintRepository _complaintRepository;

        public CreateComplaintHandler(IComplaintRepository complaintRepository)
        {
            _complaintRepository = complaintRepository;
        }

        public async Task<int> Handle(CreateComplaintCommand request, CancellationToken cancellationToken)
        {
            var complaint = new Complaint
            {
                Title = request.Title,
                Description = request.Description,
                DepartmentId = request.DepartmentId,
                UserId = request.UserId,
                Status = "Beklemede"
            };

            await _complaintRepository.AddAsync(complaint);
            return complaint.Id;
        }
    }
}
