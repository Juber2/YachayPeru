using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Aprendiz
{
    public class FestividadReminder : BaseEntity
    {
        public int UserId { get; set; }
        public int FestividadId { get; set; }
        public bool Enabled { get; set; }
    }
}
