using Application.Features.Commands.UserCommands;
using Application.interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Handlers.UserHandlers
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user != null)
            {
                var role = await _userRepository.GetRoleByNameAsync(request.Role); // Rol nesnesini çekiyoruz
                if (role == null)
                {
                    throw new Exception("Belirtilen rol bulunamadı.");
                }

                user.FullName = request.FullName;
                user.Username = request.Email;
                user.PasswordHash = request.PasswordHash;
                user.AppRoleId = role.AppRoleId; // Doğrudan ID atanır
                user.DepartmentId = request.DepartmentId;

                await _userRepository.UpdateAsync(user);
            }

            return Unit.Value;
        }
    }
}
