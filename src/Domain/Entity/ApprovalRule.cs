namespace Domain.Entity
{
    public class ApprovalRule
    {
        public long Id { get; set; }
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }
        public int StepOrder { get; set; }

        public int ApproverRoleId { get; set; }
        public virtual ApproverRole ApproverRole { get; set; }

        public int? Area { get; set; }
        public virtual Area? AreaEntity { get; set; } = null;

        public int? Type { get; set; }
        public virtual ProjectType? TypeEntity { get; set; } = null;
    }
}
