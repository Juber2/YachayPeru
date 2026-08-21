using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using YachayPeru.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using YachayPeru.API.Config;
using YachayPeru.API.Contracts.Common;
using YachayPeru.API.Mappings;
using YachayPeru.API.Middleware;
using YachayPeru.API.Services;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application;
using YachayPeru.Application.Common.Exceptions;
using YachayPeru.Infrastructure;
using YachayPeru.Infrastructure.Config;
using YachayPeru.Infrastructure.Persistence.SqlServer.Seed;
using System.Text;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Conventions.Insert(0, new RoutePrefixConvention("api/v1"));
})
.AddJsonOptions(o => o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase)
.ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .SelectMany(x => x.Value!.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToArray();
        throw new YachayPeru.Application.Common.Exceptions.ValidationException("00",errors );

    };
}); 

builder.Services.AddSingleton<AppMapper>();

var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Sin esto, ASP.NET Core remapea "sub" → ClaimTypes.NameIdentifier (y otros claims
        // cortos del JWT a URIs largas), rompiendo ICurrentUser.Id que busca "sub" tal cual.
        options.MapInboundClaims = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
            ClockSkew = TimeSpan.Zero
        };
        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                var json = """{"success":false,"message":"No autorizado.","errors":["TOKEN_INVALID"]}""";
                return context.Response.WriteAsync(json);
            },
            OnForbidden = context =>
            {
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";
                var json = """{"success":false,"message":"Acceso prohibido.","errors":["FORBIDDEN"]}""";
                return context.Response.WriteAsync(json);
            }
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.FullName?.Replace("+", ".") ?? type.Name);
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Ingresá el token JWT. Ejemplo: Bearer {token}"
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, CurrentUserService>();

builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, AnyPermissionAuthorizationHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AppPermissions.CourseEditor.Read, policy => policy
        .RequireAuthenticatedUser()
        .AddRequirements(new AnyPermissionRequirement(
            AppPermissions.CursosPlantilla.Read)));

    options.AddPolicy(AppPermissions.CourseEditor.Update, policy => policy
        .RequireAuthenticatedUser()
        .AddRequirements(new AnyPermissionRequirement(
            AppPermissions.CursosPlantilla.Update)));
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
var corsOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins(corsOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
var app = builder.Build();

// Detrás de Nginx, este proceso solo recibe HTTP puertas adentro (el certificado TLS lo maneja
// Nginx). Sin esto, request.Scheme siempre da "http" y las URLs absolutas (imágenes, etc.) se
// arman como http:// aunque el sitio se sirva por https:// -> "Mixed Content" en el navegador.
var forwardedHeadersOptions = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
};
forwardedHeadersOptions.KnownIPNetworks.Clear();
forwardedHeadersOptions.KnownProxies.Clear();
app.UseForwardedHeaders(forwardedHeadersOptions);

app.UseCors("AllowAngular");
if (app.Environment.IsDevelopment())
{
    await app.Services.InitialiseDatabaseAsync();
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseMiddleware<GlobalExceptionHandler>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
