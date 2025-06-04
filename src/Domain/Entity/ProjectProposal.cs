using Domain.Enum;

namespace Domain.Entity
{
    public class ProjectProposal
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }

        public int Area { get; set; }
        public virtual Area AreaEntity { get; set; }

        public int Type { get; set; }
        public virtual ProjectType TypeEntity { get; set; }

        public decimal EstimatedAmount { get; set; }
        public int EstimatedDuration { get; set; }


        public int Status { get; set; } = (int)StatusEnum.Pending;
        public virtual ApprovalStatus ApprovalStatus { get; set; }

        public DateTime CreateAt { get; set; }

        public int CreateBy { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<ProjectApprovalStep> ApprovalSteps { get; set; }
    }
}
