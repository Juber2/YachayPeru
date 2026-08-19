using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Application.Common;

namespace YachayPeru.Application.Features.Aprendiz.Perfil.Queries.GetActividad
{
    public class GetActividadHandler : IRequestHandler<GetActividadQuery, IReadOnlyList<AprendizActividadItem>>
    {
        private readonly IAprendizActivityLogRepository repository;
        public GetActividadHandler(IAprendizActivityLogRepository _repository) => repository = _repository;

        public async Task<IReadOnlyList<AprendizActividadItem>> Handle(GetActividadQuery request, CancellationToken ct)
        {
            var logs = await repository.GetByUserAsync(request.UserId, ct);
            return logs.Select(l => new AprendizActividadItem
            {
                Text = l.Text,
                When = RelativeTimeFormatter.Format(l.CreatedAt)
            }).ToList();
        }
    }
}
