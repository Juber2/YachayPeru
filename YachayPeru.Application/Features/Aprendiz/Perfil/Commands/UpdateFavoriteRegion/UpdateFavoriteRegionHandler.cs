using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Application.Abstractions.Persistence.Learning;
using YachayPeru.Application.Common.Results;
using YachayPeru.Domain.Entities.Aprendiz;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Aprendiz.Perfil.Commands.UpdateFavoriteRegion
{
    public class UpdateFavoriteRegionHandler : IRequestHandler<UpdateFavoriteRegionCommand, Result>
    {
        private readonly IAprendizProfileRepository profileRepository;
        private readonly ICourseRepository courseRepository;
        private readonly IUnitOfWork unitOfWork;

        public UpdateFavoriteRegionHandler(
            IAprendizProfileRepository _profileRepository,
            ICourseRepository _courseRepository,
            IUnitOfWork _unitOfWork)
        {
            profileRepository = _profileRepository;
            courseRepository = _courseRepository;
            unitOfWork = _unitOfWork;
        }

        public async Task<Result> Handle(UpdateFavoriteRegionCommand request, CancellationToken ct)
        {
            var region = await courseRepository.GetByIdAsync(request.RegionId, ct);
            if (region is null)
                return Result.Failure("Región no encontrada.", NotFound);

            var profile = await profileRepository.GetByUserIdAsync(request.UserId, ct);
            if (profile is null)
            {
                profile = new AprendizProfile
                {
                    UserId = request.UserId,
                    Points = 0,
                    Level = 1,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = request.UserId
                };
                await profileRepository.AddAsync(profile, ct);
            }

            profile.FavoriteRegionId = request.RegionId;
            profile.UpdatedAt = DateTime.UtcNow;
            profile.UpdatedBy = request.UserId;
            profileRepository.Update(profile);

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
