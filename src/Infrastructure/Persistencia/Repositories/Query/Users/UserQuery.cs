using Application.Common.Interface.Infrastructure;
using Domain.Common.ResultPattern;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistencia.Repositories.Query.Users
{
    public class UserQuery : IUserQuery
    {
        private readonly IRepositoryQuery _repositoryQuery;

        public UserQuery(IRepositoryQuery dataBaseService)
        {
            _repositoryQuery = dataBaseService;
        }
        public async Task<Result<List<User>>> GetAllUsers()
        {
            try
            {
                var users = await _repositoryQuery.Query<User>()
                    .Include(u => u.ApproverRole)
                    .ToListAsync();
                if (users == null || !users.Any())
                    return new Failed<List<User>>("No se encontraron usuarios");
                return new Success<List<User>>(users);
            }
            catch (Exception ex)
            {
                return new Failed<List<User>>("Ocurrio un error en el sistema, por favor intente m�s tarde");
            }
        }
        public async Task<Result<User>> GetUserByEmailAsync(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    return new Failed<User>("El correo no puede estar vacio");

                var user = await _repositoryQuery.Query<User>()

                    .FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                    return new Failed<User>($"No se encontro un usuario con el email {email}");

                return new Success<User>(user);
            }
            catch (Exception ex)
            {
                return new Failed<User>("Ocurrio un error en el sistema, por favor intente m�s tarde");
            }
        }

        public async Task<Result<User>> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _repositoryQuery.Query<User>()
                    .FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                    return new Failed<User>($"No se encontro un usuario con el id {id}");
                return new Success<User>(user);
            }
            catch (Exception ex)
            {
                return new Failed<User>("Ocurrio un error en el sistema, por favor intente m�s tarde");
            }
        }

    }
}
