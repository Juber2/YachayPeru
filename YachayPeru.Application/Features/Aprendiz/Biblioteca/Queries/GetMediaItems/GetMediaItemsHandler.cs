using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Persistence.Learning;
using YachayPeru.Domain.Constants;

namespace YachayPeru.Application.Features.Aprendiz.Biblioteca.Queries.GetMediaItems
{
    public class GetMediaItemsHandler : IRequestHandler<GetMediaItemsQuery, IReadOnlyList<AprendizMediaItemListItem>>
    {
        private readonly IMediaItemRepository mediaItemRepository;
        private readonly ICourseRepository courseRepository;

        public GetMediaItemsHandler(IMediaItemRepository _mediaItemRepository, ICourseRepository _courseRepository)
        {
            mediaItemRepository = _mediaItemRepository;
            courseRepository = _courseRepository;
        }

        public async Task<IReadOnlyList<AprendizMediaItemListItem>> Handle(GetMediaItemsQuery request, CancellationToken ct)
        {
            var mediaItems = await mediaItemRepository.ListAsync(m =>
                m.IsActive &&
                (request.MediaTypeCode == null || m.MediaTypeCode == request.MediaTypeCode) &&
                (request.RegionId == null || m.CourseId == request.RegionId), ct);

            var items = new List<AprendizMediaItemListItem>();
            foreach (var mediaItem in mediaItems)
            {
                var region = await courseRepository.GetByIdAsync(mediaItem.CourseId, ct);

                items.Add(new AprendizMediaItemListItem
                {
                    Id = mediaItem.Id,
                    Title = mediaItem.Title,
                    MediaTypeCode = mediaItem.MediaTypeCode,
                    RegionId = mediaItem.CourseId,
                    RegionTitle = region?.Title ?? string.Empty,
                    ThumbnailUrl = mediaItem.ThumbnailUrl,
                    IsPlayable = mediaItem.MediaTypeCode == AppConstants.MediaTypeCode.Video
                        || mediaItem.MediaTypeCode == AppConstants.MediaTypeCode.Musica
                });
            }

            return items;
        }
    }
}
