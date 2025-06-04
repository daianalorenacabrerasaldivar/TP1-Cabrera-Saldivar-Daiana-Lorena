using Application.Common.Interface.Infrastructure;
using Domain.Dto;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCase.Roles.Queries
{
    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, List<RoleDto>>
    {
        private readonly IRepositoryQuery _repositoryQuery;

        public GetAllRolesQueryHandler(IRepositoryQuery repositoryQuery)
        {
            _repositoryQuery = repositoryQuery;
        }

        public async Task<List<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _repositoryQuery.Query<ApproverRole>()
                .Select(role => new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name
                })
                .ToListAsync(cancellationToken);

            if (!roles.Any())
                roles = new List<RoleDto>();

            return roles;
        }
    }
}