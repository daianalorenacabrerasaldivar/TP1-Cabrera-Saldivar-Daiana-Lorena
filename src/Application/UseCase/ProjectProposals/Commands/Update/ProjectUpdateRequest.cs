namespace Application.UseCase.ProjectProposals.Commands.Update
{
    public class ProjectUpdateRequest
    {
        public string title { get; set; }
        public string description { get; set; }
        public int duration { get; set; }

    }
}
