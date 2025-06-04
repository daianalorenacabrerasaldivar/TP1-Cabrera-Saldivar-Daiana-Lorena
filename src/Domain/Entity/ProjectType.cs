namespace Domain.Entity
{
    public class ProjectType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ProjectProposal> ProjectProposals { get; set; }
        public virtual ICollection<ApprovalRule> ApprovalRules { get; set; }
    }
}