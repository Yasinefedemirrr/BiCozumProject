using Application.Features.Results.AppUserResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Queries.AppUserQueries
{
    public class GetCheckUserQuery : IRequest<GetCheckUserQueryResult>
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
