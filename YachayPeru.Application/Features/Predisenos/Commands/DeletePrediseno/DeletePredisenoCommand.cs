using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Predisenos.Commands.DeletePrediseno
{
    public record DeletePredisenoCommand(int Id) : IRequest<Result>;
}
