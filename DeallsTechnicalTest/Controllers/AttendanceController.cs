using DeallsTechnicalTest.Models.DTO;
using DeallsTechnicalTest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeallsTechnicalTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _service;

        public AttendanceController(IAttendanceService service)
        {
            _service = service;
        }

        [HttpPost("submit")]
        [Authorize]
        public async Task<IActionResult> SubmitAttendance([FromBody] AttendanceRequestDto dto)
        {
            try
            {

                //string employeeId = User.Identity?.Name ?? "Unknown"; // Replace with actual auth logic
                string employeeId = User.FindFirst("EmployeeId")?.Value ?? "Unknown";
                //string employeeId = "75374";
                string submittedBy = employeeId;

                await _service.SubmitAttendanceAsync(employeeId, dto, submittedBy);
                return Ok(new { message = "Attendance submitted successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An error occurred while submitting attendance." });
            }
        }
    }
}
