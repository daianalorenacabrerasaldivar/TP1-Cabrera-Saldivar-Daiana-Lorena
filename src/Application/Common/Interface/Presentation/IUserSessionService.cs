using Domain.Entity;


public interface IUserSessionService
{
    Task LoginAsync();
    User GetActiveUser();
}
