using MediatR;

namespace Application.Features.Commands.UserCommands
{
    public class DeleteUserCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
