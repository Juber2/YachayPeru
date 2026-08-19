using MediatR;

namespace YachayPeru.Application.Features.Aprendiz.Perfil.Queries.GetActividad
{
    public record GetActividadQuery(int UserId) : IRequest<IReadOnlyList<AprendizActividadItem>>;

    public class AprendizActividadItem
    {
        public string Text { get; set; } = string.Empty;
        public string When { get; set; } = string.Empty;
    }
}
