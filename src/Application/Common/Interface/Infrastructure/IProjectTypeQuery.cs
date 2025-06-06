using Domain.Entity;

namespace Application.Common.Interface.Infrastructure
{
    public interface IProjectTypeQuery
    {
        Task<List<ProjectType>> GetAllProjectTypesAsync();
        Task<ProjectType> GetProjectTypeByNameAsync(string name);
    }
}