using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Actions.Aprendiz;
using YachayPeru.Application.Actions.Auth;
using YachayPeru.Application.Actions.Courses;
using YachayPeru.Application.Actions.Roles;
using YachayPeru.Application.Actions.Users;
using YachayPeru.Application.Common.Behaviors;
using YachayPeru.Application.Features.Administration.Users.Commands.CreateUser;

namespace YachayPeru.Application
{

    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
            cfg.RegisterServicesFromAssembly(typeof(CreateUserHandler).Assembly);
            });

            services.AddValidatorsFromAssembly(typeof(CreateUserCommandValidator).Assembly);
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
            services.AddScoped<UserPlatformCrudActions>();
            services.AddScoped<PlatformRoleCrudActions>();
            services.AddScoped<CourseCrudActions>();
            services.AddScoped<CourseContentActions>();
            services.AddScoped<RetoActions>();
            services.AddScoped<CertificateActions>();
            services.AddScoped<IAuthTokenIssuer, AuthTokenIssuer>();
            services.AddScoped<IInsigniaEvaluator, InsigniaEvaluator>();

            return services;
        }
    }
}
