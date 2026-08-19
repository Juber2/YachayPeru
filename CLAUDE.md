# YachayPeru — guía para Claude Code

## Qué es este proyecto

Backend de **Yachay Perú**, una plataforma cultural: expone contenido educativo sobre las regiones del Perú (regiones, "retos" tipo quiz, insignias, certificados, calendario de festividades, biblioteca multimedia, comunidad, plan premium) tanto para un panel de administración/gestión de contenido como para una app de usuario final ("Aprendiz").

Este backend nació de un pivote: originalmente era **"CapaActivaCorp"**, un SaaS multi-tenant de capacitación corporativa. Ese modelo de tenants/empresas fue eliminado por completo. **No menciones CapaActivaCorp, tenants ni empresas al hablar de este proyecto** — es legado eliminado, no una feature. Si encuentras residuos de ese nombre (p. ej. `*.csproj.Backup.tmp` sueltos en `YachayPeru.Application/` o `YachayPeru.Infrastructure/`), son basura de la migración de nombre, no forman parte del build; no los edites, se pueden borrar si estorban.

## Stack

- **.NET 10**, ASP.NET Core Web API.
- **EF Core 10 + SQL Server** como único proveedor de persistencia.
- **MediatR** para CQRS-lite (ver convenciones abajo).
- **Riok.Mapperly** (source-generated mapping, *no* AutoMapper) para mapear Request → Command en el API cuando el mapeo es trivial (`YachayPeru.API/Mappings/AppMapper.cs`).
- **QuestPDF** (licencia Community) para generar certificados en PDF.
- **QRCoder** (MIT) para los QR de verificación de certificados.

## Arquitectura (Clean Architecture)

```
YachayPeru.Domain          Entidades, enums, constantes (AppConstants), BaseEntity
YachayPeru.Application     Casos de uso: Features (CQRS), Abstractions (interfaces de repos/servicios),
                            Actions (lógica de negocio reutilizable no atada a un solo handler), Common (Result, helpers)
YachayPeru.Infrastructure  EF Core (DbContext, Configurations, Migrations, Repositories), servicios concretos, DbSeeder
YachayPeru.API             Controllers, Contracts (Request/Response), Authorization, Middleware, Program.cs
YachayPeru.Transversal     Helpers y mapeos que no dependen de una capa específica
```

Cada bounded context tiene su propio **schema de base de datos**: `course`, `assessment`, `content`, `aprendiz`, `access`, `auth`.

## Convenciones de código (seguir siempre, no improvisar variantes)

- **CQRS vía MediatR**: `IRequest<T>` / `IRequestHandler<TRequest, TResponse>`. Handlers delgados: validan, llaman a repos/Actions, devuelven `Result` / `Result<T>`.
- **`ITransactionalCommand<T>`**: marcador que engancha con `TransactionBehavior` (es una red de seguridad de rollback, no el mecanismo que abre la transacción).
- **`Result` / `Result<T>`** (`YachayPeru.Application/Common/Results/`) con `ResultCodes` (`NotFound`, `Conflict`, `Validation`, etc.). En el controller, `this.FromResult(result)` (extensión en `YachayPeru.API/Extensions/`) lo traduce a HTTP status.
- **`ApiResponse<T>`** (`Success`, `Message`, `Data`, `Errors`) es el sobre de respuesta de *todos* los endpoints exitosos.
- **Repositorios**: cada entidad tiene `I{X}Repository : IRepository<X>` en `Application/Abstractions/Persistence/{Contexto}/`, implementado en `Infrastructure/Persistence/Repositories/`. `IRepository<T>` extiende `IReadRepository<T>` (`GetByIdAsync`, `ListAsync()`, `ListAsync(predicate)`, `FirstOrDefaultAsync`, `SingleOrDefaultAsync`, `AnyAsync`, `CountAsync`) y añade `AddAsync`, `AddRangeAsync`, `Update`, `Delete`, `DeleteRange`. Si necesitas una query especial, agrégala al repo específico — **no** hay soporte de `OrderBy`/paginación en la interfaz genérica, se ordena en memoria en el handler si hace falta.
- **Soft delete**: `BaseEntity` tiene `Deleted` (bool) + `CreatedAt/CreatedBy/UpdatedAt/UpdatedBy`. Cada método de cada repositorio filtra `!x.Deleted` manualmente (no hay query filter global de EF) — replica ese patrón en repos nuevos.
- **`IUnitOfWork.SaveChangesAsync(ct)`** se llama explícitamente al final de cada handler de comando que muta datos.
- **Auth por JWT con claims custom**: `"permission"` (formato `resource:ACTION`) y `"roles"` (valor = `RoleCode`) — **no** son los claims estándar de ASP.NET (`ClaimTypes.Role`). Autorización **siempre por permiso** (no por rol) vía `PermissionPolicyProvider`/`PermissionAuthorizationHandler`/`AnyPermissionAuthorizationHandler` — mismo mecanismo para el panel admin y para la app del Aprendiz. Cada sección del Aprendiz tiene su propio recurso/permiso (`aprendiz_perfil`, `aprendiz_regiones`, `aprendiz_retos`, `aprendiz_insignias`, `aprendiz_certificados`, `aprendiz_calendario`, `aprendiz_biblioteca`, `aprendiz_comunidad`, `aprendiz_premium`), sembrado en `DbSeeder.cs` y asignado al rol `APRENDIZ` (junto con `SuperAdmin`, que hereda todos los permisos del sistema). El mecanismo de `RoleRequirement`/`RoleAuthorizationHandler`/policy `"AprendizOnly"` que existió brevemente para esto **se eliminó** — no lo reintroduzcas.
- **`Program.cs` → `AddJwtBearer`: `options.MapInboundClaims = false;` es obligatorio.** Sin esto, ASP.NET Core remapea `"sub"` a `ClaimTypes.NameIdentifier` (y otros claims cortos) automáticamente, y `ICurrentUser.Id` (que busca `"sub"` literal) siempre devuelve `0`. Ya está seteado — si algún día no aparece, es un bug real, no una opción a sacar.
- **`ICurrentUser.Id` = `User.Id`, no hay tabla de unión de roles.** `User.RoleId` (nullable int) es la única relación usuario↔rol — un usuario tiene como máximo un rol. No existe `UserPlatformRole`; se colapsó en ese campo escalar. No la reintroduzcas si ves código viejo/docs que la mencionen.
- **Archivos**: `IFileStorageService` (disco local bajo `wwwroot`, folder-scoped) para `SaveAsync`/`Delete`/`ReadAsync`. Los controllers arman URLs absolutas con `{Request.Scheme}://{Request.Host}{relativeUrl}` antes de devolverlas.
- **Calificación de exámenes/retos**: siempre server-side. La respuesta correcta (`IsCorrect`, `CorrectAnswer`) nunca se expone en el payload de la pregunta antes de responder, solo en el resultado del intento.

