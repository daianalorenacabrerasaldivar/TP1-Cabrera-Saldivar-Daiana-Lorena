using Application.UseCase.ProjectProposals.Querys.FilterParameter;

namespace Application.Common.Interface.Presentation
{
    public interface IProjectSelectionService
    {
        Task<Guid> SelectProjectAsync(List<GetProjectResponse> projects);
    }
}