using Application.Common.Interface.Presentation;
using Consola.Helpers;
using Domain.Dto;
using Domain.Entity;

namespace Consola.Presentacion
{
    public class ProjectConsolePresenter : IProjectConsolePresenter
    {

        private readonly IUserInteractionService _userInteractionService;

        public ProjectConsolePresenter(IUserInteractionService userInteractionService)
        {
            _userInteractionService = userInteractionService;
        }

        public void ShowMenu()
        {
            Console.Clear();
            _userInteractionService.ShowMessage("===============================================");
            _userInteractionService.ShowMessage("\n--- Menú Principal ---");
            _userInteractionService.ShowMessage("Seleccione una opción:");
            _userInteractionService.ShowMessage("1. Crear un nuevo proyecto");
            _userInteractionService.ShowMessage("2. Aprobar un proyecto");
            _userInteractionService.ShowMessage("3. Ver Mis proyectos");
            _userInteractionService.ShowMessage("4. Ver proyectos Aprobados");
            _userInteractionService.ShowMessage("5. Ver proyectos Rechazados");
            _userInteractionService.ShowMessage("6. Cambiar de usuario");
            _userInteractionService.ShowMessage("7. Salir");
            _userInteractionService.ShowMessage("===============================================");
            _userInteractionService.ShowMessage("Por favor, selecciona una opción:");
        }
        public void ShowNewlyCreatedProjectDto(ProjectProposalResponse project)
        {
            _userInteractionService.ShowMessage($"Nombre: {project.Title}");
            _userInteractionService.ShowMessage($"Descripción: {project.Description}");
            _userInteractionService.ShowMessage($"Solicitante: {project.User?.Name ?? ""}");
            _userInteractionService.ShowMessage($"Tipo de proyecto: {project.Type?.Name ?? ""}");
            _userInteractionService.ShowMessage($"Área: {project.Area?.Name ?? ""}");
            _userInteractionService.ShowMessage($"Monto estimado: ${project.Amount:N2}");
            _userInteractionService.ShowMessage($"Duración estimada: {project.Duration} días");
        }

        public void ShowProjectsSummary(List<ProjectProposal> projects)
        {
            if (projects == null || !projects.Any())
            {
                _userInteractionService.ShowMessage("No hay proyectos para mostrar.");
                return;
            }

            _userInteractionService.ShowMessage("===============================================");
            _userInteractionService.ShowMessage("LISTADO DE PROYECTOS");
            _userInteractionService.ShowMessage("===============================================");

            int index = 1;
            foreach (var project in projects)
            {
                string statusName = project.ApprovalStatus?.Id != null ? Traduction.GetStatusName(project.ApprovalStatus.Id) : "No especificado";
                string projectType = project.TypeEntity?.Name ?? "Sin tipo";
                string areaName = project.AreaEntity?.Name ?? "Sin área";
                _userInteractionService.ShowMessage($"{index}. Proyecto:");
                Console.Write($" Título: {project.Title}");
                Console.Write($" Estado: {statusName}");
                Console.Write($" Tipo: {projectType}");
                Console.Write($" Área: {areaName} . ");
                Console.Write($" Monto estimado: ${project.EstimatedAmount:N2}");
                Console.Write($" Duración estimada: {project.EstimatedDuration} días");
                _userInteractionService.ShowMessage("-----------------------------------------------");
                index++;
            }

            _userInteractionService.ShowMessage("===============================================");
            _userInteractionService.ShowMessage($"Total de proyectos: {projects.Count}");
            _userInteractionService.ShowMessage("===============================================");
        }
        public void ShowProjectProposalDetails(ProjectProposal project)
        {
            _userInteractionService.ShowMessage("===============================================");
            _userInteractionService.ShowMessage($"Nombre: {project.Title}");
            _userInteractionService.ShowMessage($"Descripción: {project.Description}");
            _userInteractionService.ShowMessage($"Solicitante: {project.User?.Name ?? "No especificado"}");
            _userInteractionService.ShowMessage($"Tipo de proyecto: {project.TypeEntity?.Name ?? "No especificado"}");
            _userInteractionService.ShowMessage($"Área: {project.AreaEntity?.Name ?? "No especificada"}");
            _userInteractionService.ShowMessage($"Estado: {project.ApprovalStatus?.Name ?? "No especificado"}");
            _userInteractionService.ShowMessage($"Monto estimado: ${project.EstimatedAmount:N2}");
            _userInteractionService.ShowMessage($"Duración estimada: {project.EstimatedDuration} días");

            if (project.ApprovalSteps != null && project.ApprovalSteps.Any())
            {
                _userInteractionService.ShowMessage("\nPasos de aprobación:");
                _userInteractionService.ShowMessage("-----------------------------------------");

                foreach (var step in project.ApprovalSteps.OrderBy(s => s.StepOrder))
                {
                    string statusName = Traduction.GetStatusName(step.Status);
                    string approverInfo = step.ApproverUser != null
                        ? $"{step.ApproverUser.Name}"
                        : $"Rol: {step.ApproverRole?.Name ?? "No especificado"}";

                    _userInteractionService.ShowMessage($"Paso {step.StepOrder}: {approverInfo} - Estado: {statusName}");

                    if (!string.IsNullOrEmpty(step.Observations))
                    {
                        _userInteractionService.ShowMessage($"   Observaciones: {step.Observations}");
                    }

                    if (step.DecisionDate.HasValue)
                    {
                        _userInteractionService.ShowMessage($"   Fecha de decisión: {step.DecisionDate.Value:dd/MM/yyyy HH:mm}");
                    }
                }
            }
            else
            {
                _userInteractionService.ShowMessage("\nNo hay pasos de aprobación definidos para este proyecto.");
            }

        }


    }
}
