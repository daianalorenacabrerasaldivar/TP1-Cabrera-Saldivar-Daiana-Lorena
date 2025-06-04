namespace Domain.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public int Role { get; set; }
        public virtual ApproverRole ApproverRole { get; set; }


        public virtual ICollection<ProjectProposal> ProjectProposals { get; set; }
        public virtual ICollection<ProjectApprovalStep> ProjectApprovalSteps { get; set; }
    }
}
