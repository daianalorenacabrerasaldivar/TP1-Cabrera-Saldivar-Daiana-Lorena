using Application.Common.Interface;
using Application.Common.Interface.Infrastructure;
using Application.UseCase.AprovalStep.Update;
using Domain.Common.ResultPattern;
using Domain.Entity;
using Domain.Enum;

namespace Application.Service.ValidatorsBusiness
{
    public class ProjectApprovalStepUpdateValidator : IProjectApprovalStepUpdateValidator
    {
        private readonly IUserQuery _userQuery;
        public ProjectApprovalStepUpdateValidator(IUserQuery userQuery)
        {
            _userQuery = userQuery;
        }
        public async Task<Result<ProjectApprovalStep>> TryGetUpdatableApprovalStepAsync(ProjectProposal project, UpdateApprovalStepCommand request)
        {
            bool projectIsModificable = IsProjectModifiable(project);
            if (projectIsModificable == false)
            {
                return new Failed<ProjectApprovalStep>("El proyecto no se encuentra en un estado que permite modificaciones");
            }
            var userResult = await ValidateUserAsync(request.UserId);
            if (userResult.IsFailed)
            {
                return new Failed<ProjectApprovalStep>(userResult.Info);
            }

            var approvalStep = FindApplicableApprovalStep(project, request.UserId, userResult.Value.Id);
            if (approvalStep == null)
            {
                return new Failed<ProjectApprovalStep>("No se encontro un paso de aprobacion valido para el usuario");
            }

            return new Success<ProjectApprovalStep>(approvalStep);

        }

        private static bool IsProjectModifiable(ProjectProposal project)
        {
            return project.Status == (int)StatusEnum.Pending ||
                   project.Status == (int)StatusEnum.Observed;
        }

        private async Task<Result<User>> ValidateUserAsync(int userId)
        {
            var userResult = await _userQuery.GetUserByIdAsync(userId);
            if (userResult.IsFailed)
            {
                return new Failed<User>(userResult.Info);
            }
            else if (userResult.Value == null)
            {
                return new Failed<User>("Usuario no encontrado");
            }
            return new Success<User>(userResult.Value);
        }

        private static ProjectApprovalStep FindApplicableApprovalStep(
            ProjectProposal project,
            int userId,
            int userRoleId)
        {
            return project.ApprovalSteps
                .Where(step =>
                    (step.Status == (int)StatusEnum.Pending || step.Status == (int)StatusEnum.Observed) &&
                    (step.ApproverUserId == userId ||
                     (step.ApproverUserId == null && step.ApproverRoleId == userRoleId))
                )
                .OrderBy(step => step.StepOrder)
                .FirstOrDefault();
        }
    }
}
