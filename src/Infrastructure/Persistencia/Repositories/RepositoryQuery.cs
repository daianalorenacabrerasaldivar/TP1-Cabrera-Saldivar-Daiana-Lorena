﻿using Application.Common.Interface.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistencia.Repositories
{
    public class RepositoryQuery<TContext> : IRepositoryQuery where TContext : DbContext
    {
        private readonly TContext _context;
        public RepositoryQuery(TContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public IQueryable<T> Query<T>() where T : class
        {
            return _context.Set<T>();
        }
    }
}
