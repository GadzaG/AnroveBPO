using AnroveBPO.Application.Abstractions.Core;
using AnroveBPO.Application.Abstractions.Database;
using AnroveBPO.Application.Abstractions.Database.Repositories;
using AnroveBPO.Application.Validations;
using AnroveBPO.Domain.Models;
using AnroveBPO.Domain.Models.ValueObjects;
using AnroveBPO.Domain.Shared;
using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace AnroveBPO.Application.Features.Items.Commands.CreateItem;

public sealed class CreateItemHandler(
    ITransactionManager transactionManager,
    IItemsRepository repository,
    IValidator<CreateItemCommand> validator,
    ILogger<CreateItemHandler> logger) 
    : ICommandHandler<Guid, CreateItemCommand>
{
    public async Task<Result<Guid, Error>> Handle(CreateItemCommand command, CancellationToken ct = default)
    {
        ValidationResult validationResult = await validator.ValidateAsync(command, ct);
        if (!validationResult.IsValid)
        {
            logger.LogWarning("Validation Failed");
            return validationResult.ToError();
        }

        Result<ItemCode, Error> itemCodeResult = ItemCode.Create(command.Code);
        if (itemCodeResult.IsFailure)
            return itemCodeResult.Error;

        Result<bool, Error> existsByCode = await repository.ExistsBy(x => x.Code == itemCodeResult.Value, ct);
        if (existsByCode.IsFailure)
            return existsByCode.Error;
        if (existsByCode.Value)
            return Error.Conflict("item.code.already.exists", "Товар с таким кодом уже существует", "code");

        Result<bool, Error> existsByName = await repository.ExistsBy(x => x.Name == command.Name, ct);
        if (existsByName.IsFailure)
            return existsByName.Error;
        if (existsByName.Value)
            return Error.Conflict("item.name.already.exists", "Товар с таким названием уже существует", "name");

        var createItem = Item.Create(command.Name, command.Price, command.Category, itemCodeResult.Value);
        if (createItem.IsFailure)
            return createItem.Error;
        var addItem = await repository.AddAsync(createItem.Value, ct);
        if(addItem.IsFailure)
            return addItem.Error;

        var saveChanges = await transactionManager.SaveChangesAsync(ct);
        if (saveChanges.IsFailure)
            return saveChanges.Error;
        return createItem.Value.Id;
    }
}
