using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Learning;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Actions.Courses.Models;
using YachayPeru.Application.Common.Results;
using YachayPeru.Application.Features.Courses.Queries.GetCourseContent;
using YachayPeru.Domain.Constants;
using YachayPeru.Domain.Entities.Learning;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Actions.Courses
{
    public class CourseContentActions
    {
        private readonly ICourseVersionRepository versionRepository;
        private readonly ICourseModuleRepository moduleRepository;
        private readonly IModuleContentRepository contentRepository;
        private readonly IModuleContentFileRepository fileRepository;
        private readonly IFileStorageService fileStorage;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUser currentUser;

        public CourseContentActions(
            ICourseVersionRepository _versionRepository,
            ICourseModuleRepository _moduleRepository,
            IModuleContentRepository _contentRepository,
            IModuleContentFileRepository _fileRepository,
            IFileStorageService _fileStorage,
            IUnitOfWork _unitOfWork,
            ICurrentUser _currentUser)
        {
            versionRepository = _versionRepository;
            moduleRepository  = _moduleRepository;
            contentRepository = _contentRepository;
            fileRepository    = _fileRepository;
            fileStorage       = _fileStorage;
            unitOfWork        = _unitOfWork;
            currentUser       = _currentUser;
        }

        public async Task<Result<int>> AddModule(AddModuleInput input, CancellationToken ct)
        {
            var version = await versionRepository.GetByIdAsync(input.CourseVersionId, ct);
            if (version is null)
                return Result<int>.Failure("Versión de curso no encontrada.", NotFound);

            if (version.StatusCode == AppConstants.CourseVersionStatus.Published)
                return Result<int>.Failure("No se puede modificar una versión publicada.", Conflict);

            var nextOrder = await moduleRepository.GetNextOrderIndexAsync(input.CourseVersionId, ct);

            var module = new CourseModule
            {
                CourseVersionId = input.CourseVersionId,
                Title = input.Title,
                Description = input.Description,
                DurationHours = input.DurationHours,
                OrderIndex = nextOrder,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser.Id
            };

            await moduleRepository.AddAsync(module, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return Result<int>.Success(module.Id);
        }

        public async Task<Result<int>> EditModule(EditModuleInput input, CancellationToken ct)
        {
            var module = await moduleRepository.GetByIdAsync(input.ModuleId, ct);
            if (module is null)
                return Result<int>.Failure("Módulo no encontrado.", NotFound);

            var version = await versionRepository.GetByIdAsync(module.CourseVersionId, ct);
            if (version?.StatusCode == AppConstants.CourseVersionStatus.Published)
                return Result<int>.Failure("No se puede modificar una versión publicada.", Conflict);

            module.Title = input.Title;
            module.Description = input.Description;
            module.DurationHours = input.DurationHours;
            module.UpdatedAt = DateTime.UtcNow;
            module.UpdatedBy = currentUser.Id;

            moduleRepository.Update(module);
            await unitOfWork.SaveChangesAsync(ct);

            return Result<int>.Success(module.Id);
        }

        public async Task<Result> DeleteModule(int moduleId, CancellationToken ct)
        {
            var module = await moduleRepository.GetByIdAsync(moduleId, ct);
            if (module is null)
                return Result.Failure("Módulo no encontrado.", NotFound);

            var version = await versionRepository.GetByIdAsync(module.CourseVersionId, ct);
            if (version?.StatusCode == AppConstants.CourseVersionStatus.Published)
                return Result.Failure("No se puede modificar una versión publicada.", Conflict);

            module.Deleted = true;
            module.UpdatedAt = DateTime.UtcNow;
            module.UpdatedBy = currentUser.Id;

            moduleRepository.Update(module);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }

        public async Task<Result> ReorderModules(ReorderModulesInput input, CancellationToken ct)
        {
            var version = await versionRepository.GetByIdAsync(input.CourseVersionId, ct);
            if (version is null)
                return Result.Failure("Versión de curso no encontrada.", NotFound);

            if (version.StatusCode == AppConstants.CourseVersionStatus.Published)
                return Result.Failure("No se puede modificar una versión publicada.", Conflict);

            foreach (var item in input.Items)
            {
                var module = await moduleRepository.GetByIdAsync(item.ModuleId, ct);
                if (module is null || module.CourseVersionId != input.CourseVersionId) continue;
                module.OrderIndex = item.OrderIndex;
                module.UpdatedAt = DateTime.UtcNow;
                module.UpdatedBy = currentUser.Id;
                moduleRepository.Update(module);
            }

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }

        public async Task<Result> ReorderContents(ReorderContentsInput input, CancellationToken ct)
        {
            var module = await moduleRepository.GetByIdAsync(input.ModuleId, ct);
            if (module is null)
                return Result.Failure("Módulo no encontrado.", NotFound);

            var version = await versionRepository.GetByIdAsync(module.CourseVersionId, ct);
            if (version?.StatusCode == AppConstants.CourseVersionStatus.Published)
                return Result.Failure("No se puede modificar una versión publicada.", Conflict);

            foreach (var item in input.Items)
            {
                var content = await contentRepository.GetByIdAsync(item.ContentId, ct);
                if (content is null || content.ModuleId != input.ModuleId) continue;
                content.OrderIndex = item.OrderIndex;
                content.UpdatedAt = DateTime.UtcNow;
                content.UpdatedBy = currentUser.Id;
                contentRepository.Update(content);
            }

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }

        public async Task<Result<int>> AddContent(AddModuleContentInput input, CancellationToken ct)
        {
            var module = await moduleRepository.GetByIdAsync(input.ModuleId, ct);
            if (module is null)
                return Result<int>.Failure("Módulo no encontrado.", NotFound);

            var version = await versionRepository.GetByIdAsync(module.CourseVersionId, ct);
            if (version?.StatusCode == AppConstants.CourseVersionStatus.Published)
                return Result<int>.Failure("No se puede modificar una versión publicada.", Conflict);

            var nextOrder = await contentRepository.GetNextOrderIndexAsync(input.ModuleId, ct);

            var content = new ModuleContent
            {
                ModuleId = input.ModuleId,
                Text = input.Text,
                DesignJson = input.DesignJson,
                OrderIndex = nextOrder,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser.Id
            };

            await contentRepository.AddAsync(content, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return Result<int>.Success(content.Id);
        }

        public async Task<Result<int>> EditContent(EditModuleContentInput input, CancellationToken ct)
        {
            var content = await contentRepository.GetByIdAsync(input.ContentId, ct);
            if (content is null)
                return Result<int>.Failure("Contenido no encontrado.", NotFound);

            var module = await moduleRepository.GetByIdAsync(content.ModuleId, ct);
            var version = await versionRepository.GetByIdAsync(module?.CourseVersionId ?? 0, ct);
            if (version?.StatusCode == AppConstants.CourseVersionStatus.Published)
                return Result<int>.Failure("No se puede modificar una versión publicada.", Conflict);

            content.Text = input.Text;
            content.DesignJson = input.DesignJson;
            content.UpdatedAt = DateTime.UtcNow;
            content.UpdatedBy = currentUser.Id;

            contentRepository.Update(content);
            await unitOfWork.SaveChangesAsync(ct);

            return Result<int>.Success(content.Id);
        }

        public async Task<Result> DeleteContent(int contentId, CancellationToken ct)
        {
            var content = await contentRepository.GetByIdAsync(contentId, ct);
            if (content is null)
                return Result.Failure("Contenido no encontrado.", NotFound);

            var files = await fileRepository.GetByItemAsync(contentId, ct);
            foreach (var file in files)
            {
                fileStorage.Delete(file.FileUrl);
                fileRepository.Delete(file);
            }

            content.Deleted = true;
            content.UpdatedAt = DateTime.UtcNow;
            content.UpdatedBy = currentUser.Id;

            contentRepository.Update(content);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }

        public async Task<Result> DiscardDraft(int courseId, CancellationToken ct)
        {
            var draft = await versionRepository.GetDraftAsync(courseId, ct);
            if (draft is null)
                return Result.Failure("No hay un borrador activo.", NotFound);

            draft.Deleted   = true;
            draft.UpdatedAt = DateTime.UtcNow;
            draft.UpdatedBy = currentUser.Id;
            versionRepository.Update(draft);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }

        public async Task<IReadOnlyList<VersionSummary>> GetVersionHistory(int courseId, CancellationToken ct)
        {
            var versions = await versionRepository.GetHistoryAsync(courseId, ct);
            return versions.Select(v => new VersionSummary
            {
                Id            = v.Id,
                VersionNumber = v.VersionNumber,
                StatusCode    = v.StatusCode,
                IsCurrent     = v.IsCurrent,
                PublishedAt   = v.PublishedAt,
                CreatedAt     = v.CreatedAt
            }).ToList();
        }

        public async Task<Result<VersionDetail>> GetVersionDetail(int versionId, int courseId, CancellationToken ct)
        {
            var version = await versionRepository.GetByIdAsync(versionId, ct);
            if (version is null || version.CourseId != courseId)
                return Result<VersionDetail>.Failure("Versión no encontrada.", NotFound);

            var modules = await moduleRepository.GetByVersionAsync(versionId, ct);
            var moduleDtos = new List<ModuleDto>();
            foreach (var module in modules)
            {
                var contents = await contentRepository.GetByModuleAsync(module.Id, ct);
                var contentDtos = new List<ContentDto>();
                foreach (var c in contents)
                {
                    var files = await fileRepository.GetByItemAsync(c.Id, ct);
                    contentDtos.Add(new ContentDto
                    {
                        Id                    = c.Id,
                        Text                  = c.Text,
                        DesignJson            = c.DesignJson,
                        OrderIndex            = c.OrderIndex,
                        Files                 = files.Select(f => new FileDto
                        {
                            Id           = f.Id,
                            FileTypeCode = f.FileTypeCode,
                            FileUrl      = f.FileUrl,
                            FileName     = f.FileName,
                            OrderIndex   = f.OrderIndex
                        }).ToList()
                    });
                }
                moduleDtos.Add(new ModuleDto
                {
                    Id            = module.Id,
                    Title         = module.Title,
                    Description   = module.Description,
                    OrderIndex    = module.OrderIndex,
                    DurationHours = module.DurationHours,
                    Contents      = contentDtos
                });
            }

            return Result<VersionDetail>.Success(new VersionDetail
            {
                Id            = version.Id,
                VersionNumber = version.VersionNumber,
                StatusCode    = version.StatusCode,
                DurationHours = version.DurationHours,
                IsCurrent     = version.IsCurrent,
                PublishedAt   = version.PublishedAt,
                CreatedAt     = version.CreatedAt,
                Modules       = moduleDtos
            });
        }

        public async Task<Result<int>> PublishContentVersion(int courseId, CancellationToken ct)
        {
            var draft = await versionRepository.GetDraftAsync(courseId, ct);
            if (draft is null)
                return Result<int>.Failure("No hay un borrador pendiente de publicación.", NotFound);

            var current = await versionRepository.GetCurrentAsync(courseId, ct);
            if (current is not null)
            {
                current.IsCurrent = false;
                current.StatusCode = AppConstants.CourseVersionStatus.Archived;
                current.UpdatedAt = DateTime.UtcNow;
                current.UpdatedBy = currentUser.Id;
                versionRepository.Update(current);
            }

            draft.IsCurrent = true;
            draft.StatusCode = AppConstants.CourseVersionStatus.Published;
            draft.PublishedAt = DateTime.UtcNow;
            draft.PublishedBy = currentUser.Id;
            draft.UpdatedAt = DateTime.UtcNow;
            draft.UpdatedBy = currentUser.Id;

            versionRepository.Update(draft);
            await unitOfWork.SaveChangesAsync(ct);

            return Result<int>.Success(draft.Id);
        }

        public async Task<Result<int>> CreateDraft(int courseId, CancellationToken ct)
        {
            var existingDraft = await versionRepository.GetDraftAsync(courseId, ct);
            if (existingDraft is not null)
                return Result<int>.Failure("Ya existe un borrador activo.", Conflict);

            var current = await versionRepository.GetCurrentAsync(courseId, ct);
            var nextVersion = await versionRepository.GetNextVersionNumberAsync(courseId, ct);

            var newDraft = new CourseVersion
            {
                CourseId      = courseId,
                VersionNumber = nextVersion,
                StatusCode    = AppConstants.CourseVersionStatus.Draft,
                DurationHours = current?.DurationHours,
                IsCurrent     = false,
                CreatedAt     = DateTime.UtcNow,
                CreatedBy     = currentUser.Id
            };

            await versionRepository.AddAsync(newDraft, ct);
            await unitOfWork.SaveChangesAsync(ct);

            if (current is not null)
                await CloneVersionModulesAsync(current.Id, newDraft.Id, ct);

            await unitOfWork.SaveChangesAsync(ct);
            return Result<int>.Success(newDraft.Id);
        }

        public async Task<Result<int>> RestoreContentVersion(int versionId, int courseId, CancellationToken ct)
        {
            var target = await versionRepository.GetByIdAsync(versionId, ct);
            if (target is null || target.CourseId != courseId)
                return Result<int>.Failure("Versión no encontrada.", NotFound);

            var existingDraft = await versionRepository.GetDraftAsync(courseId, ct);
            if (existingDraft is not null)
                return Result<int>.Failure("Ya existe un borrador activo. Publícalo o descártalo antes de restaurar.", Conflict);

            var nextVersion = await versionRepository.GetNextVersionNumberAsync(courseId, ct);

            var modules = await moduleRepository.GetByVersionAsync(versionId, ct);

            var newDraft = new CourseVersion
            {
                CourseId = courseId,
                VersionNumber = nextVersion,
                StatusCode = AppConstants.CourseVersionStatus.Draft,
                DurationHours = target.DurationHours,
                IsCurrent = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser.Id
            };

            await versionRepository.AddAsync(newDraft, ct);
            await unitOfWork.SaveChangesAsync(ct);

            await CloneVersionModulesAsync(versionId, newDraft.Id, ct);

            await unitOfWork.SaveChangesAsync(ct);
            return Result<int>.Success(newDraft.Id);
        }

        private async Task CloneVersionModulesAsync(int sourceVersionId, int targetVersionId, CancellationToken ct)
        {
            var modules = await moduleRepository.GetByVersionAsync(sourceVersionId, ct);
            foreach (var module in modules)
            {
                var copiedModule = new CourseModule
                {
                    CourseVersionId = targetVersionId,
                    Title           = module.Title,
                    Description     = module.Description,
                    OrderIndex      = module.OrderIndex,
                    DurationHours   = module.DurationHours,
                    CreatedAt       = DateTime.UtcNow,
                    CreatedBy       = currentUser.Id
                };
                await moduleRepository.AddAsync(copiedModule, ct);
                await unitOfWork.SaveChangesAsync(ct);

                var contents = await contentRepository.GetByModuleAsync(module.Id, ct);
                foreach (var content in contents)
                {
                    var copiedContent = new ModuleContent
                    {
                        ModuleId              = copiedModule.Id,
                        Text                  = content.Text,
                        DesignJson            = content.DesignJson,
                        OrderIndex            = content.OrderIndex,
                        CreatedAt             = DateTime.UtcNow,
                        CreatedBy             = currentUser.Id
                    };
                    await contentRepository.AddAsync(copiedContent, ct);
                    await unitOfWork.SaveChangesAsync(ct);

                    var files = await fileRepository.GetByItemAsync(content.Id, ct);
                    foreach (var file in files)
                    {
                        await fileRepository.AddAsync(new ModuleContentFile
                        {
                            ModuleContentId = copiedContent.Id,
                            FileTypeCode    = file.FileTypeCode,
                            FileUrl         = file.FileUrl,
                            FileName        = file.FileName,
                            OrderIndex      = file.OrderIndex,
                            CreatedAt       = DateTime.UtcNow,
                            CreatedBy       = currentUser.Id
                        }, ct);
                    }
                }
            }
        }
    }
}
