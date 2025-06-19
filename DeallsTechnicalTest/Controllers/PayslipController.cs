using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DeallsTechnicalTest.Services;

namespace DeallsTechnicalTest.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PayslipController : ControllerBase
    {
        private readonly IPayslipService _payslipService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PayslipController(IPayslipService payslipService, IHttpContextAccessor httpContextAccessor)
        {
            _payslipService = payslipService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("generate/{payrollNumber}")]
        public async Task<IActionResult> GeneratePayslip(string payrollNumber)
        {
            var employeeId = User.FindFirst("EmployeeId")?.Value;

            if (string.IsNullOrEmpty(employeeId))
                return Unauthorized("EmployeeId not found in token");

            var payslip = await _payslipService.GeneratePayslipAsync(employeeId, payrollNumber);
            return Ok(payslip);
        }

        [HttpGet("summary/{payrollNumber}")]
        public async Task<IActionResult> GetSummary(string payrollNumber)
        {
            var isAdmin = bool.Parse(User.FindFirst("IsAdmin")?.Value ?? "false");
            if (!isAdmin)
                return Forbid("Only admins can access payslip summaries.");

            var result = await _payslipService.GenerateSummaryAsync(payrollNumber);
            return Ok(result);
        }
    }
}
