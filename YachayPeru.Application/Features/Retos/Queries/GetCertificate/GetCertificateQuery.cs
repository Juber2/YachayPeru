using MediatR;

namespace YachayPeru.Application.Features.Retos.Queries.GetCertificate
{
    public record GetCertificateQuery(int RetoId) : IRequest<CertificateDetail?>;
}
