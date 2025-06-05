using Application.Common.Interface;
using Application.Common.Interface.Infrastructure;
using Application.Mapper;
using Domain.Common;
using Domain.Dto;
using Domain.Entity;
using Domain.Enum;
using MediatR;
using System.Net;

namespace Application.UseCase.AprovalStep.Update
{
    public class UpdateApprovalStepCommandHandler : IRequestHandler<UpdateApprovalStepCommand, ResponseCodeAndObject<ProjectProposalResponse>>
    {
        private readonly IRepositoryCommand _repositoryCommand;
        private readonly IProjectApprovalStepUpdateValidator _projectValidatorCanUpdateStatus;
        private readonly IProjectProposalQuery _projectProposalQuery;
        public UpdateApprovalStepCommandHandler(IRepositoryCommand repositoryCommand, IProjectProposalQuery projectTypeQuery, IProjectApprovalStepUpdateValidator projectValidatorCanUpdateStatus)
        {
            _repositoryCommand = repositoryCommand;
            _projectProposalQuery = projectTypeQuery;
            _projectValidatorCanUpdateStatus = projectValidatorCanUpdateStatus;
        }

        public async Task<ResponseCodeAndObject<ProjectProposalResponse>> Handle(UpdateApprovalStepCommand request, CancellationToken cancellationToken)
        {
            var validatio = UpdateApprovalStepValidator.Validate(request);
            if (validatio.IsFailed)
                throw new ArgumentException(validatio.Info);

            var projectProposalResult = await _projectProposalQuery.GetProjectProposalByIdAsync(request.ProjectId);

            if (projectProposalResult.IsFailed)
            {
                return new ResponseCodeAndObject<ProjectProposalResponse>
                {
                    Message = projectProposalResult.Info,
                    httpStatusCode = HttpStatusCode.InternalServerError
                };
            }
            else if (projectProposalResult.Value == null)
            {
                return new ResponseCodeAndObject<ProjectProposalResponse>
                {

                    Message = "Proyecto no encontrado",
                    httpStatusCode = HttpStatusCode.NotFound
                };
            }

            var ApprovalStepResul = await _projectValidatorCanUpdateStatus.TryGetUpdatableApprovalStepAsync(projectProposalResult, request);
            if (ApprovalStepResul.IsFailed)
                return new ResponseCodeAndObject<ProjectProposalResponse>
                {
                    Message = ApprovalStepResul.Info,
                    httpStatusCode = HttpStatusCode.Conflict
                };
   
            var resultProject = await UpdateProposalAndStepAsync(projectProposalResult.Value, ApprovalStepResul.Value, request);

            var mapper = MapperProposal.MapToProposalResponse(resultProject);
            return new ResponseCodeAndObject<ProjectProposalResponse>
            {
                Response = mapper,
                httpStatusCode = HttpStatusCode.OK,
            };
        }
        private async Task<ProjectProposal> UpdateProposalAndStepAsync(ProjectProposal projectProposal,ProjectApprovalStep approvalStep, UpdateApprovalStepCommand request)
        {
           
            approvalStep.Status = request.Status;
            approvalStep.ApproverUserId = request.UserId;
            approvalStep.Observations = request.Observation;
            approvalStep.DecisionDate = DateTime.UtcNow;


            if (request.Status == (int)StatusEnum.Rejected)
            {
                projectProposal.Status = (int)StatusEnum.Rejected;
                _repositoryCommand.Update(projectProposal);
            }
            else if (AreAllStepsApproved(projectProposal))
            {
                projectProposal.Status = (int)StatusEnum.Approved;
            }

            var result = await _repositoryCommand.SaveAsync();
            return projectProposal;
        }

        private bool AreAllStepsApproved(ProjectProposal project)
        {
            return project.ApprovalSteps.All(step => step.Status == (int)StatusEnum.Approved);
        }

    }
}