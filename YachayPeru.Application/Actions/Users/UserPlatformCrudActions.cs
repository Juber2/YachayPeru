using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Auth;
using YachayPeru.Application.Abstractions.Persistence.Common;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Actions.Users.Models;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;
using YachayPeru.Application.Abstractions.Persistence.Auth;
using YachayPeru.Domain.Entities.Auth;
using YachayPeru.Domain.Entities.Common;

namespace YachayPeru.Application.Actions.Users
{
    public class UserPlatformCrudActions
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserRepository userRepository;
        private readonly IPersonRepository personRepository;
        private readonly IPasswordHasher passwordHasher;
        private readonly ICurrentUser currentUser;
        private readonly IEmailService emailService;

        public UserPlatformCrudActions(
            IUserRepository _userRepository,
            IPersonRepository _personRepository,
            IPasswordHasher _passwordHasher,
            IUnitOfWork _unitOfWork,
            ICurrentUser _currentUser,
            IEmailService _emailService)
        {
            userRepository = _userRepository;
            personRepository = _personRepository;
            passwordHasher = _passwordHasher;
            unitOfWork = _unitOfWork;
            currentUser = _currentUser;
            emailService = _emailService;
        }

        public async Task<Result<int>> CreateUser(CreateUserInput input, CancellationToken ct)
        {
            if (input.ReactivateUserId.HasValue)
                return await ReactivateUser(input, ct);

            var deletedUser = await userRepository.FindDeletedByUsernameOrEmailAsync(input.Username, input.Email, ct);
            if (deletedUser is not null)
                return Result<int>.Failure(
                    "Ya existe un usuario eliminado con ese nombre de usuario o email.",
                    UserPreviouslyDeleted,
                    deletedUser);

            var usernameExists = await userRepository.AnyAsync(x => x.Username == input.Username, ct);
            if (usernameExists)
                return Result<int>.Failure("El nombre de usuario ya está en uso.", Conflict);

            var emailExists = await userRepository.AnyAsync(x => x.Email == input.Email, ct);
            if (emailExists)
                return Result<int>.Failure("El email ya está registrado.", Conflict);

            if (input.RoleId is null or 0)
                return Result<int>.Failure("Se requiere un rol.", Conflict);

            var person = new Person
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser.Id
            };
            await personRepository.AddAsync(person, ct);

            var user = new User
            {
                Person = person,
                Username = input.Username,
                Email = input.Email,
                Password = passwordHasher.Hash(input.Password),
                UserTypeCode = input.UserTypeCode,
                RoleId = input.RoleId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser.Id
            };
            await userRepository.AddAsync(user, ct);

            await unitOfWork.SaveChangesAsync(ct);

            if (input.SendWelcomeMessage && !string.IsNullOrEmpty(input.Email))
            {
                var fullName = $"{input.FirstName} {input.LastName}".Trim();
                await emailService.SendWelcomeEmailAsync(input.Email, fullName, input.Username, input.Password, ct);
            }

            return Result<int>.Success(user.Id);
        }

        public async Task<Result<int>> UpdateUser(UpdateUserInput input, CancellationToken ct)
        {
            var user = await userRepository.GetByIdWithPersonAsync(input.Id, ct);
            if (user is null)
                return Result<int>.Failure("Usuario no encontrado.", NotFound);

            var usernameExists = await userRepository.AnyAsync(x => x.Username == input.Username && x.Id != input.Id, ct);
            if (usernameExists)
                return Result<int>.Failure("El nombre de usuario ya está en uso.", Conflict);

            var emailExists = await userRepository.AnyAsync(x => x.Email == input.Email && x.Id != input.Id, ct);
            if (emailExists)
                return Result<int>.Failure("El email ya está registrado.", Conflict);

            if (input.RoleId is null or 0)
                return Result<int>.Failure("Se requiere un rol.", Conflict);

            user.Username = input.Username;
            user.Email = input.Email;
            user.IsLocked = input.IsLocked;
            user.RoleId = input.RoleId;
            user.UpdatedAt = DateTime.UtcNow;
            user.UpdatedBy = currentUser.Id;

            if (!string.IsNullOrWhiteSpace(input.Password))
                user.Password = passwordHasher.Hash(input.Password);

            user.Person.FirstName = input.FirstName;
            user.Person.LastName = input.LastName;
            user.Person.UpdatedAt = DateTime.UtcNow;
            user.Person.UpdatedBy = currentUser.Id;

            userRepository.Update(user);
            personRepository.Update(user.Person);

            await unitOfWork.SaveChangesAsync(ct);
            return Result<int>.Success(user.Id);
        }

        private async Task<Result<int>> ReactivateUser(CreateUserInput input, CancellationToken ct)
        {
            var user = await userRepository.GetDeletedByIdWithPersonAsync(input.ReactivateUserId!.Value, ct);
            if (user is null)
                return Result<int>.Failure("Usuario eliminado no encontrado.", NotFound);

            var usernameConflict = await userRepository.AnyAsync(x => x.Username == input.Username && x.Id != user.Id, ct);
            if (usernameConflict)
                return Result<int>.Failure("El nombre de usuario ya está en uso.", Conflict);

            var emailConflict = await userRepository.AnyAsync(x => x.Email == input.Email && x.Id != user.Id, ct);
            if (emailConflict)
                return Result<int>.Failure("El email ya está registrado.", Conflict);

            user.Deleted = false;
            user.Username = input.Username;
            user.Email = input.Email;
            user.IsLocked = false;
            user.RoleId = input.RoleId;
            user.UpdatedAt = DateTime.UtcNow;
            user.UpdatedBy = currentUser.Id;
            if (!string.IsNullOrWhiteSpace(input.Password))
                user.Password = passwordHasher.Hash(input.Password);

            user.Person.FirstName = input.FirstName;
            user.Person.LastName = input.LastName;
            user.Person.Deleted = false;
            user.Person.UpdatedAt = DateTime.UtcNow;
            user.Person.UpdatedBy = currentUser.Id;

            userRepository.Update(user);
            personRepository.Update(user.Person);

            await unitOfWork.SaveChangesAsync(ct);

            if (input.SendWelcomeMessage && !string.IsNullOrEmpty(input.Email))
            {
                var fullName = $"{input.FirstName} {input.LastName}".Trim();
                await emailService.SendWelcomeEmailAsync(input.Email, fullName, input.Username, input.Password, ct);
            }

            return Result<int>.Success(user.Id);
        }

        public async Task<Result> DeleteUser(int userId, CancellationToken ct)
        {
            var user = await userRepository.GetByIdWithPersonAsync(userId, ct);
            if (user is null)
                return Result.Failure("Usuario no encontrado.", NotFound);

            user.Deleted = true;
            user.UpdatedAt = DateTime.UtcNow;
            user.UpdatedBy = currentUser.Id;
            userRepository.Update(user);

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
