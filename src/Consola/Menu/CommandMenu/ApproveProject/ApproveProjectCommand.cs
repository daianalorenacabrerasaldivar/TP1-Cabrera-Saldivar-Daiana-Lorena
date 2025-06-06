using Application.Common.Interface;
using Application.Common.Interface.Presentation;
using Application.UseCase.AprovalStep.Update;
using Application.UseCase.ProjectProposals.Querys.FilterParameter;
using Application.UseCase.ProjectProposals.Querys.ProjectById;
using Domain.Enum;
using MediatR;

namespace Consola.Menu.CommandMenu.ApproveProject
{

    public class ApproveProjectCommand : IMenuOptionCommand
    {
        public string Name => "Aprobar un proyecto";

        private readonly IProjectConsolePresenter _consolePresenter;
        private readonly IConsoleUserInteractionService _userInteractionService;
        private readonly IUserSessionService _userSessionService;
        private readonly IProjectSelectionService _projectSelectionService;
        private readonly IGetProjectApprovalStep _getProjectApprovalStep;
        private readonly IMediator _mediator;

        public ApproveProjectCommand(
            IProjectConsolePresenter consolePresenter,
            IConsoleUserInteractionService userInteractionService,
            IUserSessionService userSessionService,
            IMediator mediator,
            IProjectSelectionService projectSelectionService,
            IGetProjectApprovalStep getProjectApprovalStep)
        {
            _consolePresenter = consolePresenter;
            _userInteractionService = userInteractionService;
            _userSessionService = userSessionService;
            _projectSelectionService = projectSelectionService;

            _mediator = mediator;
            _getProjectApprovalStep = getProjectApprovalStep;
        }

        public async Task ExecuteAsync()
        {

            var pendingProjects = await GetPendingProjectsForApprovalAsync();
            if (pendingProjects == null || !pendingProjects.Any())
            {
                _userInteractionService.ShowMessage("No hay proyectos pendientes de aprobación.");
                return;
            }

            _userInteractionService.ShowMessage("Seleccione un proyecto para aprobar o rechazar:");

            var selectedProject = await _projectSelectionService.SelectProjectAsync(pendingProjects);
            if (selectedProject == Guid.Empty)
            {
                return;
            }

            int newStatus = GetNewStatus();
            if (newStatus == 0)
            {
                _userInteractionService.ShowMessage("Volviendo al menú principal..");
                return;
            }

            var observation = _userInteractionService.GetInput("Ingrese observaciones (opcional):");

            var result = await UpdateProjectStatusAsync(selectedProject, newStatus, observation);

            _userInteractionService.ShowMessage(result ?
                $"El proyecto ha sido {(newStatus == (int)StatusEnum.Approved ? "aprobado" : "rechazado")} correctamente." :
                "Ha ocurrido un error al actualizar el proyecto.");
        }

        private async Task<List<GetProjectResponse>> GetPendingProjectsForApprovalAsync()
        {
            var filter = new ProjectProposalFilter
            {
                Status = new List<int> { (int)StatusEnum.Pending, (int)StatusEnum.Observed },
                ApprovalUser = _userSessionService.GetActiveUser().Id
            };

            var result = await _mediator.Send(filter);

            if (result.IsFailed)
            {
                _userInteractionService.ShowMessage($"Error al obtener proyectos pendientes: {result.Info}");
                return null;
            }

            return result.Value;
        }

        private async Task<bool> UpdateProjectStatusAsync(Guid projectId, int status, string observation)
        {
            var project = await _mediator.Send(new GetProjectByIdQuery { Id = projectId });
            var activeUser = _userSessionService.GetActiveUser();
            var steptResult = _getProjectApprovalStep.FindApplicableApprovalStepDto(project, activeUser.Id, activeUser.Role);
            var command = new UpdateApprovalStepCommand
            {
                ProjectId = projectId,
                UserId = activeUser.Id,
                Status = status,
                StepId = steptResult.Id,
                Observation= observation
            };

            try
            {
                var response = await _mediator.Send(command);
                return response.httpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _userInteractionService.ShowMessage($"Error al actualizar el proyecto: {ex.Message}");
                return false;
            }
        }

        private int GetNewStatus()
        {
            string[] options = new[] { "Aprobar", "Rechazar", "Volver al menú" };
            _userInteractionService.ShowMessage("Seleccione una opción:");

            for (int i = 0; i < options.Length; i++)
            {
                _userInteractionService.ShowMessage($"{i + 1}. {options[i]}");
            }

            int selection = _userInteractionService.GetValidatedIntegerMaxMin("Seleccione una opción:", 1, options.Length);

            return selection switch
            {
                1 => (int)StatusEnum.Approved,
                2 => (int)StatusEnum.Rejected,
                _ => 0 // Volver al menú
            };
        }
    }
}