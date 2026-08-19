using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Aprendiz.Calendario.Commands.PutRecordatorio
{
    public sealed record PutRecordatorioCommand : IRequest<Result>
    {
        public int UserId { get; init; }
        public int FestividadId { get; init; }
        public bool Enabled { get; init; }
    }
}
