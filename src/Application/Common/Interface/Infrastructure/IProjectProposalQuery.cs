using Application.UseCase.ProjectProposals.Querys.FilterParameter;
using Domain.Common.ResultPattern;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interface.Infrastructure
{
    public interface IProjectProposalQuery
    {
      public  Task<Result<ProjectProposal>> GetByIdWithDetails(Guid id);
       public Task<Result<List<ProjectProposal>>> GetFilteredProjectsAsync(ProjectProposalFilter filter);
    }
}
