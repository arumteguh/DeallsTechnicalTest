using DeallsTechnicalTest.Models;
using DeallsTechnicalTest.Models.DTO;
using DeallsTechnicalTest.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json;

namespace DeallsTechnicalTest.Services
{
    public interface IAttendancePeriodService
    {
        Task AddPeriodAsync(AttendancePeriodDto dto, string createdBy);
        Task RunPayrollAsync(string payrollNumber, string runBy);
    }
    public class AttendancePeriodService : IAttendancePeriodService
    {
        private readonly IAttendancePeriodRepository _repository;
        private readonly IAuditLogService _auditLogService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AttendancePeriodService(IAttendancePeriodRepository repository, IAuditLogService auditLogService, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _auditLogService = auditLogService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task AddPeriodAsync(AttendancePeriodDto dto, string createdBy)
        {
            if (dto.PeriodStart > dto.PeriodEnd)
                throw new ArgumentException("Start date must not be after end date.");

            if (await _repository.PayrollNumberExistsAsync(dto.PayrollNumber))
                throw new InvalidOperationException("PayrollNumber already exists.");

            var period = new AttendancePeriod
            {
                PayrollNumber = dto.PayrollNumber,
                PeriodStart = dto.PeriodStart,
                PeriodEnd = dto.PeriodEnd,
                CreatedBy = createdBy,
                UpdatedBy = createdBy,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };

            await _repository.AddAsync(period);

            var changes = JsonSerializer.Serialize(new
            {
                dto.PayrollNumber,
                dto.PeriodStart,
                dto.PeriodEnd
            });

            var requestId = _httpContextAccessor.HttpContext?.Items["RequestId"]?.ToString();

            await _auditLogService.LogAsync(
                requestId,
                "Create",
                "AttendancePeriod",
                period.Id.ToString(),
                changes,
                createdBy
            );
        }

        public async Task RunPayrollAsync(string payrollNumber, string runBy)
        {
            var period = await _repository.GetByPayrollNumberAsync(payrollNumber);
            if (period == null)
                throw new ArgumentException("PayrollNumber not found.");

            if (period.IsPayrollRun)
                throw new InvalidOperationException("Payroll has already been run for this period.");

            period.IsPayrollRun = true;
            period.PayrollRunDate = DateTime.Now;
            period.PayrollRunBy = runBy;
            period.UpdatedDate = DateTime.Now;
            period.UpdatedBy = runBy;

            await _repository.UpdateAsync(period);
            

            var changes = JsonSerializer.Serialize(period);

            var requestId = _httpContextAccessor.HttpContext?.Items["RequestId"]?.ToString();

            await _auditLogService.LogAsync(
                requestId,
                "Update",
                "AttendancePeriod",
                period.Id.ToString(),
                changes,
                runBy
            );
        }
    }
}