## Migraciones EF — política explícita

**Nunca migraciones incrementales.** Cada vez que cambia el schema, se regenera `InitialCreate` desde cero.

**Claude NUNCA ejecuta comandos `dotnet ef` en este proyecto.** Ni `migrations add`, ni `migrations remove`, ni `database update` — nada que toque migraciones o la base de datos. El servidor real corre casi siempre en la máquina del usuario mientras se trabaja (`dotnet run`/Visual Studio), y correr `dotnet ef` desde acá bloquea/corrompe los `.dll` de salida por el archivo en uso, además de que el usuario prefiere aplicar las migraciones él mismo. En vez de ejecutar nada, Claude debe:

1. Hacer todos los cambios de código/entidades/EF config necesarios.
2. Verificar con `dotnet build YachayPeru.sln -v quiet` que compila.
3. Darle al usuario, en texto, los dos comandos exactos para que los corra él:

```bash
dotnet ef migrations remove --project YachayPeru.Infrastructure --startup-project YachayPeru.API --force
dotnet ef migrations add InitialCreate --project YachayPeru.Infrastructure --startup-project YachayPeru.API
```

No hay acceso a un SQL Server real desde este entorno de desarrollo (`appsettings.json` apunta a producción) — la verificación de Claude es siempre por `dotnet build`, nunca `dotnet ef` ni pruebas end-to-end contra un servidor corriendo.

## Seeding

Todo el sembrado vive en `YachayPeru.Infrastructure/Persistence/SqlServer/Seed/DbSeeder.cs`, orquestado desde `SeedAsync`. **Nunca borres ni reordenes bloques de seed existentes al agregar uno nuevo** — cada bloque es idempotente (`if (await _context.X.AnyAsync(ct)) return;`) y se agrega como un método `SeedXAsync` nuevo, llamado al final de `SeedAsync`.

## Dos superficies de la API

1. **Panel de gestión** (`Controllers/Administration/*`, rutas `administration/...`): CRUD de contenido, protegido por permisos granulares (`AppPermissions.*`). Antes se llamaba "Platform" (`platform/...`, `PlatformCoursesController`, `Features/PlatformCourses` etc.) — se renombró a "Administration" en todo el stack (rutas, carpetas, namespaces, nombres de controller como `CoursesController`/`RoleController`/`UsersController`). Si ves "Platform" en el código hoy, es porque quedó intencionalmente en la capa de dominio/persistencia (`PlatformRole`, `PlatformRolePermission`, tabla `access.platform_roles`, y códigos seed como `PLATFORM_ROLE_CODES`) — eso no se tocó porque son nombres de tabla/datos persistidos, no parte de la superficie de rutas/controllers.
2. **App del Aprendiz** (`Controllers/Aprendiz/*`, rutas `aprendiz/...`): consumo por el usuario final, protegido por la policy `"AprendizOnly"`, usa `ICurrentUser.Id` inyectado en vez de un parámetro de ruta. Cubre: perfil, regiones, retos (con calificación e intentos), insignias, certificados (incluye descarga de PDF), calendario (festividades + recordatorios), biblioteca multimedia, comunidad (posts + likes) y premium (plan + lista de espera).

## `Course` = región cultural, sin distinción de "plantilla"

Cada fila de la tabla `Course` (schema `course`) es una región cultural del Perú — no existe otro tipo de fila en esa tabla. El campo `IsTemplate` que existía antes fue eliminado (era un discriminador redundante, ya que el 100% de las filas cumplían `IsTemplate = true`); el único filtro relevante hoy es `IsActive`. Las rutas de administración de regiones son `administration/courses`, `administration/courses/{id}`, etc. (sin el segmento `/templates` que existía antes — se quitó por confuso).

## Verificación después de cualquier cambio

```bash
dotnet build YachayPeru.sln -v quiet
```

Sin acceso a base de datos ni cliente HTTP real desde este entorno: la verificación es siempre build limpio + revisión estática de código/rutas, nunca pruebas end-to-end.
