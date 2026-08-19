using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Calendario.Queries.GetFestividadById
{
    public record GetFestividadByIdQuery(int Id) : IRequest<Result<FestividadDetail>>;

    public class FestividadDetail
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int RegionId { get; set; }
        public string RegionTitle { get; set; } = string.Empty;
        public int Month { get; set; }
        public int Day { get; set; }
        public bool IsActive { get; set; }
    }
}
