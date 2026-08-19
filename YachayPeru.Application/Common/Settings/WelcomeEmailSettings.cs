namespace YachayPeru.Application.Common.Settings
{
    public sealed class WelcomeEmailSettings
    {
        public const string SectionName = "WelcomeEmail";

        public string AppName { get; set; } = "Staff Training Platform";
        public string LoginUrl { get; set; } = "https://localhost:4200/login";
    }
}
