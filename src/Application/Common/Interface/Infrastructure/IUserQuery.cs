﻿using Domain.Common.ResultPattern;
using Domain.Entity;

namespace Application.Common.Interface.Infrastructure
{
    public interface IUserQuery
    {
        Task<Result<List<User>>> GetAllUsers();
        Task<Result<User>> GetUserByEmailAsync(string email);
        Task<Result<User>> GetUserByIdAsync(int id);
    }
}