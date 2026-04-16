using FluentValidation;

namespace AnroveBPO.Application.Features.Orders.Commands.CreateOrder;

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty().WithMessage("CustomerId cannot be empty");

        RuleFor(x => x.Items)
            .NotNull().WithMessage("Items cannot be null")
            .NotEmpty().WithMessage("Items cannot be empty")
            .Must(items => items.All(i => i.ItemId != Guid.Empty))
            .WithMessage("Each ItemId must be valid")
            .Must(items => items.All(i => i.Count > 0u))
            .WithMessage("Each item count must be greater than 0");
    }
}
