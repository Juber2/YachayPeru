namespace YachayPeru.Application.Features.Retos.Queries.GetCertificateList
{
    public class CertificateListItem
    {
        public int RetoId { get; set; }
        public string RetoTitle { get; set; } = string.Empty;
        public bool IsConfigured { get; set; }
    }
}
