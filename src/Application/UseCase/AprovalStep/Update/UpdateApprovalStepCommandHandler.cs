using Application.Common.Interface;
using Application.Common.Interface.Infrastructure;
using Application.Mapper;
using Domain.Common;
using Domain.Dto;
using Domain.Entity;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Application.UseCase.AprovalStep.Update
{
    public class UpdateApprovalStepCommandHandler : IRequestHandler<UpdateApprovalStepCommand, ResponseCodeAndObject<ProjectProposalResponse>>
    {
        private readonly IRepositoryCommand _repositoryCommand;
        private readonly IRepositoryQuery _repositoryQuery;
        private readonly IProjectValidatorCanUpdateStatus _projectValidatorCanUpdateStatus;

        public UpdateApprovalStepCommandHandler(IRepositoryCommand repositoryCommand, IRepositoryQuery repositoryQuery, IProjectValidatorCanUpdateStatus projectValidatorCanUpdateStatus)
        {
            _repositoryCommand = repositoryCommand;
            _repositoryQuery = repositoryQuery;
            _projectValidatorCanUpdateStatus = projectValidatorCanUpdateStatus;
        }

        public async Task<ResponseCodeAndObject<ProjectProposalResponse>> Handle(UpdateApprovalStepCommand request, CancellationToken cancellationToken)
        {
            var validatio = UpdateApprovalStepValidator.Validate(request);
            if (validatio.IsFailed)
                throw new ArgumentException(validatio.Info);
            var projectId = request.ProjectId;
            var projectProposal = _repositoryQuery.Query<ProjectProposal>()
                .Include(x => x.ApprovalSteps)
                .FirstOrDefault(x => x.Id == projectId);

            if (projectProposal == null)
            {
                return new ResponseCodeAndObject<ProjectProposalResponse>
                {

                    Message = "Proyecto no encontrado"
                };
            }
            var approvalStep = _repositoryQuery.Query<ProjectApprovalStep>()
            .FirstOrDefault(x => x.ProjectProposalId == projectId && x.Id == request.StepId);
            if (approvalStep == null)
                throw new ArgumentException("Paso de aprobación no encontrado");


            var isCanUpdateStatus = _projectValidatorCanUpdateStatus.CanUpdateStatus(projectProposal, approvalStep, request);
            if (isCanUpdateStatus.IsFailed || !isCanUpdateStatus.Value)
                return new ResponseCodeAndObject<ProjectProposalResponse>
                {
                    Message = isCanUpdateStatus.Info,
                    httpStatusCode = HttpStatusCode.Conflict
                };

            approvalStep.Status = request.Status;
            approvalStep.ApproverUserId = request.UserId;
            approvalStep.Observations = request.Observation;
            approvalStep.DecisionDate = DateTime.UtcNow;


            if (request.Status == (int)StatusEnum.Rejected)
            {
                projectProposal.Status = (int)StatusEnum.Rejected;
                _repositoryCommand.Update(projectProposal);
            }


            var result = await _repositoryCommand.SaveAsync();

            var mapper = MapperProposal.MapToProposalResponse(projectProposal);
            return new ResponseCodeAndObject<ProjectProposalResponse>
            {
                Response = mapper,
                httpStatusCode = HttpStatusCode.OK,
            };
        }


    }
}