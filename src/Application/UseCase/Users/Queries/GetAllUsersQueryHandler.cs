using Application.Common.Interface.Infrastructure;
using Domain.Dto;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCase.Users.Queries
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDto>>
    {
        private readonly IRepositoryQuery _repositoryQuery;

        public GetAllUsersQueryHandler(IRepositoryQuery repositoryQuery)
        {
            _repositoryQuery = repositoryQuery;
        }

        public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _repositoryQuery.Query<User>()
                .Include(u => u.ApproverRole)
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = new RoleDto
                    {
                        Id = user.Role,
                        Name = user.ApproverRole.Name
                    }
                })
                .ToListAsync(cancellationToken);

            if (!users.Any())
                users = new List<UserDto>();

            return users;
        }
    }
}