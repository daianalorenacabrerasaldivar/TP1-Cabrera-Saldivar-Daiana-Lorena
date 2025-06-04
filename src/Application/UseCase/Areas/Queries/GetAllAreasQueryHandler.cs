using Application.Common.Interface.Infrastructure;
using Domain.Dto;
using MediatR;

namespace Application.UseCase.Areas.Queries
{
    public class GetAllAreasQueryHandler : IRequestHandler<GetAllAreasQuery, List<AreaDto>>
    {
        private readonly IAreaQuery _repositoryQuery;

        public GetAllAreasQueryHandler(IAreaQuery repositoryQuery)
        {
            _repositoryQuery = repositoryQuery;
        }

        public async Task<List<AreaDto>> Handle(GetAllAreasQuery request, CancellationToken cancellationToken)
        {
            var areasEntity = await _repositoryQuery.GetAllAreasAsync();

            var areas = areasEntity.Select(area => new AreaDto
            {
                Id = area.Id,
                Name = area.Name
            });

            return areasEntity?.Select(area => new AreaDto
            {
                Id = area.Id,
                Name = area.Name
            }).ToList() ?? new List<AreaDto>();
        }
    }
}