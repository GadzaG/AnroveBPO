using AnroveBPO.Application.Abstractions.Core;
using AnroveBPO.Application.Abstractions.Database;
using AnroveBPO.Application.Abstractions.Database.Repositories;
using AnroveBPO.Application.Validations;
using AnroveBPO.Domain.Shared;
using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace AnroveBPO.Application.Features.Items.Commands.UpdateItemPrice;

public sealed class UpdateItemPriceHandler(
    ITransactionManager transactionManager,
    IItemsRepository itemsRepository,
    IValidator<UpdateItemPriceCommand> validator,
    ILogger<UpdateItemPriceHandler> logger)
    : ICommandHandler<UpdateItemPriceCommand>
{
    public async Task<UnitResult<Error>> Handle(UpdateItemPriceCommand command, CancellationToken ct = default)
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

        var item = getItem.Value;
        var updatePrice = item.UpdatePrice(command.Price);
        if (updatePrice.IsFailure)
            return updatePrice.Error;

        var saveChanges = await transactionManager.SaveChangesAsync(ct);
        return saveChanges.IsFailure ? saveChanges.Error : UnitResult.Success<Error>();
    }
}
