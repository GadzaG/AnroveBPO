using AnroveBPO.Application.Abstractions.Core;
using AnroveBPO.Application.Abstractions.Database;
using AnroveBPO.Application.Abstractions.Database.Repositories;
using AnroveBPO.Application.Validations;
using AnroveBPO.Domain.Shared;
using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace AnroveBPO.Application.Features.Items.Commands.DeleteItem;

public sealed class DeleteItemHandler(
    IItemsRepository itemsRepository,
    IValidator<DeleteItemCommand> validator,
    ITransactionManager transactionManager,
    ILogger<DeleteItemHandler> logger) : ICommandHandler<DeleteItemCommand>
{
    public async Task<UnitResult<Error>> Handle(DeleteItemCommand command, CancellationToken ct = default)
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
        var deleteItem = itemsRepository.Delete(getItem.Value);
        if(deleteItem.IsFailure)
            return deleteItem.Error;
        var saveChanges = await transactionManager.SaveChangesAsync(ct);
        return saveChanges.IsFailure ? saveChanges.Error : UnitResult.Success<Error>();
    }
}