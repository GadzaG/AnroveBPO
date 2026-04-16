using AnroveBPO.Application.Abstractions.Core;
using AnroveBPO.Application.Abstractions.Database;
using AnroveBPO.Application.Abstractions.Database.Repositories;
using AnroveBPO.Application.Validations;
using AnroveBPO.Domain.Shared;
using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace AnroveBPO.Application.Features.Items.Commands.UpdateItemName;

public sealed class UpdateItemNameHandler(
    ITransactionManager transactionManager,
    IItemsRepository itemsRepository,
    IValidator<UpdateItemNameCommand> validator,
    ILogger<UpdateItemNameHandler> logger) 
    : ICommandHandler<UpdateItemNameCommand>
{
    public async Task<UnitResult<Error>> Handle(UpdateItemNameCommand command, CancellationToken ct = default)
    {
        ValidationResult validationResult = await validator.ValidateAsync(command, ct);
        if (!validationResult.IsValid)
        {
            logger.LogWarning("Validation Failed");
            return validationResult.ToError();
        }

        var getItem = await itemsRepository.GetBy(i => i.Id == command.Id, ct);
        if (getItem.IsFailure)
            return getItem.Error;

        var existsByName = await itemsRepository.ExistsBy(i => i.Id != command.Id && i.Name == command.Name, ct);
        if (existsByName.IsFailure)
            return existsByName.Error;
        if (existsByName.Value)
            return Error.Conflict("item.name.already.exists", "Товар с таким названием уже существует", "name");

        var item = getItem.Value;
        var updateName = item.UpdateName(command.Name);
        if(updateName.IsFailure)
            return updateName.Error;
        var saveChanges = await transactionManager.SaveChangesAsync(ct);
        return saveChanges.IsFailure ? saveChanges.Error : UnitResult.Success<Error>();
    }
}
