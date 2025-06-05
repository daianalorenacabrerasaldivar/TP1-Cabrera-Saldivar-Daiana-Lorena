using Application.UseCase.ProjectProposals.Querys.FilterParameter;
using Domain.Dto;
using Domain.Entity;

namespace Application.Common.Interface.Presentation
{
    public interface IProjectConsolePresenter
    {
        void ShowNewlyCreatedProjectDto(ProjectProposalResponse project);
        void ShowProjectProposalDetails(ProjectProposalResponse project);
        void ShowProjectsSummary(List<GetProjectResponse> projects);
    }
}