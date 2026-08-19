using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Results;
using YachayPeru.Domain.Entities.Content;

namespace YachayPeru.Application.Features.Noticias.Commands.CreateNoticia
{
    public class CreateNoticiaHandler : IRequestHandler<CreateNoticiaCommand, Result<int>>
    {
        private readonly INoticiaRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUser currentUser;

        public CreateNoticiaHandler(INoticiaRepository _repository, IUnitOfWork _unitOfWork, ICurrentUser _currentUser)
        {
            repository = _repository;
            unitOfWork = _unitOfWork;
            currentUser = _currentUser;
        }

        public async Task<Result<int>> Handle(CreateNoticiaCommand request, CancellationToken ct)
        {
            var noticia = new Noticia
            {
                Title = request.Title,
                Category = request.Category,
                Body = request.Body,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser.Id
            };

            await repository.AddAsync(noticia, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return Result<int>.Success(noticia.Id);
        }
    }
}
