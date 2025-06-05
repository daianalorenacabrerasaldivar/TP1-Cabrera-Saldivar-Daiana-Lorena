using Application.Common.Exceptions;
using Application.Common.Interface.Presentation;
using Application.UseCase.Areas.Queries;
using Application.UseCase.ProjectProposals.Commands.Create;
using Application.UseCase.ProjectTypes.Queries;
using Domain.Dto;
using MediatR;

namespace Consola.Menu.CommandMenu.CreateProject
{
    public class CreateProjectConsoleAction : IMenuOptionCommand
    {
        public string Name => "Crear Proyecto";

        private readonly IUserInteractionService _userInteractionService;
        private readonly IProjectConsolePresenter _projectConsolePresenter;
        private readonly IUserSessionService _userSessionService;
        private readonly IMediator _mediator;

        public CreateProjectConsoleAction(
            IUserInteractionService userInteractionService,
            IProjectConsolePresenter projectConsolePresenter,
            IUserSessionService userSessionService,
            IMediator mediator
            )
        {
            _userInteractionService = userInteractionService;
            _projectConsolePresenter = projectConsolePresenter;
            _userSessionService = userSessionService;
            _mediator = mediator;
        }

        public async Task ExecuteAsync()
        {
            try
            {
                _userInteractionService.ShowMessage("Crear un nuevo proyecto:");
                string title = InputProjectTitle();
                string description = InputProjectDescription();
                var areas = await _mediator.Send(new GetAllAreasQuery()); ;
                AreaDto selectedArea = InputArea(areas);
                var types = await _mediator.Send(new GetAllProjectTypesQuery());
                ProjectTypeDto selectedType = InputProjectType(types);

                decimal estimatedAmount = InputEstimatedAmount();
                int estimatedDuration = InputEstimatedDuration();

                var projectCommand = new CreateProjectProposalCommand
                {
                    Title = title,
                    Description = description,
                    Area = selectedArea.Id,
                    Type = selectedType.Id,
                    Amount = estimatedAmount,
                    Duration = estimatedDuration,
                    User = _userSessionService.GetActiveUser().Id
                };
                var resultCreation = await _mediator.Send(projectCommand);


                _projectConsolePresenter
                    .ShowNewlyCreatedProjectDto(resultCreation);

                _userInteractionService.ShowMessage("\nProyecto creado exitosamente.");

            }
            catch (CustomResponseException ex)
            {
                _userInteractionService.ShowMessage(ex.Message);
            }
            catch (Exception ex)
            {
                _userInteractionService.ShowMessage($"\nError al crear el proyecto: {ex.Message}");
            }

        }

        private int InputEstimatedDuration()
        {
            int estimatedDuration = _userInteractionService.GetValidatedIntegerMaxMin("\nIngrese la duración estimada del proyecto (en días):", 0, int.MaxValue);
            return estimatedDuration;
        }
        private string InputProjectTitle()
        {
            string title;
            do
            {
                title = _userInteractionService.GetInput("\nIngrese el título del proyecto (máximo 255 caracteres):");

                if (string.IsNullOrEmpty(title))
                {
                    _userInteractionService.ShowMessage("Error: El título no puede estar vacío.");
                }
                else if (title.Length > 255)
                {
                    _userInteractionService.ShowMessage("Error: El título no puede exceder los 255 caracteres.");
                }
            } while (string.IsNullOrEmpty(title) || title.Length > 255);
            return title;
        }
        private string InputProjectDescription()
        {
            string description;
            do
            {
                description = _userInteractionService.GetInput("\nIngrese la descripción del proyecto:");
                if (string.IsNullOrEmpty(description))
                {
                    _userInteractionService.ShowMessage("Error: La descripción no puede estar vacía.");
                }
            } while (string.IsNullOrEmpty(description));

            return description;
        }
        private AreaDto InputArea(List<AreaDto> areas)
        {
            if (areas?.Count < 1)
                throw new CustomResponseException("No hay Areas disponibles");

            string message = "\nSeleccione el área del proyecto:\n";
            _userInteractionService.ShowMessage("\nSeleccione el área del proyecto:");
            for (int i = 0; i < areas.Count; i++)
            {
                message += $"{i + 1}. {areas[i].Name}\n";
            }
            message += "Ingrese el número del área:";

            int areaIndex = _userInteractionService.GetValidatedIntegerMaxMin(message, 1, areas.Count);

            return areas[areaIndex - 1];
        }
        private ProjectTypeDto InputProjectType(List<ProjectTypeDto> types)
        {
            if (types?.Count < 1)
                throw new CustomResponseException("No hay tipos de proyecto disponibles");

            string message = "\nSeleccione el tipo de proyecto:";
            for (int i = 0; i < types.Count; i++)
            {
                message += $"{i + 1}. {types[i].Name}";
            }

            int typeIndex = _userInteractionService.GetValidatedIntegerMaxMin(message, 1, types.Count);

            return types[typeIndex - 1];
        }
        private decimal InputEstimatedAmount()
        {
            return _userInteractionService.GetValidatedDecimalMaxMin("\nIngrese el monto estimado del proyecto:", 0, decimal.MaxValue);
        }



    }
}
