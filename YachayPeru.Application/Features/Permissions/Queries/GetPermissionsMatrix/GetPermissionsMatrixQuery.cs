using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Access;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Permissions.Queries.GetPermissionsMatrix
{
    public sealed record GetPermissionsMatrixQuery : IRequest<Result<IReadOnlyList<ResourceMatrixDto>>>;

    public sealed record ResourceMatrixDto(
        int ResourceId,
        string ResourceCode,
        string ResourceName,
        IReadOnlyList<PermissionItemDto> Permissions);

    public sealed record PermissionItemDto(int Id, string ActionCode, string ActionName);

    public class GetPermissionsMatrixHandler : IRequestHandler<GetPermissionsMatrixQuery, Result<IReadOnlyList<ResourceMatrixDto>>>
    {
        private readonly IPermissionRepository permissionRepository;

        public GetPermissionsMatrixHandler(IPermissionRepository _permissionRepository)
        {
            permissionRepository = _permissionRepository;
        }

        public async Task<Result<IReadOnlyList<ResourceMatrixDto>>> Handle(
            GetPermissionsMatrixQuery request, CancellationToken cancellationToken)
        {
            var matrix = await permissionRepository.GetMatrixAsync(cancellationToken);

            var result = matrix
                .GroupBy(x => x.Resource)
                .Select(g => new ResourceMatrixDto(
                    g.Key.Id,
                    g.Key.Code,
                    g.Key.Name,
                    g.Select(x => new PermissionItemDto(x.Permission.Id, x.Permission.PermissionCode, x.ActionName))
                     .ToList()))
                .ToList();

            return Result<IReadOnlyList<ResourceMatrixDto>>.Success(result);
        }
    }
}
