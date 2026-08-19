using Microsoft.EntityFrameworkCore;
using YachayPeru.Domain.Entities.Access;
using YachayPeru.Domain.Entities.Aprendiz;
using YachayPeru.Domain.Entities.Assessment;
using YachayPeru.Domain.Entities.Common;
using YachayPeru.Domain.Entities.Auth;
using YachayPeru.Domain.Entities.Content;
using YachayPeru.Domain.Entities.Learning;

namespace YachayPeru.Infrastructure.Persistence.SqlServer
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // ── Common ────────────────────────────────────────────────────────────────
        public DbSet<MasterCode> MasterCodes => Set<MasterCode>();
        public DbSet<Person> Persons => Set<Person>();

        // ── Auth ──────────────────────────────────────────────────────────────────
        public DbSet<User> Users => Set<User>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<UserPasswordChange> UserPasswordChanges => Set<UserPasswordChange>();

        // ── Learning ──────────────────────────────────────────────────────────────
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<CourseVersion> CourseVersions => Set<CourseVersion>();
        public DbSet<CourseModule> CourseModules => Set<CourseModule>();
        public DbSet<ModuleContent> ModuleContents => Set<ModuleContent>();
        public DbSet<ModuleContentFile> ModuleContentFiles => Set<ModuleContentFile>();
        public DbSet<CertificateTemplate> CertificateTemplates => Set<CertificateTemplate>();

        // ── Assessment ────────────────────────────────────────────────────────────
        public DbSet<Reto> Retos => Set<Reto>();
        public DbSet<RetoVersion> RetoVersions => Set<RetoVersion>();
        public DbSet<RetoVersionQuestion> RetoVersionQuestions => Set<RetoVersionQuestion>();
        public DbSet<RetoVersionQuestionChoice> RetoVersionQuestionChoices => Set<RetoVersionQuestionChoice>();
        public DbSet<RetoVersionQuestionBlank> RetoVersionQuestionBlanks => Set<RetoVersionQuestionBlank>();

        // ── Access ────────────────────────────────────────────────────────────────
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<Resource> Resources => Set<Resource>();
        public DbSet<PlatformRole> PlatformRoles => Set<PlatformRole>();
        public DbSet<PlatformRolePermission> PlatformRolePermissions => Set<PlatformRolePermission>();

        // ── Content ───────────────────────────────────────────────────────────────
        public DbSet<Insignia> Insignias => Set<Insignia>();
        public DbSet<InsigniaRequiredRegion> InsigniaRequiredRegions => Set<InsigniaRequiredRegion>();
        public DbSet<InsigniaRequiredReto> InsigniaRequiredRetos => Set<InsigniaRequiredReto>();
        public DbSet<MediaItem> MediaItems => Set<MediaItem>();
        public DbSet<Festividad> Festividades => Set<Festividad>();
        public DbSet<RegionDestacada> RegionesDestacadas => Set<RegionDestacada>();
        public DbSet<Noticia> Noticias => Set<Noticia>();
        public DbSet<Prediseno> Predisenos => Set<Prediseno>();
        public DbSet<PremiumPlan> PremiumPlans => Set<PremiumPlan>();
        public DbSet<PremiumBenefit> PremiumBenefits => Set<PremiumBenefit>();
        public DbSet<PremiumPlanFeature> PremiumPlanFeatures => Set<PremiumPlanFeature>();

        // ── Aprendiz ──────────────────────────────────────────────────────────────
        public DbSet<AprendizProfile> AprendizProfiles => Set<AprendizProfile>();
        public DbSet<AprendizActivityLog> AprendizActivityLogs => Set<AprendizActivityLog>();
        public DbSet<AprendizRegionActivity> AprendizRegionActivities => Set<AprendizRegionActivity>();
        public DbSet<AprendizRegionExplored> AprendizRegionExplored => Set<AprendizRegionExplored>();
        public DbSet<RetoAttempt> RetoAttempts => Set<RetoAttempt>();
        public DbSet<AprendizInsigniaEarned> AprendizInsigniasEarned => Set<AprendizInsigniaEarned>();
        public DbSet<FestividadReminder> FestividadReminders => Set<FestividadReminder>();
        public DbSet<CommunityPost> CommunityPosts => Set<CommunityPost>();
        public DbSet<CommunityPostLike> CommunityPostLikes => Set<CommunityPostLike>();
        public DbSet<PremiumWaitlistEntry> PremiumWaitlistEntries => Set<PremiumWaitlistEntry>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
