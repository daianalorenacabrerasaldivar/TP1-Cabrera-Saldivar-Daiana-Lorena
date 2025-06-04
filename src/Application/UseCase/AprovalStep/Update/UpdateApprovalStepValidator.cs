using Domain.Common.ResultPattern;

namespace Application.UseCase.AprovalStep.Update
{
    public static class UpdateApprovalStepValidator
    {
        public static Result<string> Validate(UpdateApprovalStepCommand command)
        {

            if (command.ProjectId == Guid.Empty)
                return new Failed<string>("El ID del proyecto no puede ser un GUID vacío");

            if (command.StepId <= 0)
                return new Failed<string>("El ID del paso debe ser mayor que 0");

            if (command.UserId <= 0)
                return new Failed<string>("El ID del usuario debe ser mayor que 0");

            if (command.Status < 1 || command.Status > 3)
                return new Failed<string>("El estado debe estar entre 1 y 3");

            return new Success<string>("Validación exitosa");
        }
    }
}