using AnroveBPO.Application.Abstractions.Core;
using AnroveBPO.Application.Abstractions.Database;
using AnroveBPO.Application.Abstractions.Database.Repositories;
using AnroveBPO.Application.Validations;
using AnroveBPO.Domain.Models.ValueObjects;
using AnroveBPO.Domain.Shared;
using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace AnroveBPO.Application.Features.Items.Commands.UpdateItemCode;

public sealed class UpdateItemCodeHandler(
    ITransactionManager transactionManager,
    IItemsRepository itemsRepository,
    IValidator<UpdateItemCodeCommand> validator,
    ILogger<UpdateItemCodeHandler> logger)
    : ICommandHandler<UpdateItemCodeCommand>
{
    public async Task<UnitResult<Error>> Handle(UpdateItemCodeCommand command, CancellationToken ct = default)
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

        var getItem = await itemsRepository.GetBy(i => i.Id == command.Id, ct);
        if (getItem.IsFailure)
            return getItem.Error;

        var existsByCode = await itemsRepository.ExistsBy(i => i.Id != command.Id && i.Code == itemCodeResult.Value, ct);
        if (existsByCode.IsFailure)
            return existsByCode.Error;
        if (existsByCode.Value)
            return Error.Conflict("item.code.already.exists", "Товар с таким кодом уже существует", "code");

        var item = getItem.Value;
        var updateCode = item.UpdateCode(itemCodeResult.Value);
        if (updateCode.IsFailure)
            return updateCode.Error;

        var saveChanges = await transactionManager.SaveChangesAsync(ct);
        return saveChanges.IsFailure ? saveChanges.Error : UnitResult.Success<Error>();
    }
}
