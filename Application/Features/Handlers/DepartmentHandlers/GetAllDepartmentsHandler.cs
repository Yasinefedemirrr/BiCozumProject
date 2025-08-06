using Application.Features.Queries.DepartmentQueries;
using Application.Features.Results.DepartmentResults;
using Application.interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Handlers.DepartmentHandlers
{
    public class GetAllDepartmentsHandler : IRequestHandler<GetAllDepartmentsQuery, List<GetAllDepartmentsResult>>
    {
        private readonly IDepartmentRepository _departmentRepository;

        public GetAllDepartmentsHandler(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<List<GetAllDepartmentsResult>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
        {
            var departments = await _departmentRepository.GetAllAsync();

            return departments.Select(d => new GetAllDepartmentsResult
            {
                Id = d.Id,
                Name = d.Name
            }).ToList();
        }
    }
}
