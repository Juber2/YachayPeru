using MediatR;
using YachayPeru.Application.Actions.Courses;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Retos.Commands.UploadCertificateSignature
{
    public sealed record UploadCertificateSignatureCommand : IRequest<Result<string>>
    {
        public int RetoId { get; init; }
        public Stream FileStream { get; init; } = Stream.Null;
        public string FileName { get; init; } = string.Empty;
    }

    public class UploadCertificateSignatureHandler : IRequestHandler<UploadCertificateSignatureCommand, Result<string>>
    {
        private readonly CertificateActions certificateActions;
        public UploadCertificateSignatureHandler(CertificateActions _certificateActions) => certificateActions = _certificateActions;
        public Task<Result<string>> Handle(UploadCertificateSignatureCommand request, CancellationToken ct)
            => certificateActions.UploadSignature(request.RetoId, request.FileStream, request.FileName, ct);
    }
}
