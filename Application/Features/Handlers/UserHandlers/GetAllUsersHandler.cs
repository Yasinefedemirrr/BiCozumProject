using Application.Features.Queries.UserQueries;
using Application.Features.Results.UserResults;
using Application.interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Handlers.UserHandlers
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, List<GetAllUsersResult>>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<GetAllUsersResult>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllWithIncludesAsync();

            return users.Select(u => new GetAllUsersResult
            {
                Id = u.Id,
                FullName = u.FullName,
                Role = u.AppRole?.AppRoleName ?? "",
                DepartmentName = u.Department?.Name ?? ""
            }).ToList();
        }
    }
}
