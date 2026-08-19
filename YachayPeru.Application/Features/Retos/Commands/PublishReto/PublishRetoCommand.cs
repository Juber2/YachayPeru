using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Retos.Commands.PublishReto
{
    public record PublishRetoCommand(int RetoId) : IRequest<Result<int>>;
}
