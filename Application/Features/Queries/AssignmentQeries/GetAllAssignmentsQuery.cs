using Application.Features.Results.AssignmentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Queries.AssignmentQeries
{
    public class GetAllAssignmentsQuery : IRequest<List<GetAllAssignmentsResult>> 
    {
    }

    
}
