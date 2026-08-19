using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Calendario.Commands.DeleteFestividad
{
    public record DeleteFestividadCommand(int Id) : IRequest<Result>;
}
