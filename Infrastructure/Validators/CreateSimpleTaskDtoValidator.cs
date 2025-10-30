using FluentValidation;
using SimpleTaskManagerProject.Infrastructure.Dto;

namespace SimpleTaskManagerProject.Infrastructure.Validators;

public class CreateSimpleTaskDtoValidator : AbstractValidator<CreateSimpleTaskDto>
{
    public CreateSimpleTaskDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
    }
}