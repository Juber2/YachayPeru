using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Aprendiz.Regiones.Queries.GetRegionById
{
    public record GetRegionByIdQuery(int UserId, int RegionId) : IRequest<Result<AprendizRegionDetail>>;

    public class AprendizRegionDetail
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? AmbientAudioUrl { get; set; }
        public string? AmbientAudioTitle { get; set; }
        public string? SpotifyUrl { get; set; }
        public List<AprendizModule> Modules { get; set; } = [];
        public int RetoCount { get; set; }
        public int CompletedRetoCount { get; set; }
    }

    public class AprendizModule
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int OrderIndex { get; set; }
        public decimal? DurationHours { get; set; }
        public List<AprendizModuleContent> Contents { get; set; } = [];
    }

    public class AprendizModuleContent
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public int OrderIndex { get; set; }
        public List<AprendizModuleContentFile> Files { get; set; } = [];
    }

    public class AprendizModuleContentFile
    {
        public int Id { get; set; }
        public string FileTypeCode { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
    }
}
