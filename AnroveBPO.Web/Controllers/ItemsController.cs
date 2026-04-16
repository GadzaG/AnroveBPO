using AnroveBPO.Application.Abstractions;
using AnroveBPO.Application.Features.Items.Commands.CreateItem;
using AnroveBPO.Application.Features.Items.Commands.DeleteItem;
using AnroveBPO.Application.Features.Items.Commands.UpdateItemCategory;
using AnroveBPO.Application.Features.Items.Commands.UpdateItemCode;
using AnroveBPO.Application.Features.Items.Commands.UpdateItemName;
using AnroveBPO.Application.Features.Items.Commands.UpdateItemPrice;
using AnroveBPO.Application.Features.Items.Queries.GetAllItems;
using AnroveBPO.Contracts.Items.Dto;
using AnroveBPO.Contracts.Items.Requests;
using AnroveBPO.Infrastructure.Identity.Models;
using AnroveBPO.Web.EndpointResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EndpointResult = AnroveBPO.Web.EndpointResults.EndpointResult;

namespace AnroveBPO.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class ItemsController : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<EndpointResult<Guid>> CreateItem(
        [FromBody] CreateItemRequest request,
        [FromServices] CreateItemHandler handler,
        CancellationToken ct = default)
    {
        CreateItemCommand command = new(
            request.Code,
            request.Name,
            request.Price,
            request.Category);

        return await handler.Handle(command, ct);
    }

    [HttpGet]
    public async Task<EndpointResult<PagedResult<ItemDto>>> GetAllItems(
        [FromServices] GetAllItemsHandler handler,
        [FromQuery] GetAllItemsRequest request,
        CancellationToken ct = default)
    {
        GetAllItemsQuery query = new(request.Page, request.PageSize);
        return await handler.Handle(query, ct);
    }

    [HttpDelete("{id:guid}")]
    public async Task<EndpointResult> DeleteItem(
        [FromRoute] Guid id,
        [FromServices] DeleteItemHandler handler,
        CancellationToken ct = default)
    {
        DeleteItemCommand command = new(id);
        return await handler.Handle(command, ct);
    }

    [HttpPatch("{id:guid}/name")]
    public async Task<EndpointResult> UpdateItemName(
        [FromRoute] Guid id,
        [FromBody] UpdateItemNameRequest request,
        [FromServices] UpdateItemNameHandler handler,
        CancellationToken ct = default)
    {
        UpdateItemNameCommand command = new(id, request.Name);
        return await handler.Handle(command, ct);
    }

    [HttpPatch("{id:guid}/code")]
    public async Task<EndpointResult> UpdateItemCode(
        [FromRoute] Guid id,
        [FromBody] UpdateItemCodeRequest request,
        [FromServices] UpdateItemCodeHandler handler,
        CancellationToken ct = default)
    {
        UpdateItemCodeCommand command = new(id, request.Code);
        return await handler.Handle(command, ct);
    }

    [HttpPatch("{id:guid}/category")]
    public async Task<EndpointResult> UpdateItemCategory(
        [FromRoute] Guid id,
        [FromBody] UpdateItemCategoryRequest request,
        [FromServices] UpdateItemCategoryHandler handler,
        CancellationToken ct = default)
    {
        UpdateItemCategoryCommand command = new(id, request.Category);
        return await handler.Handle(command, ct);
    }

    [HttpPatch("{id:guid}/price")]
    public async Task<EndpointResult> UpdateItemPrice(
        [FromRoute] Guid id,
        [FromBody] UpdateItemPriceRequest request,
        [FromServices] UpdateItemPriceHandler handler,
        CancellationToken ct = default)
    {
        UpdateItemPriceCommand command = new(id, request.Price);
        return await handler.Handle(command, ct);
    }
}
