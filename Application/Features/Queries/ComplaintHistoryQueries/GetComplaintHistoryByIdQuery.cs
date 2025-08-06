using Application.Features.Results.ComplaintHistoryResult;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Queries.ComplaintHistoryQueries
{
    public class GetComplaintHistoryByIdQuery : IRequest<GetComplaintHistoryByIdResult>
    {
        public int Id { get; set; }
        public GetComplaintHistoryByIdQuery(int id)
        {
            Id = id;
        }
    }
}
