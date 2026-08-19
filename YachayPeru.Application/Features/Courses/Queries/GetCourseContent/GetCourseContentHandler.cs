using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Learning;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Courses.Queries.GetCourseContent
{
    public class GetCourseContentHandler : IRequestHandler<GetCourseContentQuery, Result<CourseContentDetail>>
    {
        private readonly ICourseVersionRepository versionRepository;
        private readonly ICourseModuleRepository moduleRepository;
        private readonly IModuleContentRepository contentRepository;
        private readonly IModuleContentFileRepository fileRepository;

        public GetCourseContentHandler(
            ICourseVersionRepository _versionRepository,
            ICourseModuleRepository _moduleRepository,
            IModuleContentRepository _contentRepository,
            IModuleContentFileRepository _fileRepository)
        {
            versionRepository = _versionRepository;
            moduleRepository  = _moduleRepository;
            contentRepository = _contentRepository;
            fileRepository    = _fileRepository;
        }

        public async Task<Result<CourseContentDetail>> Handle(GetCourseContentQuery request, CancellationToken cancellationToken)
        {
            var draft = await versionRepository.GetDraftAsync(request.CourseId, cancellationToken);
            var version = draft ?? await versionRepository.GetCurrentAsync(request.CourseId, cancellationToken);

            if (version is null)
                return Result<CourseContentDetail>.Failure("El curso no tiene contenido aún.", NotFound);

            var modules = await moduleRepository.GetByVersionAsync(version.Id, cancellationToken);

            var moduleDtos = new List<ModuleDto>();
            foreach (var module in modules)
            {
                var contents = await contentRepository.GetByModuleAsync(module.Id, cancellationToken);
                var contentDtos = new List<ContentDto>();
                foreach (var c in contents)
                {
                    var files = await fileRepository.GetByItemAsync(c.Id, cancellationToken);
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

            return Result<CourseContentDetail>.Success(new CourseContentDetail
            {
                CourseVersionId = version.Id,
                VersionNumber = version.VersionNumber,
                StatusCode = version.StatusCode,
                DurationHours = version.DurationHours,
                Modules = moduleDtos
            });
        }
    }
}
