using MediatR;
using YachayPeru.Application.Actions.Courses;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Retos.Commands.UploadCertificateLogo
{
    public sealed record UploadCertificateLogoCommand : IRequest<Result<string>>
    {
        public int RetoId { get; init; }
        public Stream FileStream { get; init; } = Stream.Null;
        public string FileName { get; init; } = string.Empty;
    }

    public class UploadCertificateLogoHandler : IRequestHandler<UploadCertificateLogoCommand, Result<string>>
    {
        private readonly CertificateActions certificateActions;
        public UploadCertificateLogoHandler(CertificateActions _certificateActions) => certificateActions = _certificateActions;
        public Task<Result<string>> Handle(UploadCertificateLogoCommand request, CancellationToken ct)
            => certificateActions.UploadLogo(request.RetoId, request.FileStream, request.FileName, ct);
    }
}
