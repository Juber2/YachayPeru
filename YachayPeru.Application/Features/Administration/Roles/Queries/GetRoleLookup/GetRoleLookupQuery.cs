using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Access;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Administration.Roles.Queries.GetRoleLookup
{
    public sealed record GetRoleLookupQuery : IRequest<Result<IReadOnlyList<PlatformRoleLookupDto>>>;

    public sealed record PlatformRoleLookupDto(int Id, string Name);

    public sealed class GetRoleLookupHandler
        : IRequestHandler<GetRoleLookupQuery, Result<IReadOnlyList<PlatformRoleLookupDto>>>
    {
        private readonly IPlatformRoleRepository _repository;

        public GetRoleLookupHandler(IPlatformRoleRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<IReadOnlyList<PlatformRoleLookupDto>>> Handle(
            GetRoleLookupQuery request, CancellationToken ct)
        {
            var roles = await _repository.GetAllAsync(ct);

            var dtos = roles
                .Select(r => new PlatformRoleLookupDto(r.Id, r.Name))
                .ToList();

            return Result<IReadOnlyList<PlatformRoleLookupDto>>.Success(dtos);
        }
    }
}
