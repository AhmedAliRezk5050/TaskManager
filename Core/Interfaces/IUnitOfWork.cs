namespace Core.Interfaces;

public interface IUnitOfWork
{
    public ITaskItemRepository TaskItemRepository { get; set; }
    Task<bool> SaveAsync();
}
