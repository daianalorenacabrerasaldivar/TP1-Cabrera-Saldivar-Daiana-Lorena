using Application.Common.Interface;
using Application.Common.Interface.Infrastructure;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
public class ApprovalRuleValidator : IApprovalRuleValidator
{
    private readonly IRepositoryQuery _repository;

    public ApprovalRuleValidator(IRepositoryQuery repository)
    {
        _repository = repository;
    }

    public async Task<List<ApprovalRule>> GetApplicableRulesAsync(ProjectProposal projectProposal)
    {
        return await _repository.Query<ApprovalRule>()
            .Where(r =>
                (r.MinAmount == 0 || projectProposal.EstimatedAmount >= r.MinAmount) &&
                (r.MaxAmount == 0 || projectProposal.EstimatedAmount <= r.MaxAmount) &&
                (r.AreaEntity == null || r.Area == projectProposal.Area) &&
                (r.Type == null || r.Type == projectProposal.Type))
            .OrderByDescending(rule => rule.Area != null && rule.Type != null)
            .ThenBy(rule => rule.StepOrder)
            .ToListAsync();
    }
}
