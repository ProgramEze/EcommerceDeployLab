using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PaymentDto>>> GetAll()
    {
        var payments = await _paymentService.GetAllAsync();

        return Ok(payments);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PaymentDto>> GetById(Guid id)
    {
        var payment = await _paymentService.GetByIdAsync(id);

        if (payment is null)
        {
            return NotFound(new
            {
                message = "Pago no encontrado."
            });
        }

        return Ok(payment);
    }

    [HttpGet("order/{orderId:guid}")]
    public async Task<ActionResult<IReadOnlyList<PaymentDto>>> GetByOrderId(Guid orderId)
    {
        var payments = await _paymentService.GetByOrderIdAsync(orderId);

        return Ok(payments);
    }

    [HttpPost]
    public async Task<ActionResult<PaymentDto>> Create(CreatePaymentDto dto)
    {
        var payment = await _paymentService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = payment.Id },
            payment
        );
    }

    [HttpPatch("{id:guid}/approve")]
    public async Task<ActionResult<PaymentDto>> Approve(Guid id)
    {
        var payment = await _paymentService.ApproveAsync(id);

        if (payment is null)
        {
            return NotFound(new
            {
                message = "Pago no encontrado."
            });
        }

        return Ok(payment);
    }

    [HttpPatch("{id:guid}/reject")]
    public async Task<ActionResult<PaymentDto>> Reject(Guid id)
    {
        var payment = await _paymentService.RejectAsync(id);

        if (payment is null)
        {
            return NotFound(new
            {
                message = "Pago no encontrado."
            });
        }

        return Ok(payment);
    }

    [HttpPatch("{id:guid}/cancel")]
    public async Task<ActionResult<PaymentDto>> Cancel(Guid id)
    {
        var payment = await _paymentService.CancelAsync(id);

        if (payment is null)
        {
            return NotFound(new
            {
                message = "Pago no encontrado."
            });
        }

        return Ok(payment);
    }
}