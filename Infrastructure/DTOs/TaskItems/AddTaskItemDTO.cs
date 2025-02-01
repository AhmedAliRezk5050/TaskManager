using Core.Enums;

namespace Infrastructure.DTOs.TaskItems;

public class AddTaskItemDTO
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public TaskItemStatus? Status { get; set; }
    public DateTimeOffset? DueDate { get; set; }
}

