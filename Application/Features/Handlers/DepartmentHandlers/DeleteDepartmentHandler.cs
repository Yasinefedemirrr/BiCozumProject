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
    public class DeleteDepartmentHandler : IRequestHandler<DeleteDepartmentCommand, Unit>
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUserRepository _userRepository;

        public DeleteDepartmentHandler(
            IDepartmentRepository departmentRepository,
            IUserRepository userRepository)
        {
            _departmentRepository = departmentRepository;
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await _departmentRepository.GetByIdAsync(request.Id);

            if (department == null)
                return Unit.Value;

            // Önce user’ları sil
            var users = await _userRepository.GetUsersByDepartmentAsync(request.Id);
            foreach (var user in users)
            {
                await _userRepository.DeleteAsync(user);
            }

            // Departmanı sil
            await _departmentRepository.DeleteAsync(department);
            return Unit.Value;
        }
    }
}
