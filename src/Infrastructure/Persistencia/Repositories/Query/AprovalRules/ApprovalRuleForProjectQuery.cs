using Application.Common.Interface.Infrastructure;
using Domain.Common.ResultPattern;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
public class ApprovalRuleForProjectQuery : IApprovalRuleForProjectQuery
{
    private readonly IRepositoryQuery _repository;

    public ApprovalRuleForProjectQuery(IRepositoryQuery repository)
    {
        _repository = repository;
    }

    public async Task<Result<ApprovalRule>> SelectBestApprovalRuleAsync(ProjectProposal projectProposal)
    {
        var candidateForRules = await _repository.Query<ApprovalRule>()
            .Where(r =>
                (r.MinAmount == 0 || projectProposal.EstimatedAmount >= r.MinAmount) &&
                (r.MaxAmount == 0 || projectProposal.EstimatedAmount <= r.MaxAmount) &&
                (r.AreaEntity == null || r.Area == projectProposal.Area) &&
                (r.Type == null || r.Type == projectProposal.Type))
            .OrderByDescending(rule => rule.Area != null && rule.Type != null)
            .ToListAsync();

        if (candidateForRules == null || !candidateForRules.Any())
        {
            return new Failed<ApprovalRule>("No se encontraron reglas de aprobación aplicables para el proyecto.");
        }

        var mostSpecificRule = candidateForRules
         .OrderByDescending(r =>
              (r.Area.HasValue ? 1 : 0) +
              (r.Type.HasValue ? 1 : 0) +
              (r.MinAmount > 0 ? 1 : 0) +
              (r.MaxAmount > 0 ? 1 : 0))
          .FirstOrDefault();

        if (mostSpecificRule == null)
        {
            return new Failed<ApprovalRule>("No se encontraron reglas de aprobación aplicables para el proyecto.");
        }

        return new Success<ApprovalRule>(mostSpecificRule);

    }
}
