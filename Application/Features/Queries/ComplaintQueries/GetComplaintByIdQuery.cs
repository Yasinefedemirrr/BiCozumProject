using Application.Features.Results.ComplaintResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Queries.ComplaintQueries
{
    public class GetComplaintByIdQuery : IRequest<GetComplaintByIdResult>
    {
        public int Id { get; set; }

        public GetComplaintByIdQuery(int id)
        {
            Id = id;
        }
    }
}
