using Application.Common.Interface;
using Domain.Dto;
using Domain.Entity;
using Domain.Enum;

namespace Application.Service.StatusProjectAndStep
{
    public class GetProjectApprovalStep : IGetProjectApprovalStep
    {

        public AprovalStepDto FindApplicableApprovalStepDto(
            ProjectProposalResponse project,
            int userId,
            int userRoleId)
        {
            return project.Steps
                .Where(step =>
                    (step.Status.Id == (int)StatusEnum.Pending || step.Status.Id == (int)StatusEnum.Observed) &&
                    (step.ApproverUser?.Id == userId ||
                     (step.ApproverUser == null && step.ApproverRole.Id == userRoleId))
                )
                .OrderBy(step => step.StepOrder)
                .FirstOrDefault();
        }


        public  ProjectApprovalStep FindApplicableApprovalStep(
            ProjectProposal project,
            int userId,
            int userRoleId, long stepId)
        {
            return project.ApprovalSteps
                .Where(step =>
                    (step.Status == (int)StatusEnum.Pending || step.Status == (int)StatusEnum.Observed) &&
                    (step.ApproverUserId == userId ||
                     (step.ApproverUserId == null && step.ApproverRoleId == userRoleId))
                     &&
                     step.Id == stepId

                )
                .OrderBy(step => step.StepOrder)
                .FirstOrDefault();
        }
    }
}
