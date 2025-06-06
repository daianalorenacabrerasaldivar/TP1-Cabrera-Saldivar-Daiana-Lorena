using Application.Common.Interface;
using Application.Common.Interface.Infrastructure;
using Application.UseCase.AprovalStep.Update;
using Domain.Entity;
using Domain.Enum;

namespace Application.Service.StatusProjectAndStep
{
    public class ApprovalStepStatusUpdater : IApprovalStepStatusUpdater
    {
        private readonly IRepositoryCommand _repositoryCommand;
        public ApprovalStepStatusUpdater(IRepositoryCommand repositoryCommand)
        {
            _repositoryCommand = repositoryCommand;
        }
        public async Task<ProjectProposal> UpdateProposalAndStepAsync(ProjectProposal projectProposal, ProjectApprovalStep approvalStep, UpdateApprovalStepCommand request)
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
                _repositoryCommand.Update<ProjectProposal>(projectProposal);
            }
            _repositoryCommand.Update<ProjectApprovalStep>(approvalStep);
            var result = await _repositoryCommand.SaveAsync();
            return projectProposal;
        }

        private bool AreAllStepsApproved(ProjectProposal project)
        {
            return project.ApprovalSteps.All(step => step.Status == (int)StatusEnum.Approved);
        }
    }
}
