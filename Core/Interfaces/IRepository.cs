﻿using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IRepository<T>
    {
        public IQueryable<T> All { get; }

        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

        Task<List<T>> GetAllAsync(
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            IPagingParams? pagingParams = null
        );

        void Add(T entity);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> range);

        void Update(T entity);

        int Count();
    }
}
