using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Application.Abstractions.Persistence.Assessment;
using YachayPeru.Application.Abstractions.Persistence.Auth;
using YachayPeru.Application.Abstractions.Persistence.Learning;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Aprendiz.Certificados.Queries.GetCertificadoPdf
{
    public class GetCertificadoPdfHandler : IRequestHandler<GetCertificadoPdfQuery, Result<byte[]>>
    {
        private readonly IRetoAttemptRepository attemptRepository;
        private readonly IRetoRepository retoRepository;
        private readonly IRetoVersionRepository versionRepository;
        private readonly ICertificateTemplateRepository certificateRepository;
        private readonly ICourseRepository courseRepository;
        private readonly IUserRepository userRepository;
        private readonly IFileStorageService fileStorage;
        private readonly ICertificatePdfRenderer renderer;

        public GetCertificadoPdfHandler(
            IRetoAttemptRepository _attemptRepository,
            IRetoRepository _retoRepository,
            IRetoVersionRepository _versionRepository,
            ICertificateTemplateRepository _certificateRepository,
            ICourseRepository _courseRepository,
            IUserRepository _userRepository,
            IFileStorageService _fileStorage,
            ICertificatePdfRenderer _renderer)
        {
            attemptRepository = _attemptRepository;
            retoRepository = _retoRepository;
            versionRepository = _versionRepository;
            certificateRepository = _certificateRepository;
            courseRepository = _courseRepository;
            userRepository = _userRepository;
            fileStorage = _fileStorage;
            renderer = _renderer;
        }

        public async Task<Result<byte[]>> Handle(GetCertificadoPdfQuery request, CancellationToken ct)
        {
            var hasPassed = await attemptRepository.HasPassedAsync(request.UserId, request.RetoId, ct);
            if (!hasPassed)
                return Result<byte[]>.Failure("Todavía no aprobaste este reto.", NotFound);

            var certificate = await certificateRepository.GetByRetoAsync(request.RetoId, ct);
            if (certificate is null)
                return Result<byte[]>.Failure("Este reto no tiene un certificado configurado.", NotFound);

            var reto = await retoRepository.GetByIdAsync(request.RetoId, ct);
            var published = await versionRepository.GetPublishedByRetoAsync(request.RetoId, ct);
            var region = reto is not null ? await courseRepository.GetByIdAsync(reto.CourseId, ct) : null;
            var bestAttempt = await attemptRepository.GetBestByUserAndRetoAsync(request.UserId, request.RetoId, ct);
            var user = await userRepository.GetByIdWithPersonAsync(request.UserId, ct);

            var studentName = user?.Person is null ? string.Empty : $"{user.Person.FirstName} {user.Person.LastName}".Trim();

            var logoBytes = certificate.LogoUrl is null ? null : await fileStorage.ReadAsync(certificate.LogoUrl, ct);
            var signatureBytes = certificate.SignatureUrl is null ? null : await fileStorage.ReadAsync(certificate.SignatureUrl, ct);
            var sealBytes = certificate.SealUrl is null ? null : await fileStorage.ReadAsync(certificate.SealUrl, ct);

            var certificateId = $"{request.UserId}-{request.RetoId}-{bestAttempt?.Id ?? 0}";

            var pdfData = new CertificatePdfData
            {
                MainTitle = certificate.MainTitle,
                Subtitle = certificate.Subtitle,
                BodyText = certificate.BodyText,
                FooterText = certificate.FooterText,
                IncludeCompletionDate = certificate.IncludeCompletionDate,
                IncludeScore = certificate.IncludeScore,
                IncludeDuration = certificate.IncludeDuration,
                IncludeQrCode = certificate.IncludeQrCode,
                IncludeEmployeeId = certificate.IncludeEmployeeId,
                IncludeCertificateId = certificate.IncludeCertificateId,
                IncludeInstructor = certificate.IncludeInstructor,
                IncludeLocation = certificate.IncludeLocation,
                IncludeModality = certificate.IncludeModality,
                IncludeValidity = certificate.IncludeValidity,
                Orientation = certificate.Orientation,
                Prefix = certificate.Prefix,
                PrimaryColor = certificate.PrimaryColor,
                SecondaryColor = certificate.SecondaryColor,
                AccentColor = certificate.AccentColor,
                FontFamily = certificate.FontFamily,
                BorderStyle = certificate.BorderStyle,
                BorderWidth = certificate.BorderWidth,
                ShowLogo = certificate.ShowLogo,
                LogoBytes = logoBytes,
                SignerName = certificate.SignerName,
                SignerTitle = certificate.SignerTitle,
                SignatureBytes = signatureBytes,
                ShowSeal = certificate.ShowSeal,
                SealBytes = sealBytes,
                ShowWatermark = certificate.ShowWatermark,
                StudentName = studentName,
                RetoTitle = published?.Title ?? string.Empty,
                RegionTitle = region?.Title ?? string.Empty,
                CompletionDate = bestAttempt?.CreatedAt ?? DateTime.UtcNow,
                Score = bestAttempt?.EarnedPoints ?? 0,
                CertificateId = certificateId,
                QrContent = certificate.IncludeQrCode ? $"YachayPeru certificate verify {certificateId}" : null
            };

            var pdfBytes = renderer.Render(pdfData);
            return Result<byte[]>.Success(pdfBytes);
        }
    }
}
