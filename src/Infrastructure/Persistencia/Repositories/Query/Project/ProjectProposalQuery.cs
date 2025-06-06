using Application.Common.Interface.Infrastructure;
using Application.UseCase.ProjectProposals.Querys.FilterParameter;
using Domain.Common.ResultPattern;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistencia.Repositories.Query.Project
{
    public class ProjectProposalQuery : IProjectProposalQuery
    {
        private readonly IRepositoryQuery _repository;

        public ProjectProposalQuery(IRepositoryQuery repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Result<ProjectProposal>> GetProjectProposalByIdAsync(Guid id)
        {
            try
            {
                var result = await _repository.Query<ProjectProposal>()
                .Include(x => x.AreaEntity)
                .Include(x => x.TypeEntity)
                .Include(x => x.ApprovalStatus)
                .Include(x => x.ApprovalSteps)
                    .ThenInclude(x => x.ApproverUser)
                .Include(x => x.ApprovalSteps)
                    .ThenInclude(x => x.ApproverRole)
                .Include(x => x.ApprovalSteps)
                    .ThenInclude(x => x.ApprovalStatus)
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

                if (filter.Status != null && filter.Status.Count > 0)
                {
                    query = query.Where(p => filter.Status.Contains(p.Status));
                }

                if (filter.Applicant.HasValue && filter.Applicant > 0)
                {
                    query = query.Where(p => p.CreateBy == filter.Applicant.Value);
                }

                if (filter.ApprovalUser.HasValue && filter.ApprovalUser > 0)
                {
                    var approverUserStep = _repository.Query<User>()
                     .AsNoTracking()
                     .Include(u => u.ApproverRole)
                     .FirstOrDefault(u => u.Id == filter.ApprovalUser);

                    if (approverUserStep != null)
                    {
                        query = query.Where(p => p.ApprovalSteps
                        .Any(
                            step => step.ApproverUser == approverUserStep || //es aprobador
                            (step.ApproverUser == null && step.ApproverRole == approverUserStep.ApproverRole) //es un rol de aprobador

                        ));
                    }

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


    }
}