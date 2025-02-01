using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;
using Infrastructure.Data;
using System;

namespace Infrastructure.Repository;

public class TaskItemRepository : BaseRepository<TaskItem>, ITaskItemRepository
{
    public TaskItemRepository(AppDbContext context, IUserContextService userContextService) : base(context, userContextService) { }
}
