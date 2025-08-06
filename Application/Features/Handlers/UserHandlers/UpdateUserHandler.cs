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
                user.FullName = request.FullName;
                user.Email = request.Email;
                user.PasswordHash = request.PasswordHash;
                user.Role = request.Role;
                user.DepartmentId = request.DepartmentId;

                await _userRepository.UpdateAsync(user);
            }

            return Unit.Value;
        }
    }
}
