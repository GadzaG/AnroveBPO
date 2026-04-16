using FluentValidation;

namespace AnroveBPO.Application.Features.Items.Commands.UpdateItemPrice;

public class UpdateItemPriceCommandValidator : AbstractValidator<UpdateItemPriceCommand>
{
    public UpdateItemPriceCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id cannot be empty");
        RuleFor(x => x.Price).GreaterThan(0);
    }
}
