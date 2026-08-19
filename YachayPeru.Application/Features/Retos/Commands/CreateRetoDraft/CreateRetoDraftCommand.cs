using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Retos.Commands.CreateRetoDraft
{
    public record CreateRetoDraftCommand(int RetoId) : IRequest<Result<int>>;
}
