using Application.Common.Interface;
using Application.Common.Interface.Infrastructure;
using Application.Mapper;
using Domain.Common;
using Domain.Dto;
using MediatR;
using System.Net;

namespace Application.UseCase.AprovalStep.Update
{
    public class UpdateApprovalStepCommandHandler : IRequestHandler<UpdateApprovalStepCommand, ResponseCodeAndObject<ProjectProposalResponse>>
    {
        private readonly IApprovalStepStatusUpdater _approvalStepStatusUpdater;
        private readonly IProjectApprovalStepUpdateValidator _projectValidatorCanUpdateStatus;
        private readonly IProjectProposalQuery _projectProposalQuery;
        public UpdateApprovalStepCommandHandler(IApprovalStepStatusUpdater approvalStepStatusUpdater, IProjectProposalQuery projectTypeQuery, IProjectApprovalStepUpdateValidator projectValidatorCanUpdateStatus)
        {
            _approvalStepStatusUpdater = approvalStepStatusUpdater;
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

            var resultProject = await _approvalStepStatusUpdater.UpdateProposalAndStepAsync(projectProposalResult.Value, ApprovalStepResul.Value, request);

            var mapper = MapperProposal.MapToProposalResponse(resultProject);
            return new ResponseCodeAndObject<ProjectProposalResponse>
            {
                Response = mapper,
                httpStatusCode = HttpStatusCode.OK,
            };
        }


    }
}