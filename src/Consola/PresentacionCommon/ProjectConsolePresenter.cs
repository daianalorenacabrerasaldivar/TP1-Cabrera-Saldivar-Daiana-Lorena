using Application.Common.Interface.Presentation;
using Application.UseCase.ProjectProposals.Querys.FilterParameter;
using Consola.Helpers;
using Domain.Dto;
using Domain.Entity;

namespace Consola.PresentacionCommon
{
    public class ProjectConsolePresenter : IProjectConsolePresenter
    {
        private readonly IUserInteractionService _userInteractionService;

        public ProjectConsolePresenter(IUserInteractionService userInteractionService)
        {
            _userInteractionService = userInteractionService;
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

        public void ShowProjectsSummary(List<GetProjectResponse> projects)
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
                string statusName = project.Status ?? "No especificado";
                string projectType = project.Type ?? "Sin tipo";
                string areaName = project.Area ?? "Sin área";
                _userInteractionService.ShowMessage($"{index}. Proyecto:");
                Console.Write($" Título: {project.Title}");
                Console.Write($" Estado: {statusName}");
                Console.Write($" Tipo: {projectType}");
                Console.Write($" Área: {areaName} . ");
                Console.Write($" Monto estimado: ${project.Amount:N2}");
                Console.Write($" Duración estimada: {project.Duration} días");
                _userInteractionService.ShowMessage("-----------------------------------------------");
                index++;
            }

            _userInteractionService.ShowMessage("===============================================");
            _userInteractionService.ShowMessage($"Total de proyectos: {projects.Count}");
            _userInteractionService.ShowMessage("===============================================");
        }
        public void ShowProjectProposalDetails(ProjectProposalResponse project)
        {
            _userInteractionService.ShowMessage("===============================================");
            _userInteractionService.ShowMessage($"Nombre: {project.Title}");
            _userInteractionService.ShowMessage($"Descripción: {project.Description}");
            _userInteractionService.ShowMessage($"Solicitante: {project.User?.Name ?? "No especificado"}");
            _userInteractionService.ShowMessage($"Tipo de proyecto: {project.Type?.Name ?? "No especificado"}");
            _userInteractionService.ShowMessage($"Área: {project.Area?.Name ?? "No especificada"}");
            _userInteractionService.ShowMessage($"Estado: {project.Status?.Name ?? "No especificado"}");
            _userInteractionService.ShowMessage($"Monto estimado: ${project.Amount:N2}");
            _userInteractionService.ShowMessage($"Duración estimada: {project.Duration} días");

            if (project.Steps != null && project.Steps.Any())
            {
                _userInteractionService.ShowMessage("\nPasos de aprobación:");
                _userInteractionService.ShowMessage("-----------------------------------------");

                foreach (var step in project.Steps.OrderBy(s => s.StepOrder))
                {
                    string statusName = step.Status.Name;
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
