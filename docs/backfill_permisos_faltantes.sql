-- Backfill de recursos/permisos que el seeder no insertó porque la BD ya tenía datos
-- cuando se agregaron (el seeder solo corre en una base vacía). Idempotente: se puede
-- correr las veces que haga falta, solo inserta lo que falte.

SET NOCOUNT ON;

DECLARE @Resources TABLE (Code NVARCHAR(100), Name NVARCHAR(200));
INSERT INTO @Resources (Code, Name) VALUES
    ('dashboard',                'Dashboard'),
    ('cursos_plantilla',         'Cursos Plantilla'),
    ('usuarios_apoyo',           'Usuarios de Apoyo'),
    ('roles_apoyo',              'Roles de Apoyo'),
    ('insignias',                'Insignias'),
    ('biblioteca',               'Biblioteca'),
    ('calendario',               'Calendario'),
    ('region_destacada',         'Región Destacada'),
    ('noticias',                 'Noticias'),
    ('predisenos',               'Prediseños'),
    ('aprendiz_perfil',          'Aprendiz - Perfil'),
    ('aprendiz_regiones',        'Aprendiz - Regiones'),
    ('aprendiz_retos',           'Aprendiz - Retos'),
    ('aprendiz_insignias',       'Aprendiz - Insignias'),
    ('aprendiz_certificados',    'Aprendiz - Certificados'),
    ('aprendiz_calendario',      'Aprendiz - Calendario'),
    ('aprendiz_biblioteca',      'Aprendiz - Biblioteca'),
    ('aprendiz_comunidad',       'Aprendiz - Comunidad'),
    ('aprendiz_premium',         'Aprendiz - Premium'),
    ('aprendiz_noticias',        'Aprendiz - Noticias'),
    ('aprendiz_region_destacada','Aprendiz - Región Destacada');

-- 1) Recursos faltantes
--    OJO: access.resources NO hereda de BaseEntity -> solo tiene Id, Code, Name, Scope
--    (sin CreatedAt/CreatedBy/Deleted, a diferencia de casi todas las demás tablas).
INSERT INTO access.resources (Code, Name, Scope)
SELECT r.Code, r.Name, 'RESOURCE_SCOPES_PLATFORM'
FROM @Resources r
WHERE NOT EXISTS (SELECT 1 FROM access.resources x WHERE x.Code = r.Code);

-- 2) Permisos faltantes. Los recursos aprendiz_* solo llevan READ;
--    el resto lleva las 4 acciones (Read/Create/Update/Delete).
DECLARE @Actions TABLE (Code NVARCHAR(50));
INSERT INTO @Actions (Code) VALUES
    ('PERMISSIONS_READ'), ('PERMISSIONS_CREATE'), ('PERMISSIONS_UPDATE'), ('PERMISSIONS_DELETE');

INSERT INTO access.permissions (ResourceId, PermissionCode, CreatedAt, CreatedBy, Deleted)
SELECT res.Id, a.Code, SYSUTCDATETIME(), 0, 0
FROM @Resources r
JOIN access.resources res ON res.Code = r.Code
CROSS JOIN @Actions a
WHERE (
        (r.Code LIKE 'aprendiz\_%' ESCAPE '\' AND a.Code = 'PERMISSIONS_READ')
        OR (r.Code NOT LIKE 'aprendiz\_%' ESCAPE '\')
      )
  AND NOT EXISTS (
        SELECT 1 FROM access.permissions p
        WHERE p.ResourceId = res.Id AND p.PermissionCode = a.Code
      );

-- 3) Asignar todos esos permisos a SuperAdmin y a APRENDIZ
--    (mismo criterio que ya usa DbSeeder.cs: ambos roles reciben el mismo set completo).
DECLARE @RoleIds TABLE (Id INT);
INSERT INTO @RoleIds (Id)
SELECT Id FROM access.platform_roles
WHERE RoleCode IN ('PLATFORM_ROLE_CODES_SUPER_ADMIN', 'APRENDIZ');

INSERT INTO access.platform_role_permissions (PlatformRoleId, PermissionId, CreatedAt, CreatedBy, Deleted)
SELECT ro.Id, p.Id, SYSUTCDATETIME(), 0, 0
FROM @RoleIds ro
CROSS JOIN access.permissions p
JOIN access.resources res ON res.Id = p.ResourceId
JOIN @Resources r ON r.Code = res.Code
WHERE NOT EXISTS (
    SELECT 1 FROM access.platform_role_permissions rp
    WHERE rp.PlatformRoleId = ro.Id AND rp.PermissionId = p.Id
);

PRINT 'Backfill de permisos completado.';
