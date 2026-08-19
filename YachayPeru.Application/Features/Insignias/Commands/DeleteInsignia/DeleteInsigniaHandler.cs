using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Insignias.Commands.DeleteInsignia
{
    public class DeleteInsigniaHandler : IRequestHandler<DeleteInsigniaCommand, Result>
    {
        private readonly IInsigniaRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUser currentUser;

        public DeleteInsigniaHandler(IInsigniaRepository _repository, IUnitOfWork _unitOfWork, ICurrentUser _currentUser)
        {
            repository = _repository;
            unitOfWork = _unitOfWork;
            currentUser = _currentUser;
        }

        public async Task<Result> Handle(DeleteInsigniaCommand request, CancellationToken ct)
        {
            var insignia = await repository.GetByIdAsync(request.Id, ct);
            if (insignia is null)
                return Result.Failure("Insignia no encontrada.", NotFound);

            insignia.Deleted = true;
            insignia.UpdatedAt = DateTime.UtcNow;
            insignia.UpdatedBy = currentUser.Id;

            repository.Update(insignia);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }
    }
}
