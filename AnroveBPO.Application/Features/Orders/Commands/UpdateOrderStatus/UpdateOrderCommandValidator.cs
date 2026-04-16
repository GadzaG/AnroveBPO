using AnroveBPO.Domain.Models.Enums;
using FluentValidation;

namespace AnroveBPO.Application.Features.Orders.Commands.UpdateOrderStatus;

public sealed class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id cannot be empty");
        RuleFor(x => x.Status).IsInEnum();

        RuleFor(x => x.ShipmentDate)
            .NotNull()
            .When(x => x.Status == OrderStatus.IN_PROGRESS)
            .WithMessage("ShipmentDate is required for IN_PROGRESS status");
    }
}
