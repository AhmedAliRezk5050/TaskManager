using Core.Enums;

namespace Core.Entities;

public class TaskItem : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public TaskItemStatus Status { get; set; } = TaskItemStatus.ToDo;
    public DateTimeOffset DueDate { get; set; }
}
