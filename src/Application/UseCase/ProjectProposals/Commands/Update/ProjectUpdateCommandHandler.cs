using Application.Common.Interface.Infrastructure;
using Application.Mapper;
using Domain.Common;
using Domain.Dto;
using Domain.Entity;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCase.ProjectProposals.Commands.Update
{
    public class ProjectUpdateCommandHandler : IRequestHandler<ProjectUpdateCommand, ResponseCodeAndObject<ProjectProposalResponse>>
    {
        private readonly IRepositoryCommand _repositoryCommand;
        private readonly IRepositoryQuery _repositoryQuery;
        public ProjectUpdateCommandHandler(IRepositoryCommand repositoryCommand, IRepositoryQuery repositoryQuery)
        {
            _repositoryCommand = repositoryCommand;
            _repositoryQuery = repositoryQuery;
        }

        public async Task<ResponseCodeAndObject<ProjectProposalResponse>> Handle(ProjectUpdateCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.title) && string.IsNullOrWhiteSpace(request.description) && request.duration <= 0)
            {
                throw new ArgumentException("Datos del proyecto inválidos");
            }
            var project = _repositoryQuery.Query<ProjectProposal>()
                 .FirstOrDefault(x => x.Id == request.ProjectId);
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
            project = _repositoryQuery.Query<ProjectProposal>()
            .Include(x => x.AreaEntity)
            .Include(x => x.TypeEntity)
            .Include(x => x.ApprovalSteps)
                .ThenInclude(step => step.ApproverUser)
            .Include(x => x.ApprovalSteps)
                .ThenInclude(step => step.ApproverRole)
            .Include(x => x.ApprovalSteps)
                .ThenInclude(step => step.ApprovalStatus)
            .Include(x => x.ApprovalStatus)
            .Include(x => x.User)
            .FirstOrDefault(x => x.Id == request.ProjectId);
            var response = MapperProposal.MapToProposalResponse(project);
            return new ResponseCodeAndObject<ProjectProposalResponse> { Response = response };

        }
    }
}
