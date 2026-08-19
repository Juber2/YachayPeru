using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Application.Abstractions.Persistence.Assessment;
using YachayPeru.Application.Abstractions.Persistence.Learning;

namespace YachayPeru.Application.Features.Aprendiz.Certificados.Queries.GetCertificados
{
    public class GetCertificadosHandler : IRequestHandler<GetCertificadosQuery, IReadOnlyList<AprendizCertificadoListItem>>
    {
        private readonly IRetoAttemptRepository attemptRepository;
        private readonly IRetoVersionRepository versionRepository;
        private readonly ICertificateTemplateRepository certificateRepository;

        public GetCertificadosHandler(
            IRetoAttemptRepository _attemptRepository,
            IRetoVersionRepository _versionRepository,
            ICertificateTemplateRepository _certificateRepository)
        {
            attemptRepository = _attemptRepository;
            versionRepository = _versionRepository;
            certificateRepository = _certificateRepository;
        }

        public async Task<IReadOnlyList<AprendizCertificadoListItem>> Handle(GetCertificadosQuery request, CancellationToken ct)
        {
            var passedRetoIds = await attemptRepository.GetPassedRetoIdsByUserAsync(request.UserId, ct);
            var items = new List<AprendizCertificadoListItem>();

            foreach (var retoId in passedRetoIds)
            {
                var published = await versionRepository.GetPublishedByRetoAsync(retoId, ct);
                var certificate = await certificateRepository.GetByRetoAsync(retoId, ct);

                items.Add(new AprendizCertificadoListItem
                {
                    RetoId = retoId,
                    RetoTitle = published?.Title ?? string.Empty,
                    IsAvailable = certificate is not null
                });
            }

            return items;
        }
    }
}
