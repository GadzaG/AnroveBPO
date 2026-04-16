using FluentValidation;

namespace AnroveBPO.Application.Features.Items.Commands.UpdateItemName;

public class UpdateItemNameCommandValidator : AbstractValidator<UpdateItemNameCommand>
{
    public UpdateItemNameCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id cannot be empty");
        RuleFor(x => x.Name).Length(1, 100).NotEmpty();
    }
}