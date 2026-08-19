using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Auth;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Administration.Users.Queries.GetUserList
{
    public sealed record GetUserListQuery : IRequest<Result<IReadOnlyList<PlatformUserListItemDto>>>;

    public sealed record PlatformUserListItemDto(
        int      Id,
        string   FullName,
        string?  Email,
        bool     IsLocked,
        string?  RoleName,
        string?  RoleCode,
        DateTime? LastAccess);

    public sealed class GetUserListHandler
        : IRequestHandler<GetUserListQuery, Result<IReadOnlyList<PlatformUserListItemDto>>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserListHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<IReadOnlyList<PlatformUserListItemDto>>> Handle(
            GetUserListQuery request, CancellationToken ct)
        {
            var rows = await _userRepository.GetPlatformUserListAsync(ct);
            var dtos = rows
                .Select(r => new PlatformUserListItemDto(
                    r.UserId,
                    $"{r.FirstName} {r.LastName}".Trim(),
                    r.Email,
                    r.IsLocked,
                    r.RoleName,
                    r.RoleCode,
                    r.LastAccess
                ))
                .ToList();

            return Result<IReadOnlyList<PlatformUserListItemDto>>.Success(dtos);
        }
    }
}
