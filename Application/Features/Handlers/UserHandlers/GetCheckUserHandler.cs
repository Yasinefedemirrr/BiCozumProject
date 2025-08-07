using Application.Features.Queries.AppUserQueries;
using Application.Features.Results.AppUserResults;
using Application.interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Handlers.UserHandlers
{
    public class GetCheckUserHandler : IRequestHandler<GetCheckUserQuery, GetCheckUserQueryResult>
    {
        private readonly IUserRepository _userRepository;

        public GetCheckUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetCheckUserQueryResult> Handle(GetCheckUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdWithIncludesAsync(request.Id);

            if (user == null)
                return null!;
                
            return new GetCheckUserQueryResult
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.AppRole?.AppRoleName ?? "Visitor"
            };
        }
    }
}
