using AnroveBPO.Application.Abstractions.Core;
using AnroveBPO.Application.Abstractions.Database;
using AnroveBPO.Application.Abstractions.Database.Repositories;
using AnroveBPO.Application.Validations;
using AnroveBPO.Domain.Models;
using AnroveBPO.Domain.Shared;
using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace AnroveBPO.Application.Features.Orders.Commands.CreateOrder;

public sealed class CreateOrderHandler(
    ICustomersRepository customersRepository,
    IItemsRepository itemsRepository,
    IOrderRepository orderRepository,
    IOrderItemsRepository orderItemsRepository,
    ITransactionManager transactionManager,
    IValidator<CreateOrderCommand> validator,
    ILogger<CreateOrderHandler> logger) 
    : ICommandHandler<Guid, CreateOrderCommand>
{
    public async Task<Result<Guid, Error>> Handle(CreateOrderCommand command, CancellationToken ct = default)
    {
        ValidationResult validationResult = await validator.ValidateAsync(command, ct);
        if (!validationResult.IsValid)
        {
            logger.LogWarning("Validation Failed");
            return validationResult.ToError();
        }

        Result<Domain.Models.Customer, Error> customerResult = await customersRepository.GetBy(c => c.Id == command.CustomerId, ct);
        if (customerResult.IsFailure)
            return customerResult.Error;

        Result<uint, Error> orderNumberResult = await GenerateOrderNumberForCustomer(command.CustomerId, ct);
        if (orderNumberResult.IsFailure)
            return orderNumberResult.Error;

        Result<Order> createOrder = Order.Create(command.CustomerId, orderNumberResult.Value);
        if (createOrder.IsFailure)
            return GeneralErrors.ValueIsInvalid("order");

        Result<Guid, Error> addOrder = await orderRepository.AddAsync(createOrder.Value, ct);
        if (addOrder.IsFailure)
            return addOrder.Error;

        Result<List<(Guid ItemId, uint Count)>, Error> normalizedItemsResult = NormalizeOrderItems(command.Items);
        if (normalizedItemsResult.IsFailure)
            return normalizedItemsResult.Error;

        foreach ((Guid itemId, uint count) in normalizedItemsResult.Value)
        {
            Result<Item, Error> itemResult = await itemsRepository.GetBy(i => i.Id == itemId, ct);
            if (itemResult.IsFailure)
                return itemResult.Error;

            Result<OrderItem, Error> createOrderItem = OrderItem.Create(
                createOrder.Value.Id,
                itemId,
                count,
                itemResult.Value.Price);

            if (createOrderItem.IsFailure)
                return createOrderItem.Error;

            Result<Guid, Error> addOrderItem = await orderItemsRepository.AddAsync(createOrderItem.Value, ct);
            if (addOrderItem.IsFailure)
                return addOrderItem.Error;
        }

        UnitResult<Error> saveChanges = await transactionManager.SaveChangesAsync(ct);
        if (saveChanges.IsFailure)
            return saveChanges.Error;

        return createOrder.Value.Id;
    }

    private static Result<List<(Guid ItemId, uint Count)>, Error> NormalizeOrderItems(
        IReadOnlyCollection<(Guid ItemId, uint Count)> items)
    {
        try
        {
            List<(Guid ItemId, uint Count)> normalizedItems = items
                .GroupBy(x => x.ItemId)
                .Select(group => (
                    ItemId: group.Key,
                    Count: checked((uint)group.Aggregate(0UL, (acc, item) => checked(acc + item.Count)))))
                .ToList();

            return normalizedItems;
        }
        catch (OverflowException)
        {
            return Error.Validation(
                "order.items.count.overflow",
                "Items count is too large",
                "items");
        }
    }

    private async Task<Result<uint, Error>> GenerateOrderNumberForCustomer(Guid customerId, CancellationToken ct)
    {
        Result<long, Error> ordersCountResult = await orderRepository.CountBy(o => o.CustomerId == customerId, ct);
        if (ordersCountResult.IsFailure)
            return ordersCountResult.Error;

        try
        {
            uint orderNumber = checked((uint)(ordersCountResult.Value + 1));
            return orderNumber;
        }
        catch (OverflowException)
        {
            return Error.Validation(
                "order.number.overflow",
                "Order number is too large for this customer",
                "orderNumber");
        }
    }
}
