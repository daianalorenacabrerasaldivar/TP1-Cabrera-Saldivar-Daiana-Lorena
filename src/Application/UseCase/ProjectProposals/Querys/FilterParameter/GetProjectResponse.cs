namespace Application.UseCase.ProjectProposals.Querys.FilterParameter
{
    public class GetProjectResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public int Duration { get; set; }
        public string Area { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
    }
}
