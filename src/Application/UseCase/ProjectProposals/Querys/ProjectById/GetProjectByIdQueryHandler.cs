using Application.Common.Interface.Infrastructure;
using Application.Mapper;
using Domain.Dto;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCase.ProjectProposals.Querys.ProjectById
{
    public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectProposalResponse>
    {
        private readonly IRepositoryQuery _repositoryQuery;

        public GetProjectByIdQueryHandler(IRepositoryQuery repositoryQuery)
        {
            _repositoryQuery = repositoryQuery;
        }

        public async Task<ProjectProposalResponse> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var proposal = await _repositoryQuery.Query<ProjectProposal>()
                .Include(x => x.AreaEntity)
                .Include(x => x.TypeEntity)
                .Include(x => x.ApprovalStatus)
                .Include(x => x.ApprovalSteps)
                    .ThenInclude(x => x.ApproverUser)
                .Include(x => x.ApprovalSteps)
                    .ThenInclude(x => x.ApproverRole)
                .Include(x => x.ApprovalSteps)
                    .ThenInclude(x => x.ApprovalStatus)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (proposal == null)
                throw new ArgumentException($"No se encontró el proyecto con ID: {request.Id}");

            return MapperProposal.MapToProposalResponse(proposal);
        }
    }
}