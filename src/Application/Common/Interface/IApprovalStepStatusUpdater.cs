using Application.UseCase.AprovalStep.Update;
using Domain.Entity;

namespace Application.Common.Interface
{
    public interface IApprovalStepStatusUpdater
    {
        Task<ProjectProposal> UpdateProposalAndStepAsync(ProjectProposal projectProposal, ProjectApprovalStep approvalStep, UpdateApprovalStepCommand request);
    }
}