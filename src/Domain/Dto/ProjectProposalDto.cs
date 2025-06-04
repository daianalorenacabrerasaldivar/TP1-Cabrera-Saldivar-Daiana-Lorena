using Domain.Entity;

namespace Domain.Dto
{
    public class ProjectProposalDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Area Area { get; set; }
        public ProjectType Type { get; set; }
        public decimal EstimatedAmount { get; set; }
        public int EstimatedDuration { get; set; }
        public User CreateBy { get; set; }
    }
}
