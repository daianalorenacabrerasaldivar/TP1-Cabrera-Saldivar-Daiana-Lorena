using Domain.Entity;
using Domain.Enum;
namespace Infrastructure.Persistencia.Repositories.Query.Project
{
    public class ProjectValidator : IProjectValidator
    {
        public bool CanUpdateStatus(ProjectProposal project)
        {
            return project.Status != (int)StatusEnum.Rejected && project.Status != (int)StatusEnum.Approved;
        }
    }
}

