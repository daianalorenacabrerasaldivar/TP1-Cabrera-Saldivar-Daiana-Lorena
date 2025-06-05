using Application.UseCase.AprovalStep.Update;
using Domain.Common.ResultPattern;
using Domain.Entity;

namespace Application.Common.Interface
{
    public interface IProjectApprovalStepUpdateValidator
    {
        Task<Result<ProjectApprovalStep>> TryGetUpdatableApprovalStepAsync(ProjectProposal project, UpdateApprovalStepCommand request);
    }
}

