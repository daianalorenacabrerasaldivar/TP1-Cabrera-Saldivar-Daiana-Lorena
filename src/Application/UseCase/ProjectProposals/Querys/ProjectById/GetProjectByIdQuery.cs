using Domain.Dto;
using MediatR;

namespace Application.UseCase.ProjectProposals.Querys.ProjectById
{
    public class GetProjectByIdQuery : IRequest<ProjectProposalResponse>
    {
        public Guid Id { get; set; }
    }
}