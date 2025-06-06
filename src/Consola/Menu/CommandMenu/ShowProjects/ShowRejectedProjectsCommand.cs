using Application.Common.Interface;
using Application.Common.Interface.Presentation;
using Application.UseCase.ProjectProposals.Querys.FilterParameter;
using Domain.Enum;
using MediatR;

namespace Consola.Menu.CommandMenu.ShowProjects
{
    public class ShowRejectedProjectsCommand : IMenuOptionCommand
    {
        public string Name => "Ver proyectos Rechazados";

        private readonly IConsoleUserInteractionService _userInteractionService;
        private readonly IUserSessionService _userSessionService;
        private readonly IProjectSelectionService _projectSelectionService;
        private readonly IProjectConsolePresenter _projectConsolePresenter;
        private readonly IMediator _mediator;
        public ShowRejectedProjectsCommand(
            IConsoleUserInteractionService userInteractionService,
            IUserSessionService userSessionService,
            IProjectSelectionService projectSelectionService,
            IProjectConsolePresenter projectConsolePresenter,
            IMediator mediator)
        {
            _userInteractionService = userInteractionService;
            _userSessionService = userSessionService;
            _projectSelectionService = projectSelectionService;
            _projectConsolePresenter = projectConsolePresenter;
            _mediator = mediator;
        }

        public async Task ExecuteAsync()
        {
            try
            {
                _userInteractionService.ShowMessage("Ver proyectos Rechazados");

                var activeUser = _userSessionService.GetActiveUser();
                if (activeUser == null)
                {
                    _userInteractionService.ShowMessage("No hay un usuario activo. Por favor inicie sesión primero.");
                    return;
                }

                var filter = new ProjectProposalFilter
                {
                    Status = new List<int> { (int)StatusEnum.Rejected },
                    ApprovalUser = activeUser.Id
                };

                var projectsResult = await _mediator.Send(filter);

                if (projectsResult.IsFailed)
                {
                    _userInteractionService.ShowMessage($"Error al obtener proyectos: {projectsResult.Info}");
                    return;
                }

                var myProjects = projectsResult.Value;

                if (myProjects == null || !myProjects.Any())
                {
                    _userInteractionService.ShowMessage("No tienes proyectos que haya Rechazados.");
                    return;
                }

                _userInteractionService.ShowMessage($"Tienes {myProjects.Count} proyecto(s):");

                var selectedProjectId = await _projectSelectionService.SelectProjectAsync(myProjects);

                if (selectedProjectId == Guid.Empty)
                {
                    _userInteractionService.ShowMessage("No se seleccionó ningún proyecto.");
                }
            }
            catch (Exception ex)
            {
                _userInteractionService.ShowMessage($"Error al consultar proyectos: {ex.Message}");
            }
        }
    }
}