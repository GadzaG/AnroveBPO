using AnroveBPO.Application.Validations;
using AnroveBPO.Domain.Models.ValueObjects;
using FluentValidation;

namespace AnroveBPO.Application.Features.Items.Commands.CreateItem;

public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemCommandValidator()
    {
        RuleFor(x => x.Code)
            .MustBeValueObject(ItemCode.Create);
    }
}