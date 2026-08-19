using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Access;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Application.Abstractions.Persistence.Auth;
using YachayPeru.Application.Abstractions.Persistence.Common;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Results;
using YachayPeru.Application.Features.Authentication.Response;
using YachayPeru.Domain.Constants;
using YachayPeru.Domain.Entities.Aprendiz;
using YachayPeru.Domain.Entities.Auth;
using YachayPeru.Domain.Entities.Common;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Authentication.Commands.Register
{
    public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthResult>>
    {
        private readonly IUserRepository userRepository;
        private readonly IPersonRepository personRepository;
        private readonly IPlatformRoleRepository platformRoleRepository;
        private readonly IAprendizProfileRepository profileRepository;
        private readonly IPasswordHasher passwordHasher;
        private readonly IAuthTokenIssuer authTokenIssuer;
        private readonly IUnitOfWork unitOfWork;

        public RegisterCommandHandler(
            IUserRepository _userRepository,
            IPersonRepository _personRepository,
            IPlatformRoleRepository _platformRoleRepository,
            IAprendizProfileRepository _profileRepository,
            IPasswordHasher _passwordHasher,
            IAuthTokenIssuer _authTokenIssuer,
            IUnitOfWork _unitOfWork)
        {
            userRepository = _userRepository;
            personRepository = _personRepository;
            platformRoleRepository = _platformRoleRepository;
            profileRepository = _profileRepository;
            passwordHasher = _passwordHasher;
            authTokenIssuer = _authTokenIssuer;
            unitOfWork = _unitOfWork;
        }

        public async Task<Result<AuthResult>> Handle(RegisterCommand request, CancellationToken ct)
        {
            var usernameExists = await userRepository.AnyAsync(x => x.Username == request.Email, ct);
            if (usernameExists)
                return Result<AuthResult>.Failure("Ya existe una cuenta con ese email.", Conflict);

            var aprendizRole = await platformRoleRepository.GetByCodeAsync("APRENDIZ", ct);
            if (aprendizRole is null)
                return Result<AuthResult>.Failure("El rol de Aprendiz no está configurado.", NotFound);

            var (firstName, lastName) = SplitFullName(request.FullName);

            var person = new Person
            {
                FirstName = firstName,
                LastName = lastName,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = 0
            };
            await personRepository.AddAsync(person, ct);
            await unitOfWork.SaveChangesAsync(ct);

            var user = new User
            {
                PersonId = person.Id,
                Person = person,
                UserTypeCode = AppConstants.UserType.Platform,
                Username = request.Email,
                Email = request.Email,
                Password = passwordHasher.Hash(request.Password),
                IsLocked = false,
                RoleId = aprendizRole.Id,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = 0
            };
            await userRepository.AddAsync(user, ct);
            await unitOfWork.SaveChangesAsync(ct);

            await profileRepository.AddAsync(new AprendizProfile
            {
                UserId = user.Id,
                Points = 0,
                Level = 1,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = user.Id
            }, ct);

            await unitOfWork.SaveChangesAsync(ct);

            var result = await authTokenIssuer.IssueAsync(user, request.IpAddress, request.UserAgent, ct);
            return Result<AuthResult>.Success(result);
        }

        private static (string FirstName, string LastName) SplitFullName(string fullName)
        {
            var trimmed = fullName.Trim();
            var spaceIndex = trimmed.IndexOf(' ');
            if (spaceIndex < 0)
                return (trimmed, string.Empty);

            return (trimmed[..spaceIndex], trimmed[(spaceIndex + 1)..].Trim());
        }
    }
}
