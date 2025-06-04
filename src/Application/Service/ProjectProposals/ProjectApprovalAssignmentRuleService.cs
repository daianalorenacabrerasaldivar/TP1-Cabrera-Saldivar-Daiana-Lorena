using Application.Common.Interface;
using Application.Common.Interface.Infrastructure;
using Domain.Common.ResultPattern;
using Domain.Entity;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;
namespace Application.Service.ProjectProposals;
public class ProjectApprovalAssignmentRuleService : IApprovalAssignmentService
{
    private readonly IRepositoryQuery _queryRepository;

    public ProjectApprovalAssignmentRuleService(IRepositoryQuery queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public async Task<Result<List<ProjectApprovalStep>>> GetApprovalStepsForProposalAsync(ProjectProposal proposal)
    {
        var rules = await _queryRepository.Query<ApprovalRule>()
             .Where(r =>
            (r.MinAmount == 0 || proposal.EstimatedAmount >= r.MinAmount) &&
            (r.MaxAmount == 0 || proposal.EstimatedAmount <= r.MaxAmount) &&
            (r.AreaEntity == null || r.Area == proposal.Area) &&
            (r.Type == null || r.Type == proposal.Type))
            .OrderByDescending(rule => rule.Area != null && rule.Type != null)
            // Dentro de las reglas igual de específicas, ordena por el orden del paso.
            .ThenBy(rule => rule.StepOrder)
            .ToListAsync();

        List<ProjectApprovalStep> steps = new List<ProjectApprovalStep>();
        foreach (var rule in rules)
        {
            var step = new ProjectApprovalStep
            {
                ProjectProposalId = proposal.Id,
                ApproverRoleId = rule.ApproverRoleId,
                StepOrder = rule.StepOrder,
                Status = (int)StatusEnum.Pending
            };
            steps.Add(step);

        }
        return new Success<List<ProjectApprovalStep>>(steps);
    }
}