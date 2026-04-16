using AnroveBPO.Application.Abstractions.Core;

namespace AnroveBPO.Application.Features.Items.Commands.UpdateItemCategory;

public record UpdateItemCategoryCommand(Guid Id, string Category) : ICommand;
