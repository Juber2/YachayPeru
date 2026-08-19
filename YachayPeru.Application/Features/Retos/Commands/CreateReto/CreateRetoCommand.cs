using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Retos.Commands.CreateReto
{
    public record CreateRetoCommand(int CourseId) : IRequest<Result<int>>;
}
