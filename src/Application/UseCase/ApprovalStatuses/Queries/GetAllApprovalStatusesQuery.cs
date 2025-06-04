using Domain.Dto;
using MediatR;

namespace Application.UseCase.ApprovalStatuses.Queries
{
    public class GetAllApprovalStatusesQuery : IRequest<List<StatusDto>>
    {
    }
}