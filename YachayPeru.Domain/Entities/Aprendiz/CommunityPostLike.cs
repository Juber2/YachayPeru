using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Aprendiz
{
    public class CommunityPostLike : BaseEntity
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
    }
}
