using DeallsTechnicalTest.Models.DTO;
using DeallsTechnicalTest.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeallsTechnicalTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OvertimeController : ControllerBase
    {
        private readonly IOvertimeService _service;

        public OvertimeController(IOvertimeService service)
        {
            _service = service;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitOvertime([FromBody] OvertimeRequestDto dto)
        {
            try
            {
                string submittedBy = User.Identity?.Name ?? "System"; // Adjust depending on auth setup
                await _service.SubmitOvertimeAsync(dto, submittedBy);
                return Ok(new { message = "Overtime submitted successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An error occurred while submitting overtime." });
            }
        }
    }
}
