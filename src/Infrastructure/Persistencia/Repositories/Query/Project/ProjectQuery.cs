using System.Linq.Expressions;
using Application.Common.Interface;
using Application.Common.Interface.Infrastructure;
using Domain.Entity;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistencia.Repositories.Query.Project
{
    public class ProjectQuery : IProjectQueryService
    {
        private readonly IRepositoryQuery _context;

        public ProjectQuery(IRepositoryQuery context)
        {
            _context = context;
        }
        public async Task<List<ProjectProposal>> GetAllProjectsForStatusAsync(int status)
        {
          return  await _context.Query<ProjectProposal>()
                .Include(p => p.AreaEntity)
                .Include(p => p.TypeEntity)
                .Include(p => p.ApprovalStatus)
                .Include(p => p.CreateBy)
                .Include(p => p.ApprovalSteps)
                .Where(p => p.Status == status)
                .ToListAsync();
           
        }
        public async Task<List<ProjectProposal>> GetProjectsCreatedByUserAsync(int createdByUserId)
        {
          return  await _context.Query<ProjectProposal>()
                .Include(p => p.AreaEntity)
                .Include(p => p.TypeEntity)
                .Include(p => p.ApprovalStatus)
                .Include(p => p.User)
                .Include(p => p.ApprovalSteps)
                    .ThenInclude(s => s.ApproverRole)
                .Include(p => p.ApprovalSteps)
                    .ThenInclude(s => s.ApproverUser)
                .Where(p => p.CreateBy == createdByUserId)
                .ToListAsync();
        }


        public List<ProjectProposal> GetProjectForAprobation(User user)
        {
            var role = user.Role;
            var filter = FilterPendingOrObservedSteps(role);

            var propuestas = _context.Query<ProjectProposal>()
           .AsNoTracking() // Mejora el rendimiento si no necesitas modificar las entidades
           .Where(filter)
           .Include(p => p.AreaEntity) 
           .Include(p => p.TypeEntity) 
           .Include(p => p.ApprovalStatus) 
           .Include(p => p.User) 
           .Include(p => p.ApprovalSteps)
               .ThenInclude(s => s.ApproverRole) 
           .Include(p => p.ApprovalSteps)
               .ThenInclude(s => s.ApproverUser)
           .ToList();

            return propuestas;
        }

        private static Expression<Func<ProjectProposal, bool>> FilterPendingOrObservedSteps(int role)
        {
            return proposal => proposal.ApprovalSteps.Any(s =>
                (s.Status == (int)StatusEnum.Pending || s.Status == (int)StatusEnum.Observed) &&
                s.ApproverRoleId == role &&
                (
                    s.StepOrder == 1 ||
                    proposal.ApprovalSteps.Any(previousStep =>
                        previousStep.ProjectProposalId == s.ProjectProposalId &&
                        previousStep.StepOrder == s.StepOrder - 1 &&
                        previousStep.Status == (int)StatusEnum.Approved
                    )
                )
            );
        }


    }
}
