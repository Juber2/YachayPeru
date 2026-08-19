using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Aprendiz
{
    public class AprendizProfile : BaseEntity
    {
        public int UserId { get; set; }
        public string? AvatarUrl { get; set; }
        public int Points { get; set; }
        public int Level { get; set; } = 1;
        public int? FavoriteRegionId { get; set; }
        public bool IsPremiumUser { get; set; }
        public int ModulesDone { get; set; }
        public int LearningTimeMinutes { get; set; }
        public DateOnly? LastActiveDate { get; set; }
        public int CurrentLoginStreakDays { get; set; }
        public int BestLoginStreakDays { get; set; }
        public int CurrentAnswerStreak { get; set; }
        public int BestAnswerStreak { get; set; }
    }
}
