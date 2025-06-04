using Application.Common.Interface;
using Application.Common.Interface.Infrastructure;
using Application.UsesCases.Query.ProjectTypes;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistencia.Repositories.Query.ProjectTypes
{
    public class ProjectTypeQuery : IProjectTypeQuery
    {

        private readonly IRepositoryQuery _context;
        public ProjectTypeQuery(IRepositoryQuery context)
        {
            _context = context;
        }
        public async Task<List<ProjectType>> GetAllProjectTypesAsync()
        {
            var projectTypes = await _context.Query<ProjectType>().ToListAsync();
            return projectTypes;
        }
        public async Task<ProjectType> GetProjectTypeByIdAsync(int id)
        {
            var projectType = await _context.Query<ProjectType>()
                .FirstOrDefaultAsync(p => p.Id == id);
            return projectType;
        }

        public async Task<ProjectType> GetProjectTypeByNameAsync(string name)
        {
            var projectType = await _context.Query<ProjectType>()
                .FirstOrDefaultAsync(p => p.Name == name);
            return projectType;
        }
    }
}
