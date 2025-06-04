using Domain.Dto;
using Domain.Entity;

namespace Application.Common.Interface.Presentation
{
    public interface IProjectConsolePresenter
    {
        void ShowMenu();
        void ShowNewlyCreatedProjectDto(ProjectProposalResponse project);
        void ShowProjectProposalDetails(ProjectProposal project);
        void ShowProjectsSummary(List<ProjectProposal> projects);
    }
}