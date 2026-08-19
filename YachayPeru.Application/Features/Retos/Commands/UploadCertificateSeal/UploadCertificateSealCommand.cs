using MediatR;
using YachayPeru.Application.Actions.Courses;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Retos.Commands.UploadCertificateSeal
{
    public sealed record UploadCertificateSealCommand : IRequest<Result<string>>
    {
        public int RetoId { get; init; }
        public Stream FileStream { get; init; } = Stream.Null;
        public string FileName { get; init; } = string.Empty;
    }

    public class UploadCertificateSealHandler : IRequestHandler<UploadCertificateSealCommand, Result<string>>
    {
        private readonly CertificateActions certificateActions;
        public UploadCertificateSealHandler(CertificateActions _certificateActions) => certificateActions = _certificateActions;
        public Task<Result<string>> Handle(UploadCertificateSealCommand request, CancellationToken ct)
            => certificateActions.UploadSeal(request.RetoId, request.FileStream, request.FileName, ct);
    }
}
