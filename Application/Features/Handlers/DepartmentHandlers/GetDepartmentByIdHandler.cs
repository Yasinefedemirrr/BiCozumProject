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
    public class GetDepartmentByIdHandler : IRequestHandler<GetDepartmentByIdQuery, GetDepartmentByIdResult>
    {
        private readonly IDepartmentRepository _departmentRepository;

        public GetDepartmentByIdHandler(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<GetDepartmentByIdResult> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
        {
            var department = await _departmentRepository.GetByIdAsync(request.Id);
            if (department == null) return null;

            return new GetDepartmentByIdResult
            {
                Id = department.Id,
                Name = department.Name
            };
        }
    }
}