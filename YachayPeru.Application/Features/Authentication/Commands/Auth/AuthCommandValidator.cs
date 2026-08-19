using FluentValidation;

namespace YachayPeru.Application.Features.Authentication.Commands.Auth
{
    public sealed class AuthCommandValidator : AbstractValidator<AuthCommand>
    {
        public AuthCommandValidator()
        {
            When(x => !x.IsRefresh, () =>
            {
                RuleFor(x => x.Username)
                    .NotEmpty().WithMessage("El nombre de usuario es requerido.")
                    .MaximumLength(100).WithMessage("El nombre de usuario no puede superar 100 caracteres.");

                RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("La contraseña es requerida.");
            });

            When(x => x.IsRefresh, () =>
            {
                RuleFor(x => x.RefreshToken)
                    .NotEmpty().WithMessage("El refresh token es requerido.");
            });
        }
    }
}
