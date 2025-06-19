using DeallsTechnicalTest.Models.DTO;
using DeallsTechnicalTest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeallsTechnicalTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendancePeriodController : ControllerBase
    {
        private readonly IAttendancePeriodService _service;

        public AttendancePeriodController(IAttendancePeriodService service)
        {
            _service = service;
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> AddPeriod([FromBody] AttendancePeriodDto dto)
        {
            try
            {
                bool isAdmin = bool.Parse(User.FindFirst("IsAdmin")?.Value ?? "false");
                if (!isAdmin)
                    return Forbid("Only admin can run payroll.");

                string adminUser = User.FindFirst("EmployeeId")?.Value ?? "System";
                await _service.AddPeriodAsync(dto, adminUser);
                return Ok(new { message = "Attendance period added successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
        }

        [HttpPost("run-payroll")]
        [Authorize]
        public async Task<IActionResult> RunPayroll([FromBody] RunPayrollRequestDto dto)
        {
            try
            {

                bool isAdmin = bool.Parse(User.FindFirst("IsAdmin")?.Value ?? "false");
                if (!isAdmin)
                    return Forbid("Only admin can run payroll.");

                string runBy = User.FindFirst("EmployeeId")?.Value ?? "System";

                await _service.RunPayrollAsync(dto.PayrollNumber, runBy);
                return Ok(new { message = $"Payroll for '{dto.PayrollNumber}' has been successfully run." });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
        }
    }
}
