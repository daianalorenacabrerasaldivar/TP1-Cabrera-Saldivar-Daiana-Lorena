using Domain.Dto;
using MediatR;

namespace Application.UseCase.ProjectTypes.Queries
{
    public class GetAllProjectTypesQuery : IRequest<List<ProjectTypeDto>>
    {
    }
}