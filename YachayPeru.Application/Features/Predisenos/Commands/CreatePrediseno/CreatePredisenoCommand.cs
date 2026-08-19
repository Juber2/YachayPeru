using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Predisenos.Commands.CreatePrediseno
{
    public sealed record CreatePredisenoCommand : IRequest<Result<int>>
    {
        public string Title { get; init; } = default!;
        public string TreeJson { get; init; } = default!;
    }
}
