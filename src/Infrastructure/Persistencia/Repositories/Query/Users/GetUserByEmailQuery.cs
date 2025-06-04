using Application.Common.Interface.Infrastructure;
using Domain.Common.ResultPattern;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistencia.Repositories.Query.Users
{
    public class GetUserByEmailQuery
    {
        private readonly IRepositoryQuery _dataBaseService;

        public GetUserByEmailQuery(IRepositoryQuery dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        public async Task<Result<User>> GetUserByEmail(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    return new Failed<User>("El correo no puede estar vacio");

                var user = await _dataBaseService.Query<User>()

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


    }
}
