using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Administration.Users.Commands.EditUser
{
    public sealed record EditUserCommand : IRequest<Result<int>>
    {
        public int Id { get; init; }
        public string FirstName { get; init; } = default!;
        public string LastName { get; init; } = default!;
        public string Username { get; init; } = default!;
        public string Email { get; init; } = default!;
        public string? Password { get; init; }
        public bool IsLocked { get; init; }
        public int? RoleId { get; init; }
    }
}
