using FluentValidation;

namespace Application.UseCase.ProjectProposals.Querys.ProjectById
{
    public class GetProjectByIdQueryValidator : AbstractValidator<GetProjectByIdQuery>
    {
        public GetProjectByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("El ID del proyecto es requerido")
                .Must(id => id != Guid.Empty)
                .WithMessage("El ID del proyecto no puede ser un GUID vacío");
        }
    }
}