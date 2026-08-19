using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Administration.Users.Commands.CreateUser
{
    public sealed record CreateUserCommand : IRequest<Result<int>>
    {
        public string FirstName { get; init; } = default!;
        public string LastName { get; init; } = default!;
        public string UserName { get; init; } = default!;
        public string Email { get; init; } = default!;
        public string Password { get; init; } = default!;
        public bool SendWelcomeMessage { get; init; }
        public int? RoleId { get; init; }
        public int? ReactivateUserId { get; init; }
    }
}
