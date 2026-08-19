using MediatR;

namespace YachayPeru.Application.Features.Aprendiz.Certificados.Queries.GetCertificados
{
    public record GetCertificadosQuery(int UserId) : IRequest<IReadOnlyList<AprendizCertificadoListItem>>;

    public class AprendizCertificadoListItem
    {
        public int RetoId { get; set; }
        public string RetoTitle { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
    }
}
