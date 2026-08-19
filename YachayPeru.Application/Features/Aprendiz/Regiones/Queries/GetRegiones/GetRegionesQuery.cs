using MediatR;

namespace YachayPeru.Application.Features.Aprendiz.Regiones.Queries.GetRegiones
{
    public record GetRegionesQuery(int UserId) : IRequest<IReadOnlyList<AprendizRegionListItem>>;

    public class AprendizRegionListItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? CoverImageUrl { get; set; }
        public int ProgressPercent { get; set; }
        public bool IsCompleted { get; set; }
    }
}
