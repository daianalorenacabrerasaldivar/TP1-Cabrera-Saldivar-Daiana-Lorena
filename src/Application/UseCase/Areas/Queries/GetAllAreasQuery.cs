using Domain.Dto;
using MediatR;

namespace Application.UseCase.Areas.Queries
{
    public class GetAllAreasQuery : IRequest<List<AreaDto>>
    {
    }
}