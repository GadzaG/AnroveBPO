using AnroveBPO.Application.Validations;
using AnroveBPO.Domain.Models.ValueObjects;
using FluentValidation;

namespace AnroveBPO.Application.Features.Items.Commands.UpdateItemCode;

public class UpdateItemCodeCommandValidator : AbstractValidator<UpdateItemCodeCommand>
{
    public UpdateItemCodeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id cannot be empty");
        RuleFor(x => x.Code)
            .MustBeValueObject(ItemCode.Create);
    }
}
