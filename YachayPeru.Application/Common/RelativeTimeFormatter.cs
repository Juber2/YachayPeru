namespace YachayPeru.Application.Common
{
    public static class RelativeTimeFormatter
    {
        public static string Format(DateTime utcDateTime, DateTime? nowUtc = null)
        {
            var now = nowUtc ?? DateTime.UtcNow;
            var span = now - utcDateTime;

            if (span.TotalSeconds < 60) return "hace un momento";
            if (span.TotalMinutes < 2) return "hace 1 minuto";
            if (span.TotalMinutes < 60) return $"hace {(int)span.TotalMinutes} minutos";
            if (span.TotalHours < 2) return "hace 1 hora";
            if (span.TotalHours < 24) return $"hace {(int)span.TotalHours} horas";
            if (span.TotalDays < 2) return "ayer";
            if (span.TotalDays < 30) return $"hace {(int)span.TotalDays} días";
            if (span.TotalDays < 60) return "hace 1 mes";
            if (span.TotalDays < 365) return $"hace {(int)(span.TotalDays / 30)} meses";

            var years = (int)(span.TotalDays / 365);
            return years <= 1 ? "hace 1 año" : $"hace {years} años";
        }
    }
}
