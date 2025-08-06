using Application.Features.Results.DepartmentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Queries.DepartmentQueries
{
    public class GetDepartmentByIdQuery : IRequest<GetDepartmentByIdResult>
    {
        public int Id { get; set; }
        public GetDepartmentByIdQuery(int id)
        {
            Id = id;
        }
    }
}
