namespace YachayPeru.Domain.Constants
{
    public static class AppConstants
    {
        public static class UserType
        {
            public const string Platform = "USER_TYPES_PLATFORM";
        }

        public static class Permissions
        {
            public const string Read   = "PERMISSIONS_READ";
            public const string Create = "PERMISSIONS_CREATE";
            public const string Update = "PERMISSIONS_UPDATE";
            public const string Delete = "PERMISSIONS_DELETE";
        }

        public static class UserStatus
        {
            public const string Active    = "USER_STATUS_ACTIVE";
            public const string Inactive  = "USER_STATUS_INACTIVE";
            public const string Suspended = "USER_STATUS_SUSPENDED";
        }

        public static class ResourceScope
        {
            public const string Platform = "RESOURCE_SCOPES_PLATFORM";
        }

        public static class CourseVersionStatus
        {
            public const string Draft     = "COURSE_VERSION_STATUS_DRAFT";
            public const string Published = "COURSE_VERSION_STATUS_PUBLISHED";
            public const string Archived  = "COURSE_VERSION_STATUS_ARCHIVED";
        }

        public static class RetoVersionStatus
        {
            public const string Draft     = "RETO_TEMPLATE_STATUS_DRAFT";
            public const string Published = "RETO_TEMPLATE_STATUS_PUBLISHED";
            public const string Archived  = "RETO_TEMPLATE_STATUS_ARCHIVED";
        }

        public static class QuestionResponseMode
        {
            public const string Single   = "QUESTION_RESPONSE_MODES_SINGLE";
            public const string Multiple = "QUESTION_RESPONSE_MODES_MULTIPLE";
        }

        public static class QuestionTypeCode
        {
            public const string SingleChoice   = "QUESTION_TYPE_SINGLE";
            public const string MultipleChoice = "QUESTION_TYPE_MULTIPLE";
            public const string TrueFalse      = "QUESTION_TYPE_TRUEFALSE";
            public const string FillBlank      = "QUESTION_TYPE_FILLBLANK";
        }

        public static class RegionZoneCode
        {
            public const string Costa  = "ZONA_COSTA";
            public const string Sierra = "ZONA_SIERRA";
            public const string Selva  = "ZONA_SELVA";
        }

        public static class FileTypeCode
        {
            public const string Image    = "FILE_TYPES_IMAGE";
            public const string Video    = "FILE_TYPES_VIDEO";
            public const string Document = "FILE_TYPES_DOCUMENT";
        }

        public static class MediaTypeCode
        {
            public const string Video   = "MEDIA_TYPE_VIDEO";
            public const string Musica  = "MEDIA_TYPE_MUSICA";
            public const string Foto    = "MEDIA_TYPE_FOTO";
            public const string Leyenda = "MEDIA_TYPE_LEYENDA";
        }

        /// <summary>Valores exactos que consume el frontend — no siguen el patrón UPPER_SNAKE del resto de códigos.</summary>
        public static class PaymentMethodCode
        {
            public const string Yape         = "yape";
            public const string Plin         = "plin";
            public const string Tarjeta      = "tarjeta";
            public const string PagoEfectivo = "pagoefectivo";

            public static readonly string[] All = { Yape, Plin, Tarjeta, PagoEfectivo };
        }

        /// <summary>Valores exactos que consume el frontend — no siguen el patrón UPPER_SNAKE del resto de códigos.</summary>
        public static class PremiumWaitlistStatusCode
        {
            public const string Pending  = "pending";
            public const string Approved = "approved";
            public const string Rejected = "rejected";
        }

    }
}
