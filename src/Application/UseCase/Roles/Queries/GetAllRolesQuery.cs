using Domain.Dto;
using MediatR;

namespace Application.UseCase.Roles.Queries
{
    public class GetAllRolesQuery : IRequest<List<RoleDto>>
    {
    }
}