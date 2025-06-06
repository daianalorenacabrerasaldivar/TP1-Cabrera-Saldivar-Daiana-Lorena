using Application.Common.Interface.Infrastructure;
using Application.Mapper;
using Domain.Dto;
using MediatR;

namespace Application.UseCase.ProjectProposals.Querys.ProjectById
{
    public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectProposalResponse>
    {
        private readonly IProjectProposalQuery _projectProposalQuery;

        public GetProjectByIdQueryHandler(IProjectProposalQuery projectProposalQuery)
        {
            _projectProposalQuery = projectProposalQuery;
        }

        public async Task<ProjectProposalResponse> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var proposal = await _projectProposalQuery.GetProjectProposalByIdAsync(request.Id);

            if (proposal == null)
                throw new ArgumentException($"No se encontró el proyecto con ID: {request.Id}");

            return MapperProposal.MapToProposalResponse(proposal);
        }
    }
}