using Application.Common.Interface.Infrastructure;
using Application.Mapper;
using Domain.Common.ResultPattern;
using MediatR;

namespace Application.UseCase.ProjectProposals.Querys.FilterParameter
{
    public class ProjectProposalQuerHandle : IRequestHandler<ProjectProposalFilter, Result<List<GetProjectResponse>>>
    {
        private readonly IProjectProposalQuery _projectRepository;

        public ProjectProposalQuerHandle(
            IProjectProposalQuery projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Result<List<GetProjectResponse>>> Handle(ProjectProposalFilter request, CancellationToken cancellationToken)
        {
            try
            {
                if (!IsValidFilter(request))
                {
                    return new Failed<List<GetProjectResponse>>("Al menos un filtro debe ser proporcionado.");
                }


                var projectsResult = await _projectRepository.GetFilteredProjectsAsync(request);

                if (projectsResult.IsFailed)
                {
                    return new Failed<List<GetProjectResponse>>(projectsResult.Info);
                }
                if (projectsResult.Value == null || !projectsResult.Value.Any())
                {
                    return new Success<List<GetProjectResponse>>(new List<GetProjectResponse>());
                }
                var mappedProjects = GetProjectResponseMapper.MapToResponse(projectsResult.Value);

                return new Success<List<GetProjectResponse>>(mappedProjects);
            }
            catch (Exception ex)
            {
                return new Failed<List<GetProjectResponse>>("Error interno al procesar la búsqueda.");
            }
        }

        private static bool IsValidFilter(ProjectProposalFilter filter)
        {
            return !string.IsNullOrEmpty(filter.Title) ||
                   filter.Status != null && filter.Status.Count > 0 ||
                   filter.Applicant.HasValue ||
                   filter.ApprovalUser.HasValue;
        }

    }
}