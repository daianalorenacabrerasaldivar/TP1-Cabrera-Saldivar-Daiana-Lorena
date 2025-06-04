namespace Domain.Entity
{
    public class ApprovalStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ProjectProposal> ProjectProposals { get; set; }
        public virtual ICollection<ProjectApprovalStep> ApprovalSteps { get; set; }

    }
}
