using MediatR;
using YachayPeru.Application.Actions.Courses;

namespace YachayPeru.Application.Features.Retos.Queries.GetCertificate
{
    public class GetCertificateHandler : IRequestHandler<GetCertificateQuery, CertificateDetail?>
    {
        private readonly CertificateActions certificateActions;
        public GetCertificateHandler(CertificateActions _certificateActions) => certificateActions = _certificateActions;
        public Task<CertificateDetail?> Handle(GetCertificateQuery request, CancellationToken ct)
            => certificateActions.GetCertificate(request.RetoId, ct);
    }
}
