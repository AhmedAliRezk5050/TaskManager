using Core.Enums;
using Infrastructure.Data;

namespace Infrastructure.DTOs.TaskItems;

public class TaskItemDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public TaskItemStatus Status { get; set; } = TaskItemStatus.ToDo;
    public DateTimeOffset DueDate { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public string CreatedById { get; set; } = null!;
    public string? UpdatedById { get; set; }
    public string CreatedByName { get; set; } = null!;
    public string? UpdatedByName { get; set; }

}
