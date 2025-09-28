using FluentValidation;
using TodoListApi.Dto;
namespace TodoListApi.Validation;

public class TodoValidator : AbstractValidator<TodoInputModel>
{
    public TodoValidator()
    {
        RuleFor(todo => todo.Title).NotEmpty().WithMessage("Title required");
    }
}
