using Domain.Common.ResultPattern;

namespace Application.Common.Interface.Infrastructure
{
    public interface IRepositoryCommand
    {
        void Add<TEntity>(TEntity entity) where TEntity : class;
        Task<Result<string>> SaveAsync();
        void Update<T>(T entity) where T : class;
    }
}