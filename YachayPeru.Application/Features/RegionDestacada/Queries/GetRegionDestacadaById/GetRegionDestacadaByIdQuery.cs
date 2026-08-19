using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.RegionDestacada.Queries.GetRegionDestacadaById
{
    public record GetRegionDestacadaByIdQuery(int Id) : IRequest<Result<RegionDestacadaDetail>>;

    public class RegionDestacadaDetail
    {
        public int Id { get; set; }
        public int RegionId { get; set; }
        public string RegionTitle { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
