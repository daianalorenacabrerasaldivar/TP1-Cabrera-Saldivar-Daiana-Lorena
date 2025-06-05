using Application.Common.Interface.Presentation;
using Application.UseCase.AprovalStep.Update;
using Application.UseCase.ProjectProposals.Commands.Update;
using Application.UseCase.ProjectProposals.Querys.FilterParameter;
using Domain.Enum;
using MediatR;

namespace Application.UsesCases.CommandPatern
{
    public class ApproveProjectCommand : IMenuOptionCommand
    {
        public string Name => "Aprobar un proyecto";
        //private readonly IProjectQueryService _projectQueryService;
        private readonly IProjectConsolePresenter _consoleDisplay;
        private readonly IUserInteractionService _userInteractionService;
        private readonly IUserSessionService _userSessionService;
        //private readonly IAprobationAProjectUIpdate _aprobationAProjectUIpdate;
        private readonly IProjectSelectionService _projectSelectionService;

        private readonly IMediator _mediator;
        public ApproveProjectCommand(
            IProjectQueryService projectQueryService,
            IProjectConsolePresenter consoleDisplay,
            IUserInteractionService userInteractionService,
            IUserSessionService userSessionService,
            IMediator mediator,
             //IAprobationAProjectUIpdate aprobationAProjectUIpdate,
             IProjectSelectionService projectSelection)
        {
            //_projectQueryService = projectQueryService;
            _consoleDisplay = consoleDisplay;
            _userInteractionService = userInteractionService;
            _userSessionService = userSessionService;
            //_aprobationAProjectUIpdate = aprobationAProjectUIpdate;
            _projectSelectionService = projectSelection;
            _mediator = mediator;
        }


        public async Task ExecuteAsync()
        {

            _userInteractionService.ShowMessage("Ver proyectos Pendientes a Aprobar:");
            ProjectProposalFilter projectProposalFilter = new ProjectProposalFilter
            {
                Status = new List<int> { (int)StatusEnum.Pending, (int)StatusEnum.Observed },
                ApprovalUser = _userSessionService.GetActiveUser().Id
            };
            var pendingProjects = await _mediator.Send(projectProposalFilter);
            if (pendingProjects.IsFailed)
            {
                _userInteractionService.ShowMessage($"Ocurrio un error al obtener proyectos pendientes de aprobacion");
                return;
            }

            if (pendingProjects == null || !pendingProjects.Value.Any())
            {
                _userInteractionService.ShowMessage("No hay proyectos pendientes de aprobación.");
                return;
            }
            _userInteractionService.ShowMessage("Seleccione un proyecto para aprobar o rechazar:");

            var selectedProject = await  _projectSelectionService.SelectProjectAsync(pendingProjects.Value);
            if (selectedProject == Guid.Empty)
            {
                return;
            }

            int newStatus = GetNewStatus();
            if (newStatus == 0)
            {
                _userInteractionService.ShowMessage("Volviendo al menú principal...");
                return;
            }
            var observation = _userInteractionService.GetInput("Ingrese observaciones (opcional):");

            var projectUpdate= new UpdateApprovalStepCommand
            { 
                ProjectId = selectedProject,
               /* StepId = 0, */// Assuming step ID is not needed for this operation
                UserId = _userSessionService.GetActiveUser().Id,
                Status = newStatus,
                Observation = observation
            }
            ;
            var response = await _mediator.Send(projectUpdate);
            var result = await _aprobationAProjectUIpdate.UpdateProjectStatusAsync(selectedProject, newStatus, observation, _userSessionService.GetActiveUser());

            if (result.IsFailed)
            {
                _userInteractionService.ShowMessage($"\nError al actualizar el estado: {result.Info}");
            }
            _userInteractionService.ShowMessage(result.Value);
        }



        private int GetNewStatus()
        {
            _userInteractionService.ShowMessage("Seleccione una opción:");
            _userInteractionService.ShowMessage("1. Aprobar");
            _userInteractionService.ShowMessage("2. Rechazar");
            _userInteractionService.ShowMessage("3. Volver al menu");
            var option = Console.ReadLine();

            int newStatus = 0;

            do
            {
                if (option == "1")
                {
                    newStatus = (int)StatusEnum.Approved;
                }
                else if (option == "2")
                {
                    newStatus = (int)StatusEnum.Rejected;
                }
                else if (option == "3")
                {
                    newStatus = 0;
                }
                else
                {
                    _userInteractionService.ShowMessage("Opción no válida. Intente nuevamente.");
                    option = Console.ReadLine();
                }
            } while (option != "1" && option != "2" && option != "3");
            return newStatus;
        }
    }
}