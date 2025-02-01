using Core.Interfaces;
using Core.Interfaces.Services;
using Infrastructure.Data;
using System;

namespace Infrastructure.Repository;
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;

    public ITaskItemRepository TaskItemRepository { get; set; }

    public UnitOfWork(AppDbContext dbContext, IUserContextService userContextService)
    {
        _dbContext = dbContext;
        TaskItemRepository = new TaskItemRepository(dbContext, userContextService);
    }

    public async Task<bool> SaveAsync()
    {
        return await _dbContext.SaveChangesAsync() > 0;
    }
}
