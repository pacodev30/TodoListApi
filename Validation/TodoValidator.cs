using FluentValidation;
using TodoListApi.Data.Models;

namespace TodoListApi.Validation;

public class TodoValidator : AbstractValidator<TodoInputModel>
{
    public TodoValidator()
    {
        RuleFor(todo => todo.Title)
            .NotEmpty()
            .MaximumLength(1024)
            .WithMessage("Title required");
        //RuleFor(todo => todo.UserId).NotEmpty().WithMessage("UserId is required");
    }
}
