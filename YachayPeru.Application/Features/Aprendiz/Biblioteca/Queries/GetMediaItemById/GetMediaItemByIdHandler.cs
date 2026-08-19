using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Persistence.Learning;
using YachayPeru.Application.Common.Results;
using YachayPeru.Domain.Constants;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Aprendiz.Biblioteca.Queries.GetMediaItemById
{
    public class GetMediaItemByIdHandler : IRequestHandler<GetMediaItemByIdQuery, Result<AprendizMediaItemDetail>>
    {
        private readonly IMediaItemRepository mediaItemRepository;
        private readonly ICourseRepository courseRepository;

        public GetMediaItemByIdHandler(IMediaItemRepository _mediaItemRepository, ICourseRepository _courseRepository)
        {
            mediaItemRepository = _mediaItemRepository;
            courseRepository = _courseRepository;
        }

        public async Task<Result<AprendizMediaItemDetail>> Handle(GetMediaItemByIdQuery request, CancellationToken ct)
        {
            var mediaItem = await mediaItemRepository.GetByIdAsync(request.Id, ct);
            if (mediaItem is null || !mediaItem.IsActive)
                return Result<AprendizMediaItemDetail>.Failure("Recurso no encontrado.", NotFound);

            var region = await courseRepository.GetByIdAsync(mediaItem.CourseId, ct);

            var detail = new AprendizMediaItemDetail
            {
                Id = mediaItem.Id,
                Title = mediaItem.Title,
                MediaTypeCode = mediaItem.MediaTypeCode,
                RegionId = mediaItem.CourseId,
                RegionTitle = region?.Title ?? string.Empty,
                ThumbnailUrl = mediaItem.ThumbnailUrl,
                ExternalUrl = mediaItem.ExternalUrl,
                LegendText = mediaItem.LegendText,
                IsPlayable = mediaItem.MediaTypeCode == AppConstants.MediaTypeCode.Video
                    || mediaItem.MediaTypeCode == AppConstants.MediaTypeCode.Musica
            };

            return Result<AprendizMediaItemDetail>.Success(detail);
        }
    }
}
