namespace YachayPeru.API.Authorization
{
    /// <summary>
    /// Constantes de permisos en formato "resource_code:PERMISSIONS_ACTION".
    /// Úsalas en [Authorize(Policy = AppPermissions.X.Read)].
    /// </summary>
    public static class AppPermissions
    {
        // ── Panel Platform ────────────────────────────────────────────────────────

        public static class Dashboard
        {
            public const string Read = "dashboard:PERMISSIONS_READ";
        }

        public static class CursosPlantilla
        {
            public const string Read   = "cursos_plantilla:PERMISSIONS_READ";
            public const string Create = "cursos_plantilla:PERMISSIONS_CREATE";
            public const string Update = "cursos_plantilla:PERMISSIONS_UPDATE";
            public const string Delete = "cursos_plantilla:PERMISSIONS_DELETE";
        }

        public static class UsuariosApoyo
        {
            public const string Read   = "usuarios_apoyo:PERMISSIONS_READ";
            public const string Create = "usuarios_apoyo:PERMISSIONS_CREATE";
            public const string Update = "usuarios_apoyo:PERMISSIONS_UPDATE";
            public const string Delete = "usuarios_apoyo:PERMISSIONS_DELETE";
        }

        public static class RolesApoyo
        {
            public const string Read   = "roles_apoyo:PERMISSIONS_READ";
            public const string Create = "roles_apoyo:PERMISSIONS_CREATE";
            public const string Update = "roles_apoyo:PERMISSIONS_UPDATE";
            public const string Delete = "roles_apoyo:PERMISSIONS_DELETE";
        }

        // ── Editor de cursos ──────────────────────────────────────────────────────
        public static class CourseEditor
        {
            public const string Read   = "course_editor_read";
            public const string Update = "course_editor_update";
        }

        // ── Módulos CMS ───────────────────────────────────────────────────────────
        public static class Insignias
        {
            public const string Read   = "insignias:PERMISSIONS_READ";
            public const string Create = "insignias:PERMISSIONS_CREATE";
            public const string Update = "insignias:PERMISSIONS_UPDATE";
            public const string Delete = "insignias:PERMISSIONS_DELETE";
        }

        public static class Biblioteca
        {
            public const string Read   = "biblioteca:PERMISSIONS_READ";
            public const string Create = "biblioteca:PERMISSIONS_CREATE";
            public const string Update = "biblioteca:PERMISSIONS_UPDATE";
            public const string Delete = "biblioteca:PERMISSIONS_DELETE";
        }

        public static class Calendario
        {
            public const string Read   = "calendario:PERMISSIONS_READ";
            public const string Create = "calendario:PERMISSIONS_CREATE";
            public const string Update = "calendario:PERMISSIONS_UPDATE";
            public const string Delete = "calendario:PERMISSIONS_DELETE";
        }

        public static class RegionDestacada
        {
            public const string Read   = "region_destacada:PERMISSIONS_READ";
            public const string Create = "region_destacada:PERMISSIONS_CREATE";
            public const string Update = "region_destacada:PERMISSIONS_UPDATE";
            public const string Delete = "region_destacada:PERMISSIONS_DELETE";
        }

        public static class Noticias
        {
            public const string Read   = "noticias:PERMISSIONS_READ";
            public const string Create = "noticias:PERMISSIONS_CREATE";
            public const string Update = "noticias:PERMISSIONS_UPDATE";
            public const string Delete = "noticias:PERMISSIONS_DELETE";
        }

        public static class Predisenos
        {
            public const string Read   = "predisenos:PERMISSIONS_READ";
            public const string Create = "predisenos:PERMISSIONS_CREATE";
            public const string Update = "predisenos:PERMISSIONS_UPDATE";
            public const string Delete = "predisenos:PERMISSIONS_DELETE";
        }

        public static class Premium
        {
            public const string Read   = "premium:PERMISSIONS_READ";
            public const string Create = "premium:PERMISSIONS_CREATE";
            public const string Update = "premium:PERMISSIONS_UPDATE";
            public const string Delete = "premium:PERMISSIONS_DELETE";
        }

        // ── App del Aprendiz (un recurso por sección) ─────────────────────────────
        public static class AprendizPerfil
        {
            public const string Read = "aprendiz_perfil:PERMISSIONS_READ";
        }

        public static class AprendizRegiones
        {
            public const string Read = "aprendiz_regiones:PERMISSIONS_READ";
        }

        public static class AprendizRetos
        {
            public const string Read = "aprendiz_retos:PERMISSIONS_READ";
        }

        public static class AprendizInsignias
        {
            public const string Read = "aprendiz_insignias:PERMISSIONS_READ";
        }

        public static class AprendizCertificados
        {
            public const string Read = "aprendiz_certificados:PERMISSIONS_READ";
        }

        public static class AprendizCalendario
        {
            public const string Read = "aprendiz_calendario:PERMISSIONS_READ";
        }

        public static class AprendizBiblioteca
        {
            public const string Read = "aprendiz_biblioteca:PERMISSIONS_READ";
        }

        public static class AprendizComunidad
        {
            public const string Read = "aprendiz_comunidad:PERMISSIONS_READ";
        }

        public static class AprendizPremium
        {
            public const string Read = "aprendiz_premium:PERMISSIONS_READ";
        }

        public static class AprendizNoticias
        {
            public const string Read = "aprendiz_noticias:PERMISSIONS_READ";
        }

        public static class AprendizRegionDestacada
        {
            public const string Read = "aprendiz_region_destacada:PERMISSIONS_READ";
        }
    }
}
