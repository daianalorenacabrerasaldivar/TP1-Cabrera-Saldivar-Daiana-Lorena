namespace Domain.Entity
{
    public class Area
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ApprovalRule> ApprovalRules { get; set; }
        public virtual ICollection<ProjectProposal> ProjectProposals { get; set; }
    }
}