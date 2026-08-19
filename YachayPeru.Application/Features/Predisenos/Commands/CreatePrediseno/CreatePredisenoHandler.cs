using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Results;
using YachayPeru.Domain.Entities.Content;

namespace YachayPeru.Application.Features.Predisenos.Commands.CreatePrediseno
{
    public class CreatePredisenoHandler : IRequestHandler<CreatePredisenoCommand, Result<int>>
    {
        private readonly IPredisenoRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUser currentUser;

        public CreatePredisenoHandler(IPredisenoRepository _repository, IUnitOfWork _unitOfWork, ICurrentUser _currentUser)
        {
            repository = _repository;
            unitOfWork = _unitOfWork;
            currentUser = _currentUser;
        }

        public async Task<Result<int>> Handle(CreatePredisenoCommand request, CancellationToken ct)
        {
            var entity = new Prediseno
            {
                Title = request.Title,
                TreeJson = request.TreeJson,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser.Id
            };

            await repository.AddAsync(entity, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return Result<int>.Success(entity.Id);
        }
    }
}
