using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Aprendiz.Certificados.Queries.GetCertificadoPdf
{
    public record GetCertificadoPdfQuery(int UserId, int RetoId) : IRequest<Result<byte[]>>;
}
