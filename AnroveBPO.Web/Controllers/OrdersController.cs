using AnroveBPO.Application.Abstractions;
using AnroveBPO.Application.Abstractions.Database;
using AnroveBPO.Application.Abstractions.Database.Repositories;
using AnroveBPO.Application.Features.Orders.Commands.CreateOrder;
using AnroveBPO.Application.Features.Orders.Commands.UpdateOrderStatus;
using AnroveBPO.Contracts.Orders.Dto;
using AnroveBPO.Contracts.Orders.Requests;
using AnroveBPO.Domain.Models;
using AnroveBPO.Domain.Models.Enums;
using AnroveBPO.Domain.Shared;
using AnroveBPO.Infrastructure.Identity.Models;
using AnroveBPO.Web.EndpointResults;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EndpointResult = AnroveBPO.Web.EndpointResults.EndpointResult;

namespace AnroveBPO.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<EndpointResult<Guid>> CreateOrder(
        [FromBody] CreateOrderRequest request,
        [FromServices] CreateOrderHandler handler,
        CancellationToken ct = default)
    {
        List<(Guid ItemId, uint Count)> items = request.Items
            .Select(x => (x.ItemId, x.Count))
            .ToList();

        CreateOrderCommand command = new(request.CustomerId, items);
        return await handler.Handle(command, ct);
    }

    //public record UpdateOrderRequest(Guid CustomerId, string Status);
    
    /*
    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<EndpointResult> UpdateOrderStatus(
        [FromBody] UpdateOrderRequest request,
        [FromServices] UpdateOrderHandler handler, 
        CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }*/

    
}
