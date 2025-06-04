namespace Domain.Dto
{
    public class ProjectProposalResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public int Duration { get; set; }
        public AreaDto Area { get; set; }
        public StatusDto Status { get; set; }
        public ProjectTypeDto Type { get; set; }
        public UserDto User { get; set; }
        public List<AprovalStepDto> Steps { get; set; }
    }
}