using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Application.Abstractions.Persistence.Assessment;
using YachayPeru.Application.Abstractions.Persistence.Learning;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Results;
using YachayPeru.Domain.Constants;
using YachayPeru.Domain.Entities.Aprendiz;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Aprendiz.Regiones.Queries.GetRegionById
{
    public class GetRegionByIdHandler : IRequestHandler<GetRegionByIdQuery, Result<AprendizRegionDetail>>
    {
        private readonly ICourseRepository courseRepository;
        private readonly ICourseVersionRepository courseVersionRepository;
        private readonly ICourseModuleRepository courseModuleRepository;
        private readonly IModuleContentRepository moduleContentRepository;
        private readonly IModuleContentFileRepository moduleContentFileRepository;
        private readonly IRetoRepository retoRepository;
        private readonly IRetoVersionRepository versionRepository;
        private readonly IRetoAttemptRepository attemptRepository;
        private readonly IAprendizActivityLogRepository activityRepository;
        private readonly IAprendizRegionActivityRepository regionActivityRepository;
        private readonly IAprendizRegionExploredRepository regionExploredRepository;
        private readonly IInsigniaEvaluator insigniaEvaluator;
        private readonly IUnitOfWork unitOfWork;

        public GetRegionByIdHandler(
            ICourseRepository _courseRepository,
            ICourseVersionRepository _courseVersionRepository,
            ICourseModuleRepository _courseModuleRepository,
            IModuleContentRepository _moduleContentRepository,
            IModuleContentFileRepository _moduleContentFileRepository,
            IRetoRepository _retoRepository,
            IRetoVersionRepository _versionRepository,
            IRetoAttemptRepository _attemptRepository,
            IAprendizActivityLogRepository _activityRepository,
            IAprendizRegionActivityRepository _regionActivityRepository,
            IAprendizRegionExploredRepository _regionExploredRepository,
            IInsigniaEvaluator _insigniaEvaluator,
            IUnitOfWork _unitOfWork)
        {
            courseRepository = _courseRepository;
            courseVersionRepository = _courseVersionRepository;
            courseModuleRepository = _courseModuleRepository;
            moduleContentRepository = _moduleContentRepository;
            moduleContentFileRepository = _moduleContentFileRepository;
            retoRepository = _retoRepository;
            versionRepository = _versionRepository;
            attemptRepository = _attemptRepository;
            activityRepository = _activityRepository;
            regionActivityRepository = _regionActivityRepository;
            regionExploredRepository = _regionExploredRepository;
            insigniaEvaluator = _insigniaEvaluator;
            unitOfWork = _unitOfWork;
        }

        public async Task<Result<AprendizRegionDetail>> Handle(GetRegionByIdQuery request, CancellationToken ct)
        {
            var region = await courseRepository.GetByIdAsync(request.RegionId, ct);
            if (region is null || !region.IsActive)
                return Result<AprendizRegionDetail>.Failure("Región no encontrada.", NotFound);

            var modules = new List<AprendizModule>();
            var publishedVersion = await courseVersionRepository.GetCurrentAsync(region.Id, ct);
            if (publishedVersion is not null)
            {
                var courseModules = await courseModuleRepository.GetByVersionAsync(publishedVersion.Id, ct);
                foreach (var m in courseModules.OrderBy(m => m.OrderIndex))
                {
                    var contents = await moduleContentRepository.GetByModuleAsync(m.Id, ct);
                    var contentDtos = new List<AprendizModuleContent>();
                    foreach (var c in contents.OrderBy(c => c.OrderIndex))
                    {
                        var files = await moduleContentFileRepository.GetByItemAsync(c.Id, ct);
                        contentDtos.Add(new AprendizModuleContent
                        {
                            Id = c.Id,
                            Text = c.Text,
                            OrderIndex = c.OrderIndex,
                            Files = files.OrderBy(f => f.OrderIndex).Select(f => new AprendizModuleContentFile
                            {
                                Id = f.Id,
                                FileTypeCode = f.FileTypeCode,
                                FileUrl = f.FileUrl,
                                FileName = f.FileName,
                                OrderIndex = f.OrderIndex
                            }).ToList()
                        });
                    }

                    modules.Add(new AprendizModule
                    {
                        Id = m.Id,
                        Title = m.Title,
                        Description = m.Description,
                        OrderIndex = m.OrderIndex,
                        DurationHours = m.DurationHours,
                        Contents = contentDtos
                    });
                }
            }

            var retos = await retoRepository.GetByCourseAsync(region.Id, ct);
            var retoIds = retos.Select(r => r.Id).ToList();

            var publishedVersions = await versionRepository.ListAsync(
                v => retoIds.Contains(v.RetoId) && v.StatusCode == AppConstants.RetoVersionStatus.Published, ct);
            var publishedRetoIds = publishedVersions.Select(v => v.RetoId).ToHashSet();

            var passedRetoIds = (await attemptRepository.GetPassedRetoIdsByUserAsync(request.UserId, ct)).ToHashSet();

            var retoCount = publishedRetoIds.Count;
            var completedRetoCount = publishedRetoIds.Count(passedRetoIds.Contains);

            await activityRepository.AddAsync(new AprendizActivityLog
            {
                UserId = request.UserId,
                Text = $"Exploraste la región {region.Title}",
                RegionId = region.Id,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = request.UserId
            }, ct);

            // Upsert de "última región/módulo visto" — el módulo es el primero por
            // defecto, que es el que el frontend deja activo al entrar a la región.
            var firstModuleId = modules.FirstOrDefault()?.Id;
            if (firstModuleId is not null)
            {
                var existing = await regionActivityRepository.GetByUserIdAsync(request.UserId, ct);
                if (existing is null)
                {
                    await regionActivityRepository.AddAsync(new AprendizRegionActivity
                    {
                        UserId = request.UserId,
                        RegionId = region.Id,
                        ModuleId = firstModuleId.Value,
                        ViewedAt = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = request.UserId
                    }, ct);
                }
                else
                {
                    existing.RegionId = region.Id;
                    existing.ModuleId = firstModuleId.Value;
                    existing.ViewedAt = DateTime.UtcNow;
                    existing.UpdatedAt = DateTime.UtcNow;
                    existing.UpdatedBy = request.UserId;
                    regionActivityRepository.Update(existing);
                }
            }

            // Historial de regiones exploradas (distinto de "última vista" arriba) — una fila por
            // usuario+región, nunca se sobreescribe, usada por las insignias de exploración.
            if (!await regionExploredRepository.HasExploredAsync(request.UserId, region.Id, ct))
            {
                await regionExploredRepository.AddAsync(new AprendizRegionExplored
                {
                    UserId = request.UserId,
                    RegionId = region.Id,
                    FirstViewedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = request.UserId
                }, ct);
            }

            await unitOfWork.SaveChangesAsync(ct);

            await insigniaEvaluator.EvaluateAsync(request.UserId, ct);

            return Result<AprendizRegionDetail>.Success(new AprendizRegionDetail
            {
                Id = region.Id,
                Title = region.Title,
                Description = region.Description,
                CoverImageUrl = region.CoverImageUrl,
                Modules = modules,
                RetoCount = retoCount,
                CompletedRetoCount = completedRetoCount
            });
        }
    }
}
