using YachayPeru.Domain.Entities.Aprendiz;

namespace YachayPeru.Application.Abstractions.Persistence.Aprendiz
{
    public interface ICommunityPostLikeRepository : IRepository<CommunityPostLike>
    {
        Task<CommunityPostLike?> GetByPostAndUserAsync(int postId, int userId, CancellationToken ct = default);
        Task<int> CountByPostAsync(int postId, CancellationToken ct = default);
        Task<IReadOnlyDictionary<int, int>> CountByPostsAsync(IReadOnlyCollection<int> postIds, CancellationToken ct = default);
        Task<IReadOnlyCollection<int>> GetLikedPostIdsAsync(int userId, IReadOnlyCollection<int> postIds, CancellationToken ct = default);
    }
}
