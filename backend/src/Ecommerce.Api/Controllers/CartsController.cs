using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/carts")]
public class CartsController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartsController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpPost]
    public async Task<ActionResult<CartDto>> Create()
    {
        var cart = await _cartService.CreateAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = cart.Id },
            cart
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CartDto>> GetById(Guid id)
    {
        var cart = await _cartService.GetByIdAsync(id);

        if (cart is null)
        {
            return NotFound(new
            {
                message = "Carrito no encontrado."
            });
        }

        return Ok(cart);
    }

    [HttpPost("{cartId:guid}/items")]
    public async Task<ActionResult<CartDto>> AddItem(Guid cartId, AddCartItemDto dto)
    {
        var cart = await _cartService.AddItemAsync(cartId, dto);

        if (cart is null)
        {
            return NotFound(new
            {
                message = "Carrito no encontrado."
            });
        }

        return Ok(cart);
    }

    [HttpPut("{cartId:guid}/items/{productId:guid}")]
    public async Task<ActionResult<CartDto>> ChangeItemQuantity(
        Guid cartId,
        Guid productId,
        UpdateCartItemQuantityDto dto
    )
    {
        var cart = await _cartService.ChangeItemQuantityAsync(
            cartId,
            productId,
            dto
        );

        if (cart is null)
        {
            return NotFound(new
            {
                message = "Carrito no encontrado."
            });
        }

        return Ok(cart);
    }

    [HttpDelete("{cartId:guid}/items/{productId:guid}")]
    public async Task<ActionResult<CartDto>> RemoveItem(Guid cartId, Guid productId)
    {
        var cart = await _cartService.RemoveItemAsync(cartId, productId);

        if (cart is null)
        {
            return NotFound(new
            {
                message = "Carrito no encontrado."
            });
        }

        return Ok(cart);
    }

    [HttpDelete("{cartId:guid}/items")]
    public async Task<ActionResult<CartDto>> Clear(Guid cartId)
    {
        var cart = await _cartService.ClearAsync(cartId);

        if (cart is null)
        {
            return NotFound(new
            {
                message = "Carrito no encontrado."
            });
        }

        return Ok(cart);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _cartService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                message = "Carrito no encontrado."
            });
        }

        return NoContent();
    }
}