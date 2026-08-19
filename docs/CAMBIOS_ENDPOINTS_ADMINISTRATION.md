# Cambios de endpoints — rename "Platform" → "Administration"

Este documento detalla todos los endpoints del panel de gestión que cambiaron de ruta/nombre, y el contrato (request/response) de cada uno tras el cambio. Todos siguen protegidos por `[Authorize]` + políticas de permiso `AppPermissions.*` (sin cambios en la lógica de permisos, solo en rutas/nombres).

Prefijo base de todos: `administration/...` (antes `platform/...`).

---

## 1. Courses (antes `PlatformCoursesController`, ahora `CoursesController`)

Cambio adicional aquí: se quitó el segmento `/templates` de las rutas y el campo `IsTemplate` de las respuestas (ver [CLAUDE.md](../CLAUDE.md) para el porqué). Se agregó `CoverImageUrl` a ambas respuestas (campo real de la tabla que antes no se devolvía).

| Método | Ruta anterior | Ruta nueva |
|---|---|---|
| GET | `platform/courses/templates` | `administration/courses` |
| GET | `platform/courses/templates/{id}` | `administration/courses/{id}` |
| POST | `platform/courses/templates` | `administration/courses` |
| PUT | `platform/courses/templates/{id}` | `administration/courses/{id}` |
| DELETE | `platform/courses/templates/{id}` | `administration/courses/{id}` |
| POST | `platform/courses/templates/{id}/cover-image` | `administration/courses/{id}/cover-image` |

### GET `administration/courses`
Response (`ApiResponse<List<CourseListResponse>>`):
```json
{
  "id": 1,
  "title": "Cusco",
  "description": "string | null",
  "isActive": true,
  "coverImageUrl": "https://.../wwwroot/courses/xxx.jpg | null",
  "createdAt": "2026-01-01T00:00:00Z"
}
```

### GET `administration/courses/{id}`
Response (`ApiResponse<CourseDetailResponse>`):
```json
{
  "id": 1,
  "title": "Cusco",
  "description": "string | null",
  "isActive": true,
  "coverImageUrl": "https://.../wwwroot/courses/xxx.jpg | null",
  "sourceTemplateId": null,
  "createdAt": "2026-01-01T00:00:00Z"
}
```
*(`isTemplate` ya no existe en la respuesta.)*

### POST `administration/courses`
Request (`CreateCourseRequest`, antes `CreateTemplateRequest`):
```json
{
  "title": "string",
  "description": "string | null"
}
```
Response: `ApiResponse<int>` (Id de la región creada), vía `Result<int>`.

### PUT `administration/courses/{id}`
Request (`EditCourseRequest`, antes `EditTemplateInfoRequest`):
```json
{
  "title": "string",
  "description": "string | null",
  "isActive": true
}
```
Response: `ApiResponse<int>`.

### DELETE `administration/courses/{id}`
Sin body. Response: `ApiResponse` (soft-delete).

### POST `administration/courses/{id}/cover-image`
Request: `multipart/form-data` con campo `file`.
Response: `ApiResponse<string>` con la URL relativa guardada.

---

## 2. Roles (antes `PlatformRoleController`, ahora `RoleController`)

| Método | Ruta anterior | Ruta nueva |
|---|---|---|
| GET | `platform/roles` | `administration/roles` |
| GET | `platform/roles/lookup` | `administration/roles/lookup` |
| GET | `platform/roles/{id}` | `administration/roles/{id}` |
| GET | `platform/permissions/matrix` | `administration/permissions/matrix` |
| POST | `platform/roles` | `administration/roles` |
| PUT | `platform/roles/{id}` | `administration/roles/{id}` |
| DELETE | `platform/roles/{id}` | `administration/roles/{id}` |

*(Solo cambió el prefijo de ruta; los contratos de request/response no cambiaron de forma, solo internamente los nombres de Command/Query pasaron de `CreatePlatformRoleCommand`→`CreateRoleCommand`, `GetPlatformRoleListQuery`→`GetRoleListQuery`, etc.)*

### GET `administration/roles`
Response (`ApiResponse<List<PlatformRoleListDto>>`):
```json
{
  "id": 1,
  "name": "string",
  "roleCode": "string | null",
  "description": "string | null",
  "isForCompanies": false,
  "userCount": 3,
  "permissions": [{ "resourceName": "string", "actionValue": "string" }]
}
```

### GET `administration/roles/lookup`
Response (`ApiResponse<List<PlatformRoleLookupDto>>`):
```json
{ "id": 1, "name": "string", "isForCompanies": false }
```

