using Domain.Dto;
using MediatR;
namespace Application.UseCase.ProjectProposals.Commands.Create;
public class CreateProjectProposalCommand : IRequest<ProjectProposalResponse>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public int Duration { get; set; }
    public int Area { get; set; }
    public int User { get; set; }
    public int Type { get; set; }
}