using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Auth;
using YachayPeru.Application.Common.Exceptions;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Authentication.Commands.RevokeSession
{
    public sealed class RevokeSessionCommandHandler : IRequestHandler<RevokeSessionCommand, Result<bool>>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _uow;

        public RevokeSessionCommandHandler(
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork uow)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _uow = uow;
        }

        public async Task<Result<bool>> Handle(RevokeSessionCommand command, CancellationToken ct)
        {
            var existingToken = await _refreshTokenRepository.GetByApprovalTokenAsync(command.ApprovalToken, ct);

            if (existingToken is null)
                throw new UnauthorizedException("TOKEN_INVALID", "Token inválido.");

            await _uow.BeginTransactionAsync(ct);
            await _refreshTokenRepository.RevokeAllByUserIdAsync(existingToken.UserId, ct);
            await _uow.SaveChangesAsync(ct);
            await _uow.CommitTransactionAsync(ct);

            return Result<bool>.Success(true);
        }
    }
}
