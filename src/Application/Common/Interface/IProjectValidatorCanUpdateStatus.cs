using Application.UseCase.AprovalStep.Update;
using Domain.Common.ResultPattern;
using Domain.Entity;

namespace Application.Common.Interface
{
    public interface IProjectValidatorCanUpdateStatus
    {
        Result<bool> CanUpdateStatus(ProjectProposal project, ProjectApprovalStep approvalStep, UpdateApprovalStepCommand updateApprovalStep);
    }
}