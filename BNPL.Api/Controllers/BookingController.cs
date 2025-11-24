using BNPL.Application.Contracts;
using BNPL.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly BookingService _service;

    public BookingsController(BookingService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] BookingRequest request)
    {
        try
        {
            var result = await _service.ProcessAsync(request);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                error = ex.Message,
                stack = ex.StackTrace
            });
        }
    }
}
