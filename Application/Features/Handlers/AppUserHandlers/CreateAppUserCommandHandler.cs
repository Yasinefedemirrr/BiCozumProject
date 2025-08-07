using Application.Enums;
using Application.Features.Commands.AppUserCommand;
using Application.interfaces;
using Domain.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Handlers.AppUserHandlers
{
    public class CreateAppUserCommandHandler : IRequestHandler<CreateAppUserCommand>
    {
        private readonly IRepository<AppUser> _repository;
        public CreateAppUserCommandHandler(IRepository<AppUser> repository)
        {
            _repository = repository;
        }
        public async Task Handle(CreateAppUserCommand request, CancellationToken cancellationToken)
        {
            await _repository.AddAsync(new AppUser
            {
                Password = request.Password,
                Username = request.Username,
                AppRoleId = (int)RolesType.Member,

            });
        }
    }
}
