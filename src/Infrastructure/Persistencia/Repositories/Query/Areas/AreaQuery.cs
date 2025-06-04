using Application.Common.Interface.Infrastructure;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistencia.Repositories.Query.Areas
{
    public class AreaQuery : IAreaQuery
    {
        private readonly IRepositoryQuery _repositoryBase;
        public AreaQuery(IRepositoryQuery repositoryBase)
        {
            _repositoryBase = repositoryBase;
        }
        public async Task<List<Area>> GetAllAreasAsync()
        {
            return await _repositoryBase.Query<Area>().ToListAsync();
        }

        public async Task<Area> GetAreaByIdAsync(int id)
        {
            var area = await _repositoryBase.Query<Area>().FirstOrDefaultAsync(a => a.Id == id);
            if (area == null)
            {
                throw new Exception($"Area with ID {id} not found.");
            }
            return area;
        }
    }
}
