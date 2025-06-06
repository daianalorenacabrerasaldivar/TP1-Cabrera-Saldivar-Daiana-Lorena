using Application.Common.Interface.Presentation;
using Application.UseCase.ProjectProposals.Querys.FilterParameter;
using Application.UseCase.ProjectProposals.Querys.ProjectById;
using MediatR;

namespace Consola.PresentacionCommon
{
    public class ProjectSelectionService : IProjectSelectionService
    {
        private readonly IConsoleUserInteractionService _interactionService;
        private readonly IProjectConsolePresenter _projectConsolePresenter;
        private readonly IMediator _mediator;

        public ProjectSelectionService(IConsoleUserInteractionService userInteractionService, IProjectConsolePresenter projectConsolePresenter, IMediator mediator)
        {
            _interactionService = userInteractionService;
            _projectConsolePresenter = projectConsolePresenter;
            _mediator = mediator;

        }
        public async Task<Guid> SelectProjectAsync(List<GetProjectResponse> projects)
        {
            if (projects == null || !projects.Any())
            {
                _interactionService.ShowMessage("No hay proyectos disponibles para seleccionar.");
                return Guid.Empty;
            }

            _projectConsolePresenter.ShowProjectsSummary(projects);

            while (true)
            {
                string selectOption = _interactionService.GetInput("\n¿Desea ver un proyecto en detalle? (S/N)").ToUpper();

                if (selectOption == "N")
                {
                    return Guid.Empty;
                }

                if (selectOption != "S")
                {
                    _interactionService.ShowMessage("Opción inválida. Por favor, ingrese S o N.");
                    continue;
                }

                int index = _interactionService.GetValidatedIntegerMaxMin(
                    "Ingrese el número del proyecto:",
                    1,
                    projects.Count);

                var projectId = projects[index - 1].Id;
                var query = new GetProjectByIdQuery { Id = projectId };
                var projectDetail = await _mediator.Send(query);
                _projectConsolePresenter.ShowProjectProposalDetails(projectDetail);

                if (selectOption == "S")
                {
                    return projectId;
                }
            }
        }


    }
}
