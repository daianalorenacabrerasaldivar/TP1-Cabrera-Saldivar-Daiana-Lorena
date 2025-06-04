using Application.UseCase.ProjectProposals.Commands.Create;
using Domain.Common.ResultPattern;
using Domain.Entity;

namespace Application.Common.Interface
{
    public interface ICreateProjectProposalService
    {
        Task<Result<ProjectProposal>> CreateProjectWithApprovalFlowAsync(CreateProjectProposalCommand projectProposal);
    }
}