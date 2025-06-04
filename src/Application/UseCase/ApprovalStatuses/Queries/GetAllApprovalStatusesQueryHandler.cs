using Application.Common.Interface.Infrastructure;
using Domain.Dto;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCase.ApprovalStatuses.Queries
{
    public class GetAllApprovalStatusesQueryHandler : IRequestHandler<GetAllApprovalStatusesQuery, List<StatusDto>>
    {
        private readonly IRepositoryQuery _repositoryQuery;

        public GetAllApprovalStatusesQueryHandler(IRepositoryQuery repositoryQuery)
        {
            _repositoryQuery = repositoryQuery;
        }

        public async Task<List<StatusDto>> Handle(GetAllApprovalStatusesQuery request, CancellationToken cancellationToken)
        {
            var statuses = await _repositoryQuery.Query<ApprovalStatus>()
                .Select(status => new StatusDto
                {
                    Id = status.Id,
                    Name = status.Name
                })
                .ToListAsync(cancellationToken);

            if (!statuses.Any())
                statuses = new List<StatusDto>();

            return statuses;
        }
    }
}