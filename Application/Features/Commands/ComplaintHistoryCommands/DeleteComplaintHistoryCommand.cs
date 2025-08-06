using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Commands.ComplaintHistoryCommands
{
    public class DeleteComplaintHistoryCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
