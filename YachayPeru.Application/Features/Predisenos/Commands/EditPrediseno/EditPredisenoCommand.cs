using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Predisenos.Commands.EditPrediseno
{
    public sealed record EditPredisenoCommand : IRequest<Result>
    {
        public int Id { get; init; }
        public string Title { get; init; } = default!;
        public string TreeJson { get; init; } = default!;
    }
}
