using Domain.Common.ResultPattern;
using Domain.Entity;
using Domain.Enum;

namespace Infrastructure.Persistencia.Repositories.Query.AprovalRules
{
    public class ApprovalStepUpdater : IApprovalStepUpdater
    {
        public Result<string> UpdateStep(ProjectProposal project, int newStatus, string observation, User user)
        {
            var stepToUpdate = project.ApprovalSteps
                .FirstOrDefault(step => step.ApproverRoleId == user.Role && step.Status == (int)StatusEnum.Pending);

            if (stepToUpdate == null)
            {
                return new Failed<string>("No se encontró un paso de aprobación pendiente para el rol del usuario.");
            }
            stepToUpdate.ApproverUserId = user.Id;
            stepToUpdate.Status = newStatus;
            stepToUpdate.DecisionDate = DateTime.UtcNow;
            stepToUpdate.Observations = observation;

            return new Success<string>("Paso de aprobación actualizado.");
        }
    }
}
