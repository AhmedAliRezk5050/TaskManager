using FluentValidation;
using Infrastructure.DTOs.TaskItems;

namespace Infrastructure.Validators.TaskItems;

public class AddTaskItemDtoValidator : AbstractValidator<AddTaskItemDTO>
{
    public AddTaskItemDtoValidator() {
        RuleFor(t => t.Title)
            .NotEmpty()
            .WithMessage("Title can't be empty");

        RuleFor(t => t.Description)
            .NotEmpty()
            .WithMessage("Title can't be empty");

        RuleFor(t => t.Status)
            .NotEmpty()
            .WithMessage("Status can't be empty");

        RuleFor(t => t.Description)
            .NotEmpty()
            .WithMessage("DueDate can't be empty");

    }
}