### GET `administration/roles/{id}`
Response (`ApiResponse<PlatformRoleDetailDto>`):
```json
{
  "id": 1,
  "name": "string",
  "roleCode": "string | null",
  "description": "string | null",
  "isForCompanies": false,
  "permissionIds": [1, 2, 3]
}
```

### GET `administration/permissions/matrix`
Sin cambios de contrato (`GetPermissionsMatrixQuery`, no fue parte de este rename).

### POST `administration/roles`
Request (`CreateRoleRequest`, antes `CreatePlatformRoleRequest`):
```json
{
  "name": "string",
  "roleCode": "string | null",
  "description": "string | null",
  "isForCompanies": false,
  "permissionIds": [1, 2, 3]
}
```

### PUT `administration/roles/{id}`
Request (`EditRoleRequest`, antes `EditPlatformRoleRequest`): mismos campos que `CreateRoleRequest`.

### DELETE `administration/roles/{id}`
Sin body.

---

## 3. Users (antes `PlatformUsersController`, ahora `UsersController`)

| Método | Ruta anterior | Ruta nueva |
|---|---|---|
| GET | `platform/users` | `administration/users` |
| GET | `platform/users/{id}` | `administration/users/{id}` |
| POST | `platform/users` | `administration/users` |
| PUT | `platform/users/{id}` | `administration/users/{id}` |
| DELETE | `platform/users/{id}` | `administration/users/{id}` |

*(Solo cambió el prefijo de ruta y los nombres de Request: `PlatformCreateUserRequest`→`CreateUserRequest`, `PlatformEditUserRequest`→`EditUserRequest`.)*

### GET `administration/users`
Response (`ApiResponse<List<PlatformUserListItemDto>>`):
```json
{
  "id": 1,
  "fullName": "string",
  "email": "string | null",
  "isLocked": false,
  "roleName": "string | null",
  "roleCode": "string | null",
  "lastAccess": "2026-01-01T00:00:00Z | null"
}
```

### GET `administration/users/{id}`
Response (`ApiResponse<PlatformUserDetailDto>`):
```json
{
  "id": 1,
  "firstName": "string",
  "lastName": "string",
  "email": "string | null",
  "username": "string",
  "isLocked": false,
  "roleId": 1,
  "roleName": "string | null"
}
```

### POST `administration/users`
Request (`CreateUserRequest`, antes `PlatformCreateUserRequest`):
```json
{
  "firstName": "string",
  "lastName": "string",
  "userName": "string",
  "email": "string",
  "password": "string",
  "isActive": true,
  "sendWelcomeMessage": false,
  "roleId": 1,
  "reactivateUserId": null
}
```

### PUT `administration/users/{id}`
Request (`EditUserRequest`, antes `PlatformEditUserRequest`):
```json
{
  "firstName": "string",
  "lastName": "string",
  "userName": "string",
  "email": "string",
  "password": "string | null",
  "isLocked": false,
  "roleId": 1
}
```

### DELETE `administration/users/{id}`
Sin body.

---

## 4-8. Calendario, RegionDestacada, Biblioteca, Noticias, Insignias

Estos 5 controllers **no cambiaron de nombre de clase ni de contrato** — solo cambió el prefijo de ruta de `platform/...` a `administration/...`. Nada más se modificó (mismos Request/Response de siempre).

| Controller | Ruta anterior | Ruta nueva |
|---|---|---|
| `CalendarioController` | `platform/calendario/...` | `administration/calendario/...` |
| `RegionDestacadaController` | `platform/region-destacada/...` | `administration/region-destacada/...` |
| `BibliotecaController` | `platform/biblioteca/...` | `administration/biblioteca/...` |
| `NoticiasController` | `platform/noticias/...` | `administration/noticias/...` |
| `InsigniasController` | `platform/insignias/...` | `administration/insignias/...` |

Cada uno mantiene sus 5-6 endpoints CRUD habituales (`GET`, `GET/{id}`, `POST`, `PUT/{id}`, `DELETE/{id}`, y `POST/{id}/thumbnail|image` donde aplica) bajo el nuevo prefijo.

---

## Notas para el consumidor del API (frontend)

- **Todo el prefijo `platform/` debe reemplazarse por `administration/`** en el cliente HTTP.
- **Courses**: quitar `/templates` de todas las URLs armadas a mano; el campo `isTemplate` ya no viene en las respuestas — no debe leerse ni enviarse.
- El resto de los contratos (roles, users, calendario, biblioteca, noticias, insignias, region-destacada) **no cambiaron de forma**, solo de ruta.
