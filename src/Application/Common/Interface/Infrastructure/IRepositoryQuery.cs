namespace Application.Common.Interface.Infrastructure;
public interface IRepositoryQuery
{
    IQueryable<T> Query<T>() where T : class;
}