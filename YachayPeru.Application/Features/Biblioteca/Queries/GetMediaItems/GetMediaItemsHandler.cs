using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Persistence.Learning;

namespace YachayPeru.Application.Features.Biblioteca.Queries.GetMediaItems
{
    public class GetMediaItemsHandler : IRequestHandler<GetMediaItemsQuery, IReadOnlyList<MediaItemListItem>>
    {
        private readonly IMediaItemRepository repository;
        private readonly ICourseRepository courseRepository;

        public GetMediaItemsHandler(IMediaItemRepository _repository, ICourseRepository _courseRepository)
        {
            repository = _repository;
            courseRepository = _courseRepository;
        }

        public async Task<IReadOnlyList<MediaItemListItem>> Handle(GetMediaItemsQuery request, CancellationToken ct)
        {
            var mediaItems = await repository.ListAsync(ct);
            var items = new List<MediaItemListItem>();

            foreach (var m in mediaItems)
            {
                var course = await courseRepository.GetByIdAsync(m.CourseId, ct);
                items.Add(new MediaItemListItem
                {
                    Id = m.Id,
                    Title = m.Title,
                    MediaTypeCode = m.MediaTypeCode,
                    RegionId = m.CourseId,
                    RegionTitle = course?.Title ?? string.Empty,
                    ThumbnailUrl = m.ThumbnailUrl,
                    IsActive = m.IsActive,
                    CreatedAt = m.CreatedAt
                });
            }

            return items;
        }
    }
}
