using Application.Common.Interface;
using Application.Common.Interface.Infrastructure;
using Domain.Entity;
using Domain.Enum;

namespace Infrastructure.Persistencia.Repositories.Query.Role
{
    public class ProposalsByUserRole
    {
        private readonly IRepositoryQuery _context;
        public ProposalsByUserRole(IRepositoryQuery context)
        {
            _context = context;
        }

        public List<ProjectProposal> GetPendingProposalsByUserRole(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var approvalRules = _context.Query<ApprovalRule>()
                .Where(rule => rule.ApproverRoleId == user.Role)
                .ToList();

            var pendingProposals = _context.Query<ProjectProposal>()
                .Where(proposal => proposal.Status == (int)StatusEnum.Pending
                    && approvalRules.Any(rule =>
                        (rule.AreaEntity == null || rule.AreaEntity.Id == proposal.Area) &&
                        (rule.Type == null || rule.Type == proposal.Type) &&
                        rule.MinAmount <= proposal.EstimatedAmount &&
                        (rule.MaxAmount == 0 || proposal.EstimatedAmount <= rule.MaxAmount)))
                .ToList();

            return pendingProposals;
        }
    }
}
