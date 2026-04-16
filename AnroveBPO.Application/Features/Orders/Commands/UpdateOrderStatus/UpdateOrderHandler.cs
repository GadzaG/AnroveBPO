using AnroveBPO.Application.Abstractions.Core;
using AnroveBPO.Application.Abstractions.Database;
using AnroveBPO.Application.Abstractions.Database.Repositories;
using AnroveBPO.Application.Validations;
using AnroveBPO.Domain.Models.Enums;
using AnroveBPO.Domain.Shared;
using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace AnroveBPO.Application.Features.Orders.Commands.UpdateOrderStatus;

public sealed class UpdateOrderHandler(
    IOrderRepository orderRepository,
    ITransactionManager transactionManager,
    IValidator<UpdateOrderCommand> validator,
    ILogger<UpdateOrderHandler> logger) 
    : ICommandHandler<UpdateOrderCommand>
{
    public async Task<UnitResult<Error>> Handle(UpdateOrderCommand command, CancellationToken ct = default)
    {
        ValidationResult validationResult = await validator.ValidateAsync(command, ct);
        if (!validationResult.IsValid)
        {
            logger.LogWarning("Validation Failed");
            return validationResult.ToError();
        }

        Result<Domain.Models.Order, Error> orderResult = await orderRepository.GetBy(o => o.Id == command.Id, ct);
        if (orderResult.IsFailure)
            return orderResult.Error;

        Domain.Models.Order order = orderResult.Value;

        UnitResult<Error> updateResult = command.Status switch
        {
            OrderStatus.IN_PROGRESS => order.SetInProgress(command.ShipmentDate!.Value),
            OrderStatus.COMPLETED => order.SetCompleted(),
            _ => Error.Validation(
                "order.status.transition.unsupported",
                "Поддерживаются только переходы в IN_PROGRESS и COMPLETED",
                "status")
        };

        if (updateResult.IsFailure)
            return updateResult.Error;

        UnitResult<Error> saveChanges = await transactionManager.SaveChangesAsync(ct);
        return saveChanges.IsFailure ? saveChanges.Error : UnitResult.Success<Error>();
    }
}
