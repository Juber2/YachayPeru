namespace YachayPeru.API.Contracts.Aprendiz.Certificados.Response
{
    public record AprendizCertificadoListItemResponse
    {
        public int RetoId { get; init; }
        public string RetoTitle { get; init; } = string.Empty;
        public bool IsAvailable { get; init; }
        public string? DownloadUrl { get; init; }
    }
}
