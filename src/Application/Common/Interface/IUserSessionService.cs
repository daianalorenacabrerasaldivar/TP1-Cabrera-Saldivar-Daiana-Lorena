using Domain.Common.ResultPattern;
using Domain.Entity;
namespace Application.Common.Interface
{
    public interface IUserSessionService
    {
        Task<Result<User>> LoginAsync(string email);
        User GetActiveUser();
    }
}