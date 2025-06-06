using Application.Common.Interface;
using Application.Common.Interface.Infrastructure;
using Application.UseCase.AprovalStep.Update;
using Domain.Entity;
using Domain.Enum;

namespace Infrastructure.Persistencia.Repositories.Query.AprovalRules
{
    public class ProjectApprovalStatusUpdater : IApprovalStepStatusUpdater
    {
        private readonly IRepositoryCommand _repositoryCommand;
        public ProjectApprovalStatusUpdater(IRepositoryCommand repositoryCommand)
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
