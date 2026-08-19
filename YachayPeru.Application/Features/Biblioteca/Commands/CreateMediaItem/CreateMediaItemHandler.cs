using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Results;
using YachayPeru.Domain.Entities.Content;

namespace YachayPeru.Application.Features.Biblioteca.Commands.CreateMediaItem
{
    public class CreateMediaItemHandler : IRequestHandler<CreateMediaItemCommand, Result<int>>
    {
        private readonly IMediaItemRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUser currentUser;

        public CreateMediaItemHandler(IMediaItemRepository _repository, IUnitOfWork _unitOfWork, ICurrentUser _currentUser)
        {
            repository = _repository;
            unitOfWork = _unitOfWork;
            currentUser = _currentUser;
        }

        public async Task<Result<int>> Handle(CreateMediaItemCommand request, CancellationToken ct)
        {
            var item = new MediaItem
            {
                Title = request.Title,
                MediaTypeCode = request.MediaTypeCode,
                CourseId = request.RegionId,
                ExternalUrl = request.ExternalUrl,
                LegendText = request.LegendText,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser.Id
            };

            await repository.AddAsync(item, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return Result<int>.Success(item.Id);
        }
    }
}
