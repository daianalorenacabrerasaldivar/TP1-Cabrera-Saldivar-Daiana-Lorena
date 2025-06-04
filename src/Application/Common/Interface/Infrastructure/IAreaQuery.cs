using Domain.Dto;
using Domain.Entity;

namespace Application.Common.Interface.Infrastructure
{
    public interface IAreaQuery
    { 
        Task<Area> GetAreaByIdAsync(int id);
        Task<List<Area>> GetAllAreasAsync();
    }
}