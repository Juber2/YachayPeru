using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Persistence.Learning;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Biblioteca.Queries.GetMediaItemById
{
    public class GetMediaItemByIdHandler : IRequestHandler<GetMediaItemByIdQuery, Result<MediaItemDetail>>
    {
        private readonly IMediaItemRepository repository;
        private readonly ICourseRepository courseRepository;

        public GetMediaItemByIdHandler(IMediaItemRepository _repository, ICourseRepository _courseRepository)
        {
            repository = _repository;
            courseRepository = _courseRepository;
        }

        public async Task<Result<MediaItemDetail>> Handle(GetMediaItemByIdQuery request, CancellationToken ct)
        {
            var m = await repository.GetByIdAsync(request.Id, ct);
            if (m is null)
                return Result<MediaItemDetail>.Failure("Recurso no encontrado.", NotFound);

            var course = await courseRepository.GetByIdAsync(m.CourseId, ct);

            return Result<MediaItemDetail>.Success(new MediaItemDetail
            {
                Id = m.Id,
                Title = m.Title,
                MediaTypeCode = m.MediaTypeCode,
                RegionId = m.CourseId,
                RegionTitle = course?.Title ?? string.Empty,
                ThumbnailUrl = m.ThumbnailUrl,
                ExternalUrl = m.ExternalUrl,
                LegendText = m.LegendText,
                IsActive = m.IsActive
            });
        }
    }
}
