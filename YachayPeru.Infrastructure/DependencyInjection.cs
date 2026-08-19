using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Access;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Application.Abstractions.Persistence.Assessment;
using YachayPeru.Application.Abstractions.Persistence.Auth;
using YachayPeru.Application.Abstractions.Persistence.Common;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Persistence.Learning;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Infrastructure.Persistence.Repositories;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using YachayPeru.Infrastructure.Config;
using YachayPeru.Application.Common.Settings;
using YachayPeru.Infrastructure.Persistence.SqlServer.Seed;
using YachayPeru.Infrastructure.Persistence.UnitOfWork;
using YachayPeru.Infrastructure.Services;
using Microsoft.Extensions.Options;

namespace YachayPeru.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                )
            );

            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
            services.Configure<Application.Common.Settings.SecuritySettings>(
                configuration.GetSection(Application.Common.Settings.SecuritySettings.SectionName));
            services.Configure<Application.Common.Settings.WelcomeEmailSettings>(
                configuration.GetSection(Application.Common.Settings.WelcomeEmailSettings.SectionName));
            services.Configure<Application.Common.Settings.SmtpSettings>(
                configuration.GetSection(Application.Common.Settings.SmtpSettings.SectionName));

            services.AddMemoryCache();
            services.AddSingleton<IPermissionCache, RolePermissionCache>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IMasterCodeRepository, MasterCodeRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IPlatformRoleRepository, PlatformRoleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IEmailService, SmtpEmailService>();
            services.AddScoped<IFileStorageService, LocalFileStorageService>();
            services.AddScoped<ICertificatePdfRenderer, QuestPdfCertificateRenderer>();
            services.AddScoped<DbSeeder>();

            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<ICourseVersionRepository, CourseVersionRepository>();
            services.AddScoped<ICourseModuleRepository, CourseModuleRepository>();
            services.AddScoped<IModuleContentRepository, ModuleContentRepository>();
            services.AddScoped<IModuleContentFileRepository, ModuleContentFileRepository>();
            services.AddScoped<ICertificateTemplateRepository, CertificateTemplateRepository>();
            services.AddScoped<IRetoRepository, RetoRepository>();
            services.AddScoped<IRetoVersionRepository, RetoVersionRepository>();
            services.AddScoped<IRetoVersionQuestionRepository, RetoVersionQuestionRepository>();

            services.AddScoped<IInsigniaRepository, InsigniaRepository>();
            services.AddScoped<IMediaItemRepository, MediaItemRepository>();
            services.AddScoped<IFestividadRepository, FestividadRepository>();
            services.AddScoped<IRegionDestacadaRepository, RegionDestacadaRepository>();
            services.AddScoped<INoticiaRepository, NoticiaRepository>();
            services.AddScoped<IPredisenoRepository, PredisenoRepository>();
            services.AddScoped<IPremiumPlanRepository, PremiumPlanRepository>();
            services.AddScoped<IPremiumBenefitRepository, PremiumBenefitRepository>();

            services.AddScoped<IAprendizProfileRepository, AprendizProfileRepository>();
            services.AddScoped<IAprendizActivityLogRepository, AprendizActivityLogRepository>();
            services.AddScoped<IAprendizRegionActivityRepository, AprendizRegionActivityRepository>();
            services.AddScoped<IAprendizRegionExploredRepository, AprendizRegionExploredRepository>();
            services.AddScoped<IRetoAttemptRepository, RetoAttemptRepository>();
            services.AddScoped<IAprendizInsigniaEarnedRepository, AprendizInsigniaEarnedRepository>();
            services.AddScoped<IFestividadReminderRepository, FestividadReminderRepository>();
            services.AddScoped<ICommunityPostRepository, CommunityPostRepository>();
            services.AddScoped<ICommunityPostLikeRepository, CommunityPostLikeRepository>();
            services.AddScoped<IPremiumWaitlistEntryRepository, PremiumWaitlistEntryRepository>();

            return services;
        }
    }
}
