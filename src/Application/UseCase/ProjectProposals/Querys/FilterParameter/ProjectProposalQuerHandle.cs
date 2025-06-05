using Application.Common.Interface.Infrastructure;
using Application.Mapper;
using Domain.Common.ResultPattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.ProjectProposals.Querys.FilterParameter
{
    public class ProjectProposalQuerHandle : IRequestHandler<ProjectProposalFilter, Result<List<GetProjectResponse>>>
    {
        private readonly IProjectProposalQuery _projectRepository;
        private readonly ILogger<ProjectProposalQuerHandle> _logger;

        public ProjectProposalQuerHandle(
            IProjectProposalQuery projectRepository,
            ILogger<ProjectProposalQuerHandle> logger)
        {
            _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<List<GetProjectResponse>>> Handle(ProjectProposalFilter request, CancellationToken cancellationToken)
        {
            try
            {
                if (!IsValidFilter(request))
                {
                    _logger.LogWarning("Filtro inválido: se requiere al menos un criterio de búsqueda");
                    return new Failed<List<GetProjectResponse>>("Al menos un filtro debe ser proporcionado.");
                }

                _logger.LogInformation("Iniciando búsqueda de proyectos con filtros");

                var projectsResult = await _projectRepository.GetFilteredProjectsAsync(request);

                if (projectsResult.IsFailed)
                {
                    _logger.LogError("Error al obtener proyectos filtrados: {Message}", projectsResult.Info);
                    return new Failed<List<GetProjectResponse>>(projectsResult.Info);
                }
                if (projectsResult.Value == null || !projectsResult.Value.Any())
                {
                    return new Failed<List<GetProjectResponse>>("No se encontraron proyectos con los filtros proporcionados");
                }
                var mappedProjects = GetProjectResponseMapper.MapToResponse(projectsResult.Value);

                return new Success<List<GetProjectResponse>>(mappedProjects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al procesar la búsqueda de proyectos");
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