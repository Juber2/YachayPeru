using MediatR;
using YachayPeru.Application.Actions.Courses;

namespace YachayPeru.Application.Features.Retos.Queries.GetCertificateList
{
    public class GetCertificateListHandler : IRequestHandler<GetCertificateListQuery, IReadOnlyList<CertificateListItem>>
    {
        private readonly CertificateActions certificateActions;
        public GetCertificateListHandler(CertificateActions _certificateActions) => certificateActions = _certificateActions;
        public Task<IReadOnlyList<CertificateListItem>> Handle(GetCertificateListQuery request, CancellationToken ct)
            => certificateActions.GetCertificateList(request.CourseId, ct);
    }
}
