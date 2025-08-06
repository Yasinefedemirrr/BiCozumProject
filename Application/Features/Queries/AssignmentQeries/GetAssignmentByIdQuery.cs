using Application.Features.Results.AssignmentResults;
using Application.Features.Results.ComplaintResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Queries.AssignmentQeries
{
    public class GetAssignmentByIdQuery : IRequest<GetAssignmentByIdResult>
    {
        public int Id { get; set; }
        public GetAssignmentByIdQuery(int id)
        {
            Id = id;
        }
    }
}
