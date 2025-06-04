using FluentValidation;

namespace Application.UseCase.ProjectProposals.Commands.Create
{
    public class CreateProjectProposalCommandValidator : AbstractValidator<CreateProjectProposalCommand>
    {
        public CreateProjectProposalCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(255).WithMessage("Datos del proyecto inválidos: el titulo Admite hasta 255 caracteres");
            RuleFor(x => x.Description)
                .NotNull().WithMessage("Datos del proyecto inválidos: Description es requerida.")
                .NotEmpty().WithMessage("Datos del proyecto inválidos: Description es requerida.");
            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Datos del proyecto inválidos: Duration debe ser mayor a 0.");
            RuleFor(x => x.Duration)
                .GreaterThan(0).WithMessage("Datos del proyecto inválidos: Duration debe ser mayor a 0.");
            RuleFor(x => x.Area)
                .GreaterThan(0).WithMessage("Datos del proyecto inválidos: Area es requerida");
            RuleFor(x => x.User)
                .GreaterThan(0).WithMessage("Datos del proyecto inválidos: User es requerido");
            RuleFor(x => x.Type)
                .GreaterThan(0).WithMessage("Datos del proyecto inválidos: Type es requerido");
        }
    }
}
