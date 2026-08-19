using YachayPeru.Domain.Entities.Learning;

namespace YachayPeru.Application.Abstractions.Persistence.Learning
{
    public interface ICertificateTemplateRepository : IRepository<CertificateTemplate>
    {
        Task<CertificateTemplate?> GetByRetoAsync(int retoId, CancellationToken ct = default);
    }
}
