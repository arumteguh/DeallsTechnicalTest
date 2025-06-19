using DeallsTechnicalTest.Models.DTO;
using DeallsTechnicalTest.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeallsTechnicalTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReimbursementController : ControllerBase
    {
        private readonly IReimbursementService _service;

        public ReimbursementController(IReimbursementService service)
        {
            _service = service;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitReimbursement([FromBody] ReimbursementRequestDto dto)
        {
            try
            {
                //string employeeId = User.Identity?.Name ?? "Unknown"; // Replace with real auth logic
                string employeeId = "75374";
                string submittedBy = employeeId;

                await _service.SubmitReimbursementAsync(employeeId, dto, submittedBy);
                return Ok(new { message = "Reimbursement submitted successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An error occurred while submitting reimbursement." });
            }
        }
    }
}
