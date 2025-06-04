using Domain.Dto;
using MediatR;

namespace Application.UseCase.Users.Queries
{
    public class GetAllUsersQuery : IRequest<List<UserDto>>
    {
    }
}