using FluentValidation;
using TodoListApi.Data.Models;

namespace TodoListApi.Validation;

public class UserValidator : AbstractValidator<UserInputModel>
{
    public UserValidator()
    {
        RuleFor(u => u.Name)
            .NotEmpty()
            .MaximumLength(128);
    }
}
