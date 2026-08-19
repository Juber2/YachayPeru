using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Assessment;
using YachayPeru.Application.Abstractions.Persistence.Learning;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Actions.Courses.Models;
using YachayPeru.Application.Common.Results;
using YachayPeru.Application.Features.Retos.Queries.GetCertificate;
using YachayPeru.Application.Features.Retos.Queries.GetCertificateList;
using YachayPeru.Domain.Entities.Learning;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Actions.Courses
{
    public class CertificateActions
    {
        private readonly ICertificateTemplateRepository certificateRepository;
        private readonly IRetoRepository retoRepository;
        private readonly IRetoVersionRepository versionRepository;
        private readonly IFileStorageService fileStorage;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUser currentUser;

        public CertificateActions(
            ICertificateTemplateRepository _certificateRepository,
            IRetoRepository _retoRepository,
            IRetoVersionRepository _versionRepository,
            IFileStorageService _fileStorage,
            IUnitOfWork _unitOfWork,
            ICurrentUser _currentUser)
        {
            certificateRepository = _certificateRepository;
            retoRepository = _retoRepository;
            versionRepository = _versionRepository;
            fileStorage = _fileStorage;
            unitOfWork = _unitOfWork;
            currentUser = _currentUser;
        }

        public async Task<IReadOnlyList<CertificateListItem>> GetCertificateList(int courseId, CancellationToken ct)
        {
            var retos = await retoRepository.GetByCourseAsync(courseId, ct);
            var items = new List<CertificateListItem>();

            foreach (var reto in retos)
            {
                var current = await versionRepository.GetDraftByRetoAsync(reto.Id, ct)
                    ?? await versionRepository.GetPublishedByRetoAsync(reto.Id, ct);
                if (current is null) continue;

                var certificate = await certificateRepository.GetByRetoAsync(reto.Id, ct);
                items.Add(new CertificateListItem
                {
                    RetoId = reto.Id,
                    RetoTitle = current.Title,
                    IsConfigured = certificate is not null
                });
            }

            return items;
        }

        public async Task<CertificateDetail?> GetCertificate(int retoId, CancellationToken ct)
        {
            var cert = await certificateRepository.GetByRetoAsync(retoId, ct);
            if (cert is null) return null;

            return new CertificateDetail
            {
                RetoId = cert.RetoId,
                MainTitle = cert.MainTitle,
                Subtitle = cert.Subtitle,
                BodyText = cert.BodyText,
                FooterText = cert.FooterText,
                IncludeFields = new CertificateInfoFields
                {
                    CompletionDate = cert.IncludeCompletionDate,
                    Score = cert.IncludeScore,
                    Duration = cert.IncludeDuration,
                    QrCode = cert.IncludeQrCode,
                    EmployeeId = cert.IncludeEmployeeId,
                    CertificateId = cert.IncludeCertificateId,
                    Instructor = cert.IncludeInstructor,
                    Location = cert.IncludeLocation,
                    Modality = cert.IncludeModality,
                    Validity = cert.IncludeValidity
                },
                Orientation = cert.Orientation,
                Prefix = cert.Prefix,
                PrimaryColor = cert.PrimaryColor,
                SecondaryColor = cert.SecondaryColor,
                AccentColor = cert.AccentColor,
                FontFamily = cert.FontFamily,
                BorderStyle = cert.BorderStyle,
                BorderWidth = cert.BorderWidth,
                ShowLogo = cert.ShowLogo,
                LogoUrl = cert.LogoUrl,
                SignerName = cert.SignerName,
                SignerTitle = cert.SignerTitle,
                SignatureUrl = cert.SignatureUrl,
                ShowSeal = cert.ShowSeal,
                SealUrl = cert.SealUrl,
                ShowWatermark = cert.ShowWatermark
            };
        }

        public async Task<Result> UpsertCertificate(UpsertCertificateInput input, CancellationToken ct)
        {
            var reto = await retoRepository.GetByIdAsync(input.RetoId, ct);
            if (reto is null)
                return Result.Failure("Reto no encontrado.", NotFound);

            var existing = await certificateRepository.GetByRetoAsync(input.RetoId, ct);

            if (existing is null)
            {
                var cert = new CertificateTemplate
                {
                    RetoId = input.RetoId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = currentUser.Id
                };
                ApplySettings(cert, input);
                await certificateRepository.AddAsync(cert, ct);
                await unitOfWork.SaveChangesAsync(ct);
                return Result.Success();
            }

            ApplySettings(existing, input);
            existing.UpdatedAt = DateTime.UtcNow;
            existing.UpdatedBy = currentUser.Id;

            certificateRepository.Update(existing);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }

        private static void ApplySettings(CertificateTemplate cert, UpsertCertificateInput input)
        {
            cert.MainTitle = input.MainTitle;
            cert.Subtitle = input.Subtitle;
            cert.BodyText = input.BodyText;
            cert.FooterText = input.FooterText;

            cert.IncludeCompletionDate = input.IncludeFields.CompletionDate;
            cert.IncludeScore = input.IncludeFields.Score;
            cert.IncludeDuration = input.IncludeFields.Duration;
            cert.IncludeQrCode = input.IncludeFields.QrCode;
            cert.IncludeEmployeeId = input.IncludeFields.EmployeeId;
            cert.IncludeCertificateId = input.IncludeFields.CertificateId;
            cert.IncludeInstructor = input.IncludeFields.Instructor;
            cert.IncludeLocation = input.IncludeFields.Location;
            cert.IncludeModality = input.IncludeFields.Modality;
            cert.IncludeValidity = input.IncludeFields.Validity;

            cert.Orientation = input.Orientation;
            cert.Prefix = input.Prefix;

            cert.PrimaryColor = input.PrimaryColor;
            cert.SecondaryColor = input.SecondaryColor;
            cert.AccentColor = input.AccentColor;

            cert.FontFamily = input.FontFamily;
            cert.BorderStyle = input.BorderStyle;
            cert.BorderWidth = input.BorderWidth;

            cert.ShowLogo = input.ShowLogo;
            cert.SignerName = input.SignerName;
            cert.SignerTitle = input.SignerTitle;
            cert.ShowSeal = input.ShowSeal;
            cert.ShowWatermark = input.ShowWatermark;
        }

        public async Task<Result<string>> UploadLogo(int retoId, Stream stream, string fileName, CancellationToken ct)
            => await UploadAsset(retoId, stream, fileName, (cert, url) => cert.LogoUrl = url, ct);

        public async Task<Result<string>> UploadSignature(int retoId, Stream stream, string fileName, CancellationToken ct)
            => await UploadAsset(retoId, stream, fileName, (cert, url) => cert.SignatureUrl = url, ct);

        public async Task<Result<string>> UploadSeal(int retoId, Stream stream, string fileName, CancellationToken ct)
            => await UploadAsset(retoId, stream, fileName, (cert, url) => cert.SealUrl = url, ct);

        private async Task<Result<string>> UploadAsset(
            int retoId, Stream stream, string fileName, Action<CertificateTemplate, string> assign, CancellationToken ct)
        {
            var cert = await certificateRepository.GetByRetoAsync(retoId, ct);
            if (cert is null)
                return Result<string>.Failure("El reto no tiene un certificado configurado.", NotFound);

            var url = await fileStorage.SaveAsync(stream, fileName, "certificates", ct);
            assign(cert, url);
            cert.UpdatedAt = DateTime.UtcNow;
            cert.UpdatedBy = currentUser.Id;

            certificateRepository.Update(cert);
            await unitOfWork.SaveChangesAsync(ct);

            return Result<string>.Success(url);
        }
    }
}
