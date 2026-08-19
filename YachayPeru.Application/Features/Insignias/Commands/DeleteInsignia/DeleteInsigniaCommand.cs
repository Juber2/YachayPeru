using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Insignias.Commands.DeleteInsignia
{
    public record DeleteInsigniaCommand(int Id) : IRequest<Result>;
}
