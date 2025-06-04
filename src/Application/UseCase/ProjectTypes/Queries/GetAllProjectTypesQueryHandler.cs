using Application.Common.Interface.Infrastructure;
using Domain.Dto;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCase.ProjectTypes.Queries
{
    public class GetAllProjectTypesQueryHandler : IRequestHandler<GetAllProjectTypesQuery, List<ProjectTypeDto>>
    {
        private readonly IRepositoryQuery _repositoryQuery;

        public GetAllProjectTypesQueryHandler(IRepositoryQuery repositoryQuery)
        {
            _repositoryQuery = repositoryQuery;
        }

        public async Task<List<ProjectTypeDto>> Handle(GetAllProjectTypesQuery request, CancellationToken cancellationToken)
        {
            var projectTypes = await _repositoryQuery.Query<ProjectType>()
                .Select(type => new ProjectTypeDto
                {
                    Id = type.Id,
                    Name = type.Name
                })
                .ToListAsync(cancellationToken);

            if (!projectTypes.Any())
                projectTypes = new List<ProjectTypeDto>();

            return projectTypes;
        }
    }
}