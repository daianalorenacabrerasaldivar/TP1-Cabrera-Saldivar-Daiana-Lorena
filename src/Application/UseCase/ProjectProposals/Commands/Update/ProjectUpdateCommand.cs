using Domain.Common;
using Domain.Dto;
using MediatR;

namespace Application.UseCase.ProjectProposals.Commands.Update
{
    public class ProjectUpdateCommand : IRequest<ResponseCodeAndObject<ProjectProposalResponse>>
    {
        public Guid ProjectId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int duration { get; set; }
    }
}
