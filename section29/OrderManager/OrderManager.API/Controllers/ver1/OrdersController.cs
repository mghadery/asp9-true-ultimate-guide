using Asp.Versioning;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OrderManager.Core.DTOs;
using OrderManager.Core.ServiceContracts;

namespace OrderManager.API.Controllers.ver1;

[ApiVersion("1.0")]
[Route("api/ver{version:apiVersion}/orders")]
[ApiController]
//[EnableCors("TestPolicy")]
public class OrdersController(IOrdersService ordersService, ILogger<OrdersController> logger) : ControllerBase
{
    [HttpGet("test")]
    public async Task<ActionResult> test()
    {
        return Ok("version 1");
    }

    /// <summary>
    /// Get all orders sorted by OrderDate
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult> GetAllOrders()
    {
        try
        {
            logger.LogInformation($"{ControllerContext.ActionDescriptor.ActionName} Started");
            var orders = await ordersService.GetAll();
            return Ok(orders);
        }
        finally
        {
            logger.LogInformation($"{ControllerContext.ActionDescriptor.ActionName} Ended");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetOrderById(Guid id)
    {
        var order = await ordersService.GetById(id);
        return Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult> AddOrder(OrderAddRequest orderAddRequest)
    {
        var order = await ordersService.Create(orderAddRequest);
        return Created($"/{order.OrderId}", order);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateOrder(OrderEditRequest orderEditRequest)
    {
        var order = await ordersService.Update(orderEditRequest);
        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOrder(Guid id)
    {
        var success = await ordersService.Delete(id);
        return success ? NoContent() : NotFound(new ProblemDetails()
        {
            Detail = "Not found",
            Status = StatusCodes.Status404NotFound,
            Title = "Delete error"
        }
        );
    }

    [HttpGet("{orderId}/items")]
    public async Task<ActionResult> GetItemsByOrderId(Guid orderId)
    {
        var items = await ordersService.GetItemsByOrderId(orderId);
        return Ok(items);
    }

    [HttpPost("{orderId}/items")]
    public async Task<ActionResult> AddItem(Guid orderId, OrderItemAddRequest orderItemAddRequest)
    {
        if (orderId != orderItemAddRequest.OrderId)
            return BadRequest("Order ids don't match");
        var item = await ordersService.AddItem(orderItemAddRequest);
        return Ok(item);
    }

    [HttpPut("{orderId}/items/{id}")]
    public async Task<ActionResult> UpdateItem(Guid orderId, Guid id, OrderItemUpdateRequest orderItemUpdateRequest)
    {
        if (orderId != orderItemUpdateRequest.OrderId || id != orderItemUpdateRequest.OrderItemId)
            return BadRequest("Ids don't match");
        var item = await ordersService.UpdateItem(orderItemUpdateRequest);
        return Ok(item);
    }

    [HttpDelete("{orderId}/items/{id}")]
    public async Task<ActionResult> DeleteItem(Guid orderId, Guid id)
    {
        var item = await ordersService.DeleteItem(orderId, id);
        //TODO: Delete order if no item is left
        return item ? Ok() : NotFound();
    }
}
