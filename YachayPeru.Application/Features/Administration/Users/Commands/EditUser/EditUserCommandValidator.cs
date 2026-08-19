using FluentValidation;

namespace YachayPeru.Application.Features.Administration.Users.Commands.EditUser
{
    public sealed class EditUserCommandValidator : AbstractValidator<EditUserCommand>
    {
        public EditUserCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El identificador del usuario es requerido.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("El nombre es requerido.")
                .MaximumLength(100).WithMessage("El nombre no puede superar 100 caracteres.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("El apellido es requerido.")
                .MaximumLength(100).WithMessage("El apellido no puede superar 100 caracteres.");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("El nombre de usuario es requerido.")
                .MaximumLength(100).WithMessage("El nombre de usuario no puede superar 100 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es requerido.")
                .EmailAddress().WithMessage("El email no tiene un formato válido.")
                .MaximumLength(150).WithMessage("El email no puede superar 150 caracteres.");

            When(x => !string.IsNullOrWhiteSpace(x.Password), () =>
            {
                RuleFor(x => x.Password)
                    .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.");
            });

            RuleFor(x => x.RoleId)
                .NotNull().WithMessage("El rol es requerido.")
                .GreaterThan(0).WithMessage("El rol es requerido.");
        }
    }
}
