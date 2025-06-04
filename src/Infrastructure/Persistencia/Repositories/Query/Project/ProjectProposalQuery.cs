using Application.Common.Interface.Infrastructure;
using Application.UseCase.ProjectProposals.Querys.FilterParameter;
using Domain.Common.ResultPattern;
using Domain.Entity;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistencia.Repositories.Query.Project
{
    public class ProjectProposalQuery : IProjectProposalQuery
    {
        private readonly IRepositoryQuery _repository;

        public ProjectProposalQuery(IRepositoryQuery repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Result<ProjectProposal>> GetByIdWithDetails(Guid id)
        {
            try
            {
                var result = await _repository.Query<ProjectProposal>()
                    .Include(p => p.AreaEntity)
                    .Include(p => p.TypeEntity)
                    .Include(p => p.ApprovalStatus)
                    .FirstOrDefaultAsync(p => p.Id == id);
                return new Success<ProjectProposal>(result);
            }
            catch (Exception ex)
            {
                return new Failed<ProjectProposal>($"Error al obtener el proyecto:");
            }
        }

        public async Task<Result<List<ProjectProposal>>> GetFilteredProjectsAsync(ProjectProposalFilter filter)
        {
            try
            {
                var query = _repository.Query<ProjectProposal>();

                if (!string.IsNullOrWhiteSpace(filter.Title))
                {
                    query = query.Where(p => p.Title.Contains(filter.Title));
                }

                if (filter.Status.HasValue && filter.Status > 0)
                {
                    query = query.Where(p => p.Status == filter.Status.Value);
                }

                if (filter.Applicant.HasValue && filter.Applicant > 0)
                {
                    query = query.Where(p => p.CreateBy == filter.Applicant.Value);
                }

                if (filter.ApprovalUser.HasValue && filter.ApprovalUser > 0)
                {
                    query = query.Where(p => p.ApprovalSteps.Any(s => s.ApproverUser.Id == filter.ApprovalUser));
                }

                var result = await query
                    .Include(p => p.AreaEntity)
                    .Include(p => p.TypeEntity)
                    .Include(p => p.ApprovalStatus)
                    .AsNoTracking()
                    .ToListAsync();
                return new Success<List<ProjectProposal>>(result);
            }
            catch (Exception ex)
            {
                return new Failed<List<ProjectProposal>>("Error al filtrar proyectos");
            }
        }

        public async Task<List<ProjectProposal>> GetAllProjectsForStatusAsync(int status)
        {
            return await _repository.Query<ProjectProposal>()
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
            return await _repository.Query<ProjectProposal>()
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

            return _repository.Query<ProjectProposal>()
                .AsNoTracking()
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