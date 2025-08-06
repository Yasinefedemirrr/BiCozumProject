using Application.Features.Commands.DepartmentCommands;
using Application.interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Handlers.DepartmentHandlers
{
    public class UpdateDepartmentHandler : IRequestHandler<UpdateDepartmentCommand, Unit>
    {
        private readonly IDepartmentRepository _departmentRepository;

        public UpdateDepartmentHandler(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<Unit> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await _departmentRepository.GetByIdAsync(request.Id);
            if (department != null)
            {
                department.Name = request.Name;
                await _departmentRepository.UpdateAsync(department);
            }
            return Unit.Value;
        }
    }
}
