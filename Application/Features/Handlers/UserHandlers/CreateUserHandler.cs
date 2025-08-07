using Application.Features.Commands.UserCommands;
using Application.interfaces;
using Application.Tools;
using Domain.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Handlers.UserHandlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var role = await _userRepository.GetRoleByNameAsync(request.Role);

            if (role == null)
                throw new Exception("Geçerli bir rol bulunamadı.");

            var user = new User
            {
                FullName = request.FullName,
                Username = request.Email,
                PasswordHash = request.PasswordHash, // Direkt olarak gelen şifre
                AppRoleId = role.AppRoleId,
                DepartmentId = request.DepartmentId
            };

            await _userRepository.AddAsync(user);
            return user.Id;
        }
    }
}
