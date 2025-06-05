using Application.UseCase.ProjectProposals.Querys.FilterParameter;
using Domain.Entity;

namespace Application.Mapper
{
    public class GetProjectResponseMapper
    {
        public static List<GetProjectResponse> MapToResponse(IEnumerable<ProjectProposal> projects)
        {
            return projects.Select(p => new GetProjectResponse
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Amount = p.EstimatedAmount,
                Duration = p.EstimatedDuration,
                Area = p.AreaEntity?.Name,
                Status = p.ApprovalStatus?.Name,
                Type = p.TypeEntity?.Name
            }).ToList();
        }
    }
}
