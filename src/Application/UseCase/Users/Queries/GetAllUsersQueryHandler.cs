using Application.Common.Interface.Infrastructure;
using Domain.Dto;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCase.Users.Queries
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDto>>
    {
        private readonly IUserQuery _repositoryQuery;

        public GetAllUsersQueryHandler(IUserQuery repositoryQuery)
        {
            _repositoryQuery = repositoryQuery;
        }

        public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var usersResult = await _repositoryQuery.GetAllUsers();

                if (usersResult == null || usersResult.IsFailed || !usersResult.Value.Any())
                {
                    return new List<UserDto>();
                }
                var users = usersResult.Value;
                var userDtos = users.Select(user => new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = new RoleDto
                    {
                        Id = user.Role,
                        Name = user.ApproverRole.Name
                    }
                }).ToList();

                return userDtos;
            }
            catch (Exception ex)
            {
                return new List<UserDto>();
            }
        }
    }
}