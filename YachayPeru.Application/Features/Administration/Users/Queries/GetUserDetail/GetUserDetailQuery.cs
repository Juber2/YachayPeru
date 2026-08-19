using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Auth;
using YachayPeru.Application.Common.Exceptions;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Administration.Users.Queries.GetUserDetail
{
    public sealed record GetUserDetailQuery(int Id)
        : IRequest<Result<PlatformUserDetailDto>>;

    public sealed record PlatformUserDetailDto(
        int      Id,
        string   FirstName,
        string   LastName,
        string?  Email,
        string   Username,
        bool     IsLocked,
        int?     RoleId,
        string?  RoleName);

    public sealed class GetUserDetailHandler
        : IRequestHandler<GetUserDetailQuery, Result<PlatformUserDetailDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserDetailHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<PlatformUserDetailDto>> Handle(
            GetUserDetailQuery request, CancellationToken ct)
        {
            var row = await _userRepository.GetPlatformUserDetailAsync(request.Id, ct);

            if (row is null)
                throw new NotFoundException("00"," Usuario no encontrado.");

            var dto = new PlatformUserDetailDto(
                row.UserId,
                row.FirstName,
                row.LastName,
                row.Email,
                row.Username,
                row.IsLocked,
                row.RoleId,
                row.RoleName);

            return Result<PlatformUserDetailDto>.Success(dto);
        }
    }
}
