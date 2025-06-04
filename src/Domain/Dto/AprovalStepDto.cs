namespace Domain.Dto
{
    public class AprovalStepDto
    {
        public long Id { get; set; }
        public int StepOrder { get; set; }
        public DateTime? DecisionDate { get; set; }
        public string? Observations { get; set; }
        public UserDto? ApproverUser { get; set; }
        public RoleDto ApproverRole { get; set; }
        public StatusDto Status { get; set; }
    }

    public class AprovalStepDtoRequest
    {
        public long Id { get; set; }
        public int User { get; set; }
        public int Status { get; set; }
        public string? Observation { get; set; }

    }
}