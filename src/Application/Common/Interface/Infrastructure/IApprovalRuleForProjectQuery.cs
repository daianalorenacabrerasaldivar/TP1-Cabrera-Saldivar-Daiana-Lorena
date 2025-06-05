using Domain.Common.ResultPattern;
using Domain.Entity;

public interface IApprovalRuleForProjectQuery
{
    Task<Result<ApprovalRule>> SelectBestApprovalRuleAsync(ProjectProposal projectProposal);
}