using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Retos.Commands.DiscardRetoDraft
{
    public record DiscardRetoDraftCommand(int RetoId) : IRequest<Result>;
}
