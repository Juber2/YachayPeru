using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Retos.Commands.DeleteReto
{
    public record DeleteRetoCommand(int RetoId) : IRequest<Result>;
}
