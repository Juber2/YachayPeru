using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Domain.Constants;
using YachayPeru.Domain.Entities.Access;
using YachayPeru.Domain.Entities.Aprendiz;
using YachayPeru.Domain.Entities.Assessment;
using YachayPeru.Domain.Entities.Auth;
using YachayPeru.Domain.Entities.Common;
using YachayPeru.Domain.Entities.Learning;
using YachayPeru.Infrastructure.Persistence.SqlServer;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Seed
{
    public class DbSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DbSeeder> _logger;
        private readonly IPasswordHasher _passwordHasher;

        public DbSeeder(
            ApplicationDbContext context,
            ILogger<DbSeeder> logger,
            IPasswordHasher passwordHasher)
        {
            _context = context;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        public async Task SeedAsync(CancellationToken ct = default)
        {
            await SeedCataloguesAsync(ct);
            await SeedPermissionsAsync(ct);
            await SeedAdminUserAsync(ct);
            await SeedRegionsAsync(ct);
            await SeedCommunityPostsAsync(ct);
        }

        // ─────────────────────────────────────────────────────────────────────────────

        private async Task SeedCataloguesAsync(CancellationToken ct)
        {
            if (await _context.MasterCodes.AnyAsync(ct))
                return;

            _logger.LogInformation("Seeding master codes and roles...");

            var masterCodes = new List<MasterCode>
            {
                // ── Grupos raíz ───────────────────────────────────────────────────────
                new() { Code = "PERMISSIONS",         Name = "Permissions",         Value = "Permissions",         Description = "Acciones CRUD disponibles en el sistema", OrderIndex = 1 },
                new() { Code = "USER_TYPES",          Name = "User Types",          Value = "User Types",          Description = "Tipos de usuario del sistema",            OrderIndex = 2 },
                new() { Code = "PLATFORM_ROLE_CODES", Name = "Platform Role Codes", Value = "Platform Role Codes", Description = "Códigos de roles del panel platform",     OrderIndex = 3 },
                new() { Code = "USER_STATUS",         Name = "User Status",         Value = "User Status",         Description = "Estados posibles de un usuario",          OrderIndex = 6 },
                new() { Code = "RESOURCE_SCOPES",     Name = "Resource Scopes",     Value = "Resource Scopes",     Description = "Ámbito de aplicación de un recurso",      OrderIndex = 7 },

                // ── Permisos ──────────────────────────────────────────────────────────
                new() { ParentCode = "PERMISSIONS", Code = AppConstants.Permissions.Read,   Name = "Read",   Value = "Leer",       Description = "Permite visualizar recursos",           OrderIndex = 1 },
                new() { ParentCode = "PERMISSIONS", Code = AppConstants.Permissions.Create, Name = "Create", Value = "Crear",      Description = "Permite crear nuevos registros",        OrderIndex = 2 },
                new() { ParentCode = "PERMISSIONS", Code = AppConstants.Permissions.Update, Name = "Update", Value = "Actualizar", Description = "Permite modificar registros existentes",OrderIndex = 3 },
                new() { ParentCode = "PERMISSIONS", Code = AppConstants.Permissions.Delete, Name = "Delete", Value = "Eliminar",   Description = "Permite eliminar registros",            OrderIndex = 4 },

                // ── Tipos de usuario ──────────────────────────────────────────────────
                new() { ParentCode = "USER_TYPES", Code = AppConstants.UserType.Platform, Name = "Platform", Value = "Platform", Description = "Administrador de la plataforma", OrderIndex = 1 },

                // ── Códigos de rol platform ───────────────────────────────────────────
                new() { ParentCode = "PLATFORM_ROLE_CODES", Code = "PLATFORM_ROLE_CODES_SUPER_ADMIN", Name = "Super Admin", Value = "Super Admin", Description = "Rol dueño del sistema",        OrderIndex = 1 },
                new() { ParentCode = "PLATFORM_ROLE_CODES", Code = "APRENDIZ",                        Name = "Aprendiz",    Value = "Aprendiz",    Description = "Usuario final de la app cultural", OrderIndex = 2 },

                // ── Estados de usuario ────────────────────────────────────────────────
                new() { ParentCode = "USER_STATUS", Code = AppConstants.UserStatus.Active,    Name = "Active",    Value = "Activo",    Description = "Usuario activo",             OrderIndex = 1 },
                new() { ParentCode = "USER_STATUS", Code = AppConstants.UserStatus.Inactive,  Name = "Inactive",  Value = "Inactivo",  Description = "Usuario inactivo",           OrderIndex = 2 },
                new() { ParentCode = "USER_STATUS", Code = AppConstants.UserStatus.Suspended, Name = "Suspended", Value = "Suspendido", Description = "Usuario suspendido",        OrderIndex = 3 },

                // ── Ámbito de recursos ────────────────────────────────────────────────
                new() { ParentCode = "RESOURCE_SCOPES", Code = AppConstants.ResourceScope.Platform, Name = "Platform", Value = "Platform", Description = "Recurso del panel de administración", OrderIndex = 1 },

                // ── Grupos raíz — cursos ──────────────────────────────────────────────
                new() { Code = "COURSE_VERSION_STATUS",   Name = "Course Version Status",   Value = "Course Version Status",   Description = "Estados de una versión de curso",    OrderIndex = 8 },
                new() { Code = "RETO_VERSION_STATUS",     Name = "Reto Version Status",     Value = "Reto Version Status",     Description = "Estados de una versión de reto",     OrderIndex = 9 },
                new() { Code = "QUESTION_RESPONSE_MODES", Name = "Question Response Modes", Value = "Question Response Modes", Description = "Modos de respuesta de una pregunta", OrderIndex = 10 },
                new() { Code = "FILE_TYPES",              Name = "File Types",              Value = "File Types",              Description = "Tipos de archivo del sistema",        OrderIndex = 11 },
                new() { Code = "QUESTION_TYPES",          Name = "Question Types",          Value = "Question Types",          Description = "Tipos de pregunta de un examen",     OrderIndex = 13 },
                new() { Code = "MEDIA_TYPES",              Name = "Media Types",             Value = "Media Types",             Description = "Tipos de recurso multimedia de biblioteca", OrderIndex = 14 },

                // ── Estados de versión de curso ───────────────────────────────────────
                new() { ParentCode = "COURSE_VERSION_STATUS", Code = AppConstants.CourseVersionStatus.Draft,     Name = "Draft",     Value = "Borrador",   Description = "Versión en edición, no publicada",   OrderIndex = 1 },
                new() { ParentCode = "COURSE_VERSION_STATUS", Code = AppConstants.CourseVersionStatus.Published, Name = "Published", Value = "Publicado",  Description = "Versión activa visible para alumnos", OrderIndex = 2 },
                new() { ParentCode = "COURSE_VERSION_STATUS", Code = AppConstants.CourseVersionStatus.Archived,  Name = "Archived",  Value = "Archivado",  Description = "Versión histórica sin uso activo",    OrderIndex = 3 },

                // ── Estados de versión de examen ──────────────────────────────────────
                new() { ParentCode = "RETO_VERSION_STATUS", Code = AppConstants.RetoVersionStatus.Draft,     Name = "Draft",     Value = "Borrador",   Description = "Reto en edición, no publicado",    OrderIndex = 1 },
                new() { ParentCode = "RETO_VERSION_STATUS", Code = AppConstants.RetoVersionStatus.Published, Name = "Published", Value = "Publicado",  Description = "Reto activo visible para alumnos", OrderIndex = 2 },
                new() { ParentCode = "RETO_VERSION_STATUS", Code = AppConstants.RetoVersionStatus.Archived,  Name = "Archived",  Value = "Archivado",  Description = "Reto histórico sin uso activo",    OrderIndex = 3 },

                // ── Modos de respuesta de pregunta ────────────────────────────────────
                new() { ParentCode = "QUESTION_RESPONSE_MODES", Code = AppConstants.QuestionResponseMode.Single,   Name = "Single",   Value = "Única",    Description = "Solo una respuesta correcta",     OrderIndex = 1 },
                new() { ParentCode = "QUESTION_RESPONSE_MODES", Code = AppConstants.QuestionResponseMode.Multiple, Name = "Multiple", Value = "Múltiple", Description = "Varias respuestas correctas",     OrderIndex = 2 },

                // ── Tipos de archivo ──────────────────────────────────────────────────
                new() { ParentCode = "FILE_TYPES", Code = AppConstants.FileTypeCode.Image,    Name = "Image",    Value = "Imagen",    Description = "Archivos de imagen (jpg, png, etc.)", OrderIndex = 1 },
                new() { ParentCode = "FILE_TYPES", Code = AppConstants.FileTypeCode.Video,    Name = "Video",    Value = "Video",     Description = "Archivos de video (mp4, etc.)",       OrderIndex = 2 },
                new() { ParentCode = "FILE_TYPES", Code = AppConstants.FileTypeCode.Document, Name = "Document", Value = "Documento", Description = "Archivos de documento (pdf, etc.)",   OrderIndex = 3 },

                // ── Tipos de pregunta ─────────────────────────────────────────────────
                new() { ParentCode = "QUESTION_TYPES", Code = AppConstants.QuestionTypeCode.SingleChoice,   Name = "Opción única",      Value = "Opción única",      Description = "Una sola respuesta correcta entre varias opciones",       OrderIndex = 1 },
                new() { ParentCode = "QUESTION_TYPES", Code = AppConstants.QuestionTypeCode.MultipleChoice, Name = "Opción múltiple",   Value = "Opción múltiple",   Description = "Varias respuestas correctas entre varias opciones",       OrderIndex = 2 },
                new() { ParentCode = "QUESTION_TYPES", Code = AppConstants.QuestionTypeCode.TrueFalse,      Name = "Verdadero / Falso", Value = "Verdadero / Falso", Description = "El alumno indica si el enunciado es verdadero o falso",   OrderIndex = 3 },
                new() { ParentCode = "QUESTION_TYPES", Code = AppConstants.QuestionTypeCode.FillBlank,      Name = "Completar espacio", Value = "Completar espacio", Description = "El alumno escribe la respuesta en uno o varios espacios", OrderIndex = 4 },

                // ── Tipos de recurso de biblioteca ────────────────────────────────────
                new() { ParentCode = "MEDIA_TYPES", Code = AppConstants.MediaTypeCode.Video,   Name = "Video",   Value = "Video",   Description = "Video embebido desde URL externa",  OrderIndex = 1 },
                new() { ParentCode = "MEDIA_TYPES", Code = AppConstants.MediaTypeCode.Musica,  Name = "Música",  Value = "Música",  Description = "Audio embebido desde URL externa",  OrderIndex = 2 },
                new() { ParentCode = "MEDIA_TYPES", Code = AppConstants.MediaTypeCode.Foto,    Name = "Foto",    Value = "Foto",    Description = "Fotografía subida como archivo",    OrderIndex = 3 },
                new() { ParentCode = "MEDIA_TYPES", Code = AppConstants.MediaTypeCode.Leyenda, Name = "Leyenda", Value = "Leyenda", Description = "Texto enriquecido (leyenda/relato)", OrderIndex = 4 },
            };

            // Master codes primero — los roles referencian sus codes vía FK
            await _context.MasterCodes.AddRangeAsync(masterCodes, ct);
            await _context.SaveChangesAsync(ct);

            var roles = new List<PlatformRole>
            {
                new() { Name = "SuperAdmin", RoleCode = "PLATFORM_ROLE_CODES_SUPER_ADMIN", Description = "Rol dueño del sistema" },
                new() { Name = "Aprendiz",   RoleCode = "APRENDIZ",                        Description = "Usuario final que consume el contenido cultural" }
            };

            await _context.PlatformRoles.AddRangeAsync(roles, ct);
            await _context.SaveChangesAsync(ct);
        }

        private async Task SeedPermissionsAsync(CancellationToken ct)
        {
            if (await _context.Permissions.AnyAsync(ct))
                return;

            _logger.LogInformation("Seeding permissions...");

            await using var transaction = await _context.Database.BeginTransactionAsync(ct);
            try
            {
                // ── 1. RESOURCES ──────────────────────────────────────────────────────
                var rDashboard       = Res("dashboard",        "Dashboard",           AppConstants.ResourceScope.Platform);
                var rCursosPlantilla = Res("cursos_plantilla", "Cursos Plantilla",    AppConstants.ResourceScope.Platform);
                var rUsuariosApoyo   = Res("usuarios_apoyo",   "Usuarios de Apoyo",   AppConstants.ResourceScope.Platform);
                var rRolesApoyo      = Res("roles_apoyo",      "Roles de Apoyo",      AppConstants.ResourceScope.Platform);
                var rInsignias       = Res("insignias",        "Insignias",           AppConstants.ResourceScope.Platform);
                var rBiblioteca      = Res("biblioteca",       "Biblioteca",          AppConstants.ResourceScope.Platform);
                var rCalendario      = Res("calendario",       "Calendario",          AppConstants.ResourceScope.Platform);
                var rRegionDestacada = Res("region_destacada", "Región Destacada",    AppConstants.ResourceScope.Platform);
                var rNoticias        = Res("noticias",         "Noticias",            AppConstants.ResourceScope.Platform);
                var rPredisenos      = Res("predisenos",       "Prediseños",          AppConstants.ResourceScope.Platform);
                var rPremium         = Res("premium",          "Premium",             AppConstants.ResourceScope.Platform);

                // ── App del Aprendiz (un recurso por sección) ─────────────────────────
                var rAprendizPerfil       = Res("aprendiz_perfil",       "Aprendiz - Perfil",       AppConstants.ResourceScope.Platform);
                var rAprendizRegiones     = Res("aprendiz_regiones",     "Aprendiz - Regiones",     AppConstants.ResourceScope.Platform);
                var rAprendizRetos        = Res("aprendiz_retos",        "Aprendiz - Retos",        AppConstants.ResourceScope.Platform);
                var rAprendizInsignias    = Res("aprendiz_insignias",    "Aprendiz - Insignias",    AppConstants.ResourceScope.Platform);
                var rAprendizCertificados = Res("aprendiz_certificados", "Aprendiz - Certificados", AppConstants.ResourceScope.Platform);
                var rAprendizCalendario   = Res("aprendiz_calendario",   "Aprendiz - Calendario",   AppConstants.ResourceScope.Platform);
                var rAprendizBiblioteca   = Res("aprendiz_biblioteca",   "Aprendiz - Biblioteca",   AppConstants.ResourceScope.Platform);
                var rAprendizComunidad    = Res("aprendiz_comunidad",    "Aprendiz - Comunidad",    AppConstants.ResourceScope.Platform);
                var rAprendizPremium      = Res("aprendiz_premium",      "Aprendiz - Premium",      AppConstants.ResourceScope.Platform);
                var rAprendizNoticias     = Res("aprendiz_noticias",     "Aprendiz - Noticias",     AppConstants.ResourceScope.Platform);
                var rAprendizRegionDestacada = Res("aprendiz_region_destacada", "Aprendiz - Región Destacada", AppConstants.ResourceScope.Platform);

                await _context.Resources.AddRangeAsync([
                    rDashboard, rCursosPlantilla, rUsuariosApoyo, rRolesApoyo,
                    rInsignias, rBiblioteca, rCalendario, rRegionDestacada, rNoticias, rPredisenos, rPremium,
                    rAprendizPerfil, rAprendizRegiones, rAprendizRetos, rAprendizInsignias,
                    rAprendizCertificados, rAprendizCalendario, rAprendizBiblioteca,
                    rAprendizComunidad, rAprendizPremium, rAprendizNoticias, rAprendizRegionDestacada
                ], ct);
                await _context.SaveChangesAsync(ct);

                // ── 2. PERMISSIONS ────────────────────────────────────────────────────

                // Panel Platform — SuperAdmin hereda estos
                var platformPerms = new List<Permission>
                {
                    Perm(rDashboard, AppConstants.Permissions.Read),

                    Perm(rCursosPlantilla, AppConstants.Permissions.Read),
                    Perm(rCursosPlantilla, AppConstants.Permissions.Create),
                    Perm(rCursosPlantilla, AppConstants.Permissions.Update),
                    Perm(rCursosPlantilla, AppConstants.Permissions.Delete),

                    Perm(rUsuariosApoyo, AppConstants.Permissions.Read),
                    Perm(rUsuariosApoyo, AppConstants.Permissions.Create),
                    Perm(rUsuariosApoyo, AppConstants.Permissions.Update),
                    Perm(rUsuariosApoyo, AppConstants.Permissions.Delete),

                    Perm(rRolesApoyo, AppConstants.Permissions.Read),
                    Perm(rRolesApoyo, AppConstants.Permissions.Create),
                    Perm(rRolesApoyo, AppConstants.Permissions.Update),
                    Perm(rRolesApoyo, AppConstants.Permissions.Delete),

                    Perm(rInsignias, AppConstants.Permissions.Read),
                    Perm(rInsignias, AppConstants.Permissions.Create),
                    Perm(rInsignias, AppConstants.Permissions.Update),
                    Perm(rInsignias, AppConstants.Permissions.Delete),

                    Perm(rBiblioteca, AppConstants.Permissions.Read),
                    Perm(rBiblioteca, AppConstants.Permissions.Create),
                    Perm(rBiblioteca, AppConstants.Permissions.Update),
                    Perm(rBiblioteca, AppConstants.Permissions.Delete),

                    Perm(rCalendario, AppConstants.Permissions.Read),
                    Perm(rCalendario, AppConstants.Permissions.Create),
                    Perm(rCalendario, AppConstants.Permissions.Update),
                    Perm(rCalendario, AppConstants.Permissions.Delete),

                    Perm(rRegionDestacada, AppConstants.Permissions.Read),
                    Perm(rRegionDestacada, AppConstants.Permissions.Create),
                    Perm(rRegionDestacada, AppConstants.Permissions.Update),
                    Perm(rRegionDestacada, AppConstants.Permissions.Delete),

                    Perm(rNoticias, AppConstants.Permissions.Read),
                    Perm(rNoticias, AppConstants.Permissions.Create),
                    Perm(rNoticias, AppConstants.Permissions.Update),
                    Perm(rNoticias, AppConstants.Permissions.Delete),

                    Perm(rPredisenos, AppConstants.Permissions.Read),
                    Perm(rPredisenos, AppConstants.Permissions.Create),
                    Perm(rPredisenos, AppConstants.Permissions.Update),
                    Perm(rPredisenos, AppConstants.Permissions.Delete),

                    Perm(rPremium, AppConstants.Permissions.Read),
                    Perm(rPremium, AppConstants.Permissions.Create),
                    Perm(rPremium, AppConstants.Permissions.Update),
                    Perm(rPremium, AppConstants.Permissions.Delete),

                    Perm(rAprendizPerfil, AppConstants.Permissions.Read),
                    Perm(rAprendizRegiones, AppConstants.Permissions.Read),
                    Perm(rAprendizRetos, AppConstants.Permissions.Read),
                    Perm(rAprendizInsignias, AppConstants.Permissions.Read),
                    Perm(rAprendizCertificados, AppConstants.Permissions.Read),
                    Perm(rAprendizCalendario, AppConstants.Permissions.Read),
                    Perm(rAprendizBiblioteca, AppConstants.Permissions.Read),
                    Perm(rAprendizComunidad, AppConstants.Permissions.Read),
                    Perm(rAprendizPremium, AppConstants.Permissions.Read),
                    Perm(rAprendizNoticias, AppConstants.Permissions.Read),
                    Perm(rAprendizRegionDestacada, AppConstants.Permissions.Read),
                };

                await _context.Permissions.AddRangeAsync(platformPerms, ct);
                await _context.SaveChangesAsync(ct);

                // ── 3. SUPER ADMIN — solo permisos del panel Platform ─────────────────
                var superAdminRole = await _context.PlatformRoles
                    .FirstAsync(r => r.RoleCode == "PLATFORM_ROLE_CODES_SUPER_ADMIN", ct);

                var superAdminRolePerms = platformPerms.Select(p => new PlatformRolePermission
                {
                    PlatformRoleId = superAdminRole.Id,
                    PermissionId   = p.Id,
                    CreatedAt      = DateTime.UtcNow,
                    CreatedBy      = 0
                });

                await _context.PlatformRolePermissions.AddRangeAsync(superAdminRolePerms, ct);
                await _context.SaveChangesAsync(ct);

                // ── 4. APRENDIZ — mismos permisos del panel Platform ──────────────────
                var aprendizRole = await _context.PlatformRoles
                    .FirstAsync(r => r.RoleCode == "APRENDIZ", ct);

                var aprendizRolePerms = platformPerms.Select(p => new PlatformRolePermission
                {
                    PlatformRoleId = aprendizRole.Id,
                    PermissionId   = p.Id,
                    CreatedAt      = DateTime.UtcNow,
                    CreatedBy      = 0
                });

                await _context.PlatformRolePermissions.AddRangeAsync(aprendizRolePerms, ct);
                await _context.SaveChangesAsync(ct);

                await transaction.CommitAsync(ct);
                _logger.LogInformation("Permissions seeded successfully.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(ct);
                _logger.LogError(ex, "Error seeding permissions. Transaction rolled back.");
                throw;
            }
        }

        private async Task SeedAdminUserAsync(CancellationToken ct)
        {
            if (await _context.Users.AnyAsync(ct))
                return;

            _logger.LogInformation("Seeding admin user...");

            var superAdminRole = await _context.PlatformRoles
                .FirstAsync(x => x.RoleCode == "PLATFORM_ROLE_CODES_SUPER_ADMIN", ct);

            await using var transaction = await _context.Database.BeginTransactionAsync(ct);
            try
            {
                var person = new Person
                {
                    FirstName      = "Super",
                    LastName       = "Admin",
                    DocumentType   = "DNI",
                    DocumentNumber = "00000000",
                    CreatedBy      = 0
                };

                await _context.Persons.AddAsync(person, ct);
                await _context.SaveChangesAsync(ct);

                var user = new User
                {
                    PersonId     = person.Id,
                    UserTypeCode = AppConstants.UserType.Platform,
                    Username     = "superadmin@gmail.com",
                    Password     = _passwordHasher.Hash("Admin@1234!"),
                    IsLocked     = false,
                    RoleId       = superAdminRole.Id,
                    CreatedBy    = 0
                };

                await _context.Users.AddAsync(user, ct);
                await _context.SaveChangesAsync(ct);

                await transaction.CommitAsync(ct);
                _logger.LogInformation("Admin user created. Username: superadmin | Password: Admin@1234!");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(ct);
                _logger.LogError(ex, "Error seeding admin user. Transaction rolled back.");
                throw;
            }
        }

        private async Task SeedRegionsAsync(CancellationToken ct)
        {
            if (await _context.Courses.AnyAsync(ct))
                return;

            _logger.LogInformation("Seeding regions...");

            string[] regionNames =
            [
                "Amazonas", "Áncash", "Apurímac", "Arequipa", "Ayacucho",
                "Cajamarca", "Callao", "Cusco", "Huancavelica", "Huánuco",
                "Ica", "Junín", "La Libertad", "Lambayeque", "Lima",
                "Loreto", "Madre de Dios", "Moquegua", "Pasco", "Piura",
                "Puno", "San Martín", "Tacna", "Tumbes", "Ucayali"
            ];

            var regions = regionNames.Select(name => new Course
            {
                Title       = name,
                IsActive    = true,
                CreatedAt   = DateTime.UtcNow,
                CreatedBy   = 0
            }).ToList();

            await _context.Courses.AddRangeAsync(regions, ct);
            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("Regions seeded successfully ({Count} regions).", regions.Count);
        }

        private async Task SeedCommunityPostsAsync(CancellationToken ct)
        {
            if (await _context.CommunityPosts.AnyAsync(ct))
                return;

            _logger.LogInformation("Seeding community posts...");

            var regions = await _context.Courses.OrderBy(c => c.Id).Take(4).ToListAsync(ct);

            var posts = new List<CommunityPost>
            {
                new()
                {
                    AuthorName = "María Quispe",
                    RegionId = regions.ElementAtOrDefault(0)?.Id,
                    Text = "¡Me encantó aprender sobre los tejidos tradicionales! La técnica del telar de cintura es fascinante.",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = 0
                },
                new()
                {
                    AuthorName = "Jorge Huamán",
                    RegionId = regions.ElementAtOrDefault(1)?.Id,
                    Text = "Terminé el reto de gastronomía regional y aprendí muchísimo sobre nuestros platos típicos.",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = 0
                },
                new()
                {
                    AuthorName = "Lucía Torres",
                    RegionId = regions.ElementAtOrDefault(2)?.Id,
                    Text = "Las festividades regionales son impresionantes, no sabía tanto sobre nuestras tradiciones.",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = 0
                },
                new()
                {
                    AuthorName = "Carlos Mamani",
                    RegionId = regions.ElementAtOrDefault(3)?.Id,
                    Text = "¡Gané mi primera insignia! Muy motivador seguir explorando la cultura peruana.",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = 0
                }
            };

            await _context.CommunityPosts.AddRangeAsync(posts, ct);
            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("Community posts seeded successfully ({Count} posts).", posts.Count);
        }

        // ── Helpers ───────────────────────────────────────────────────────────────────

        private static Resource Res(string code, string name, string scope = AppConstants.ResourceScope.Platform) => new()
        {
            Code  = code,
            Name  = name,
            Scope = scope
        };

        private static Permission Perm(Resource resource, string permissionCode) => new()
        {
            ResourceId     = resource.Id,
            PermissionCode = permissionCode,
            CreatedAt      = DateTime.UtcNow,
            CreatedBy      = 0
        };
    }
}
