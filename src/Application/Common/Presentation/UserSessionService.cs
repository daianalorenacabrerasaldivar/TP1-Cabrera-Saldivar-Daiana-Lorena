using Application.Common.Interface;
using Application.Common.Interface.Infrastructure;
using Domain.Common.ResultPattern;
using Domain.Entity;

namespace Application.Common.Presentation
{
    public class UserSessionService : IUserSessionService
    {
        private readonly IUserQuery _userQuery;
        private User _activeUser;

        public UserSessionService(IUserQuery userQuery)
        {
            _userQuery = userQuery;
        }

        public User GetActiveUser()
        {
            return _activeUser;
        }

        public async Task<Result<User>> LoginAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                return new Failed<User>("El correo no puede estar vacío");

            var result = await _userQuery.GetUserByEmailAsync(email);

            if (!result.IsFailed)
            {
                _activeUser = result.Value;
            }

            return result;
        }
    }
}