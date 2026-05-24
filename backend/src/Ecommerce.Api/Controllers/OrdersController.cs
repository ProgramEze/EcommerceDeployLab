using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetAll()
    {
        var orders = await _orderService.GetAllAsync();

        return Ok(orders);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderDto>> GetById(Guid id)
    {
        var order = await _orderService.GetByIdAsync(id);

        if (order is null)
        {
            return NotFound(new
            {
                message = "Orden no encontrada."
            });
        }

        return Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> Create(CreateOrderDto dto)
    {
        var order = await _orderService.CreateFromCartAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = order.Id },
            order
        );
    }

    [HttpPatch("{id:guid}/confirm")]
    public async Task<ActionResult<OrderDto>> Confirm(Guid id)
    {
        var order = await _orderService.ConfirmAsync(id);

        if (order is null)
        {
            return NotFound(new
            {
                message = "Orden no encontrada."
            });
        }

        return Ok(order);
    }

    [HttpPatch("{id:guid}/cancel")]
    public async Task<ActionResult<OrderDto>> Cancel(Guid id)
    {
        var order = await _orderService.CancelAsync(id);

        if (order is null)
        {
            return NotFound(new
            {
                message = "Orden no encontrada."
            });
        }

        return Ok(order);
    }
}