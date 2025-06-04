using Application.Common.Interface;
using Application.Common.Interface.Infrastructure;
using Application.UseCase.AprovalStep.Update;
using Domain.Common.ResultPattern;
using Domain.Entity;
using Domain.Enum;

namespace Application.Service.ValidatorsBusiness
{
    public class ProjectValidatorCanUpdateStatus : IProjectValidatorCanUpdateStatus
    {
        private readonly IRepositoryQuery _repositoryQuery;
        public ProjectValidatorCanUpdateStatus(IRepositoryQuery repositoryQuery)
        {
            _repositoryQuery = repositoryQuery;
        }
        public Result<bool> CanUpdateStatus(ProjectProposal project, ProjectApprovalStep approvalStep, UpdateApprovalStepCommand updateApprovalStep)
        {
            bool projectIsModificable = project.Status == (int)StatusEnum.Observed || project.Status == (int)StatusEnum.Observed; ;
            bool isStepModificable = approvalStep.Status == (int)StatusEnum.Pending || approvalStep.Status == (int)StatusEnum.Observed;

            return new Success<bool>(projectIsModificable);

        }

    }
}
