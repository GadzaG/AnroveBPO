using FluentValidation;

namespace AnroveBPO.Application.Features.Items.Commands.UpdateItemCategory;

public class UpdateItemCategoryCommandValidator : AbstractValidator<UpdateItemCategoryCommand>
{
    public UpdateItemCategoryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id cannot be empty");
        RuleFor(x => x.Category).Length(1, 96).NotEmpty();
    }
}
