using Domain.Common.ResultPattern;
using Domain.Entity;

namespace Application.Common.Interface
{
    public interface IApprovalAssignmentService
    {
        Task<Result<List<ProjectApprovalStep>>> GetApprovalStepsForProposalAsync(ProjectProposal proposal);
    }

}