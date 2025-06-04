namespace Domain.Entity
{
    public class ApproverRole
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ApprovalRule> ApprovalRules { get; set; }
        public virtual ICollection<ProjectApprovalStep> ProjectApprovalSteps { get; set; }

    }

}
