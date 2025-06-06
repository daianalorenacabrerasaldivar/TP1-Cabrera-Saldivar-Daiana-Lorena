using Domain.Dto;
using Domain.Entity;

namespace Application.Common.Interface
{
    public interface IGetProjectApprovalStep
    {
        ProjectApprovalStep FindApplicableApprovalStep(ProjectProposal project, int userId, int userRoleId, long stepId);
        AprovalStepDto FindApplicableApprovalStepDto(ProjectProposalResponse project, int userId, int userRoleId);

    }
}