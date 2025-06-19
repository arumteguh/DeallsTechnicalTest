using DeallsTechnicalTest.Models;
using DeallsTechnicalTest.Models.DTO;
using DeallsTechnicalTest.Repositories;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace DeallsTechnicalTest.Services
{

    public interface IOvertimeService
    {
        Task SubmitOvertimeAsync(OvertimeRequestDto dto, string submittedBy);
    }
    public class OvertimeService : IOvertimeService
    {
        private readonly IOvertimeRepository _repository;
        private readonly IAuditLogService _auditLogService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OvertimeService(IOvertimeRepository repository, IAuditLogService auditLogService, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _auditLogService = auditLogService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SubmitOvertimeAsync(OvertimeRequestDto dto, string submittedBy)
        {
            if (dto.OvertimeEnd <= dto.OvertimeStart)
                throw new ArgumentException("End time must be after start time.");

            if (dto.OvertimeEnd > DateTime.Now)
                throw new ArgumentException("Overtime must be submitted after it is completed.");

            double totalHours = (dto.OvertimeEnd - dto.OvertimeStart).TotalHours;

            if (totalHours > 3)
                throw new ArgumentException("Overtime cannot exceed 3 hours per day.");

            var overtime = new Overtime
            {
                EmployeeId = dto.EmployeeId,
                OvertimeStart = dto.OvertimeStart,
                OvertimeEnd = dto.OvertimeEnd,
                OvertimeTotal = Math.Round(totalHours, 2),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedBy = submittedBy,
                UpdatedBy = submittedBy
            };

            await _repository.AddOvertimeAsync(overtime);

            var changes = JsonSerializer.Serialize(dto);

            var requestId = _httpContextAccessor.HttpContext?.Items["RequestId"]?.ToString();

            await _auditLogService.LogAsync(
                requestId,
                "Create",
                "Overtime",
                overtime.Id.ToString(),
                changes,
                dto.EmployeeId
            );
        }
    }
}
