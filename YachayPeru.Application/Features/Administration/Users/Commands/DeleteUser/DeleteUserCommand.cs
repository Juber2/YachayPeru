using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Administration.Users.Commands.DeleteUser
{
    public sealed record DeleteUserCommand(int Id) : IRequest<Result>;
}
