using MediatR;

namespace YachayPeru.Application.Features.Retos.Queries.GetCertificateList
{
    public record GetCertificateListQuery(int CourseId) : IRequest<IReadOnlyList<CertificateListItem>>;
}
