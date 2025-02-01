using Core.Interfaces;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
using Core.Interfaces.Services;
using Infrastructure.Services;
using Core.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repository;
public class BaseRepository<T> : IRepository<T> where T : BaseEntity
{
    internal readonly DbSet<T> _dbSet;
    internal readonly AppDbContext _context;
    internal readonly IUserContextService _userContextService;

    protected BaseRepository(AppDbContext context, IUserContextService userContextService)
    {
        _dbSet = context.Set<T>();
        _context = context;
        _userContextService = userContextService;
    }


    public IQueryable<T> All => _dbSet.AsNoTracking();

    public virtual void Add(T entity)
    {
        var userId = _userContextService.GetUserId();
        if (userId == null) throw new UnauthorizedAccessException("User not authenticated");

        entity.CreatedById = userId;
        entity.CreatedAt = DateTimeOffset.UtcNow;

        _dbSet.Add(entity);
    }

    public Task<List<T>> GetAllAsync(
        List<Expression<Func<T, bool>>>? filters = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        IPagingParams? pagingParams = null
    )
    {
        IQueryable<T> query = _dbSet;

        if (filters is not null)
        {
            filters.ForEach(filter => { query = query.Where(filter); });
        }

        if (include is not null)
        {
            query = include(query);
        }

        if (orderBy is not null)
        {
            query = orderBy(query);
        }

        if (pagingParams is not null)
        {
            query = query
                .Skip((pagingParams.PageNumber - 1) * pagingParams.PageSize)
                .Take(pagingParams.PageSize);
        }

        return query.AsNoTracking().ToListAsync();
    }

    public Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        var query = _dbSet.Where(filter);

        if (include is not null)
        {
            query = include(query);
        }

        return query.FirstOrDefaultAsync();
    }

    public virtual void Update(T entity)
    {
        var userId = _userContextService.GetUserId();
        if (userId == null) throw new UnauthorizedAccessException("User not authenticated");

        entity.UpdatedById = userId;
        entity.UpdatedAt = DateTimeOffset.UtcNow;

        _dbSet.Update(entity);
    }

    public virtual void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> range)
    {
        _dbSet.RemoveRange(range);
    }

    public int Count()
    {
        return _dbSet.Count();
    }
}
