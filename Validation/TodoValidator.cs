using FluentValidation;
using TodoListApi.Data.Models;

namespace TodoListApi.Validation;

public class TodoValidator : AbstractValidator<TodoInputModel>
{
    public TodoValidator()
    {
        RuleFor(todo => todo.Title).NotEmpty().WithMessage("Title required");
        //RuleFor(todo => todo.UserId).NotEmpty().WithMessage("UserId is required");
    }
}
