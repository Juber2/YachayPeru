using FluentValidation;

namespace YachayPeru.Application.Features.Administration.Roles.Commands.EditRole
{
    public class EditRoleCommandValidator : AbstractValidator<EditRoleCommand>
    {
        public EditRoleCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
            RuleFor(x => x.RoleCode).MaximumLength(100)
                .Matches("^[A-Z0-9_]+$").WithMessage("El código solo puede contener letras mayúsculas, números y guiones bajos.");
            RuleFor(x => x.Description).MaximumLength(500).When(x => x.Description is not null);
        }
    }
}
