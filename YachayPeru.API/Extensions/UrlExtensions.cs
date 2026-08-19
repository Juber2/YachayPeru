namespace YachayPeru.API.Extensions
{
    public static class UrlExtensions
    {
        /// <summary>
        /// Arma la URL absoluta a partir de una ruta relativa devuelta por IFileStorageService
        /// (ej. "/files/insignias/xxx.jpg"). Los repositorios/servicios nunca conocen el dominio;
        /// solo el controller, en el momento de responder, lo arma con el Request actual.
        /// </summary>
        public static string? ToAbsoluteUrl(this HttpRequest request, string? relativePath)
            => relativePath is null ? null : $"{request.Scheme}://{request.Host}{relativePath}";
    }
}
