using Application.Common.Interface;
using Domain.Common.ResultPattern;
using Domain.Entity;
using Domain.Enum;
namespace Application.Service.ProjectProposals;
public class ProjectApprovalAssignmentRuleService : IApprovalAssignmentService
{
    private readonly IApprovalRuleForProjectQuery _approvalRuleSelectorQuery;

    public ProjectApprovalAssignmentRuleService(IApprovalRuleForProjectQuery approvalRuleSelectorQuery)
    {
        _approvalRuleSelectorQuery = approvalRuleSelectorQuery;
    }

    public async Task<Result<List<ProjectApprovalStep>>> GetApprovalStepsForProposalAsync(ProjectProposal proposal)
    {

        var ruleAprovalResult = await _approvalRuleSelectorQuery.SelectBestApprovalRuleAsync(proposal);
        if (ruleAprovalResult.IsFailed)
            return new Failed<List<ProjectApprovalStep>>(ruleAprovalResult.Info);

        var ruleAproval = ruleAprovalResult.Value;
        List<ProjectApprovalStep> steps = new List<ProjectApprovalStep>();
        for (int i = 0; i < ruleAproval.StepOrder; i++)
        {
            var step = new ProjectApprovalStep
            {
                ProjectProposalId = proposal.Id,
                ApproverRoleId = ruleAproval.ApproverRoleId,
                StepOrder = ruleAproval.StepOrder,
                Status = (int)StatusEnum.Pending
            };
            steps.Add(step);

        }
        return new Success<List<ProjectApprovalStep>>(steps);
    }
}