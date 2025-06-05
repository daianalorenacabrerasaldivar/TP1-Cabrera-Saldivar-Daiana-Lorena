using Application.UseCase.ProjectProposals.Querys.FilterParameter;
using Domain.Common.ResultPattern;
using Domain.Entity;

namespace Application.Common.Interface.Infrastructure
{
    public interface IProjectProposalQuery
    {
        public Task<Result<ProjectProposal>> GetProjectProposalByIdAsync(Guid id);
        public Task<Result<List<ProjectProposal>>> GetFilteredProjectsAsync(ProjectProposalFilter filter);
    }
}
