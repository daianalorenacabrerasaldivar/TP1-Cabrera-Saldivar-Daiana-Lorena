using Application.Common.Interface.Infrastructure;
using Application.Mapper;
using Domain.Common;
using Domain.Dto;
using Domain.Enum;
using MediatR;

namespace Application.UseCase.ProjectProposals.Commands.Update
{
    public class ProjectUpdateCommandHandler : IRequestHandler<ProjectUpdateCommand, ResponseCodeAndObject<ProjectProposalResponse>>
    {
        private readonly IRepositoryCommand _repositoryCommand;
        private readonly IProjectProposalQuery _projectProposalQuery;
        public ProjectUpdateCommandHandler(IRepositoryCommand repositoryCommand, IProjectProposalQuery projectProposalQuery)
        {
            _repositoryCommand = repositoryCommand;
            _projectProposalQuery = projectProposalQuery;
        }

        public async Task<ResponseCodeAndObject<ProjectProposalResponse>> Handle(ProjectUpdateCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.title) && string.IsNullOrWhiteSpace(request.description) && request.duration <= 0)
            {
                throw new ArgumentException("Datos del proyecto inválidos");
            }
            var projectResul = await _projectProposalQuery.GetProjectProposalByIdAsync(request.ProjectId);
            var project = projectResul.Value;
            if (project == null)
            {
                throw new ArgumentException("Proyecto no encontrado");
            }
            if (project.Status == (int)StatusEnum.Approved || project.Status == (int)StatusEnum.Rejected)
            {
                return new ResponseCodeAndObject<ProjectProposalResponse>
                {
                    httpStatusCode = System.Net.HttpStatusCode.Conflict,
                    Message = "No se puede actualizar un proyecto que ya ha sido aprobado o rechazado"
                };
            }
            if (!string.IsNullOrEmpty(request.title))
            {
                project.Title = request.title;
            }
            if (!string.IsNullOrEmpty(request.description))
            {
                project.Description = request.description;
            }
            if (request.duration > 0)
            {
                project.EstimatedDuration = request.duration;
            }
            _repositoryCommand.Update(project);
            var resultSave = await _repositoryCommand.SaveAsync();
            if (resultSave.IsFailed)
            {
                throw new Exception("Error al actualizar el proyecto");
            }

            var projectActualizado = await _projectProposalQuery.GetProjectProposalByIdAsync(request.ProjectId);
            var response = MapperProposal.MapToProposalResponse(projectActualizado);
            return new ResponseCodeAndObject<ProjectProposalResponse> { Response = response };

        }
    }
}
