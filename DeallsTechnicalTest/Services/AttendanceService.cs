using DeallsTechnicalTest.Models;
using DeallsTechnicalTest.Models.DTO;
using DeallsTechnicalTest.Repositories;
using System.Data;
using System.Text.Json;

namespace DeallsTechnicalTest.Services
{
    public interface IAttendanceService
    {
        Task SubmitAttendanceAsync(string employeeId, AttendanceRequestDto dto, string submittedBy);
    }
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _repository;
        private readonly IAuditLogService _auditLogService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AttendanceService(IAttendanceRepository repository, IAuditLogService auditLogService, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _auditLogService = auditLogService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SubmitAttendanceAsync(string employeeId, AttendanceRequestDto dto, string submittedBy)
        {
            if (dto.CheckIn.Date != dto.CheckOut.Date)
                throw new ArgumentException("CheckIn and CheckOut must be on the same day.");

            if (dto.CheckIn.DayOfWeek == DayOfWeek.Saturday || dto.CheckIn.DayOfWeek == DayOfWeek.Sunday)
                throw new ArgumentException("Cannot submit attendance on weekends.");

            var existing = await _repository.GetAttendanceByDateAsync(employeeId, dto.CheckIn.Date);
            if (existing != null)
                throw new InvalidOperationException("Attendance already submitted for this day.");

            var attendance = new Attendance
            {
                EmployeeId = employeeId,
                CheckIn = dto.CheckIn,
                CheckOut = dto.CheckOut,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedBy = submittedBy,
                UpdatedBy = submittedBy
            };

            await _repository.AddAttendanceAsync(attendance);

            var changes = JsonSerializer.Serialize(new
            {
                dto.CheckIn,
                dto.CheckOut
            });

            var requestId = _httpContextAccessor.HttpContext?.Items["RequestId"]?.ToString();

            await _auditLogService.LogAsync(
                requestId,
                "Create",
                "Attendance",
                attendance.Id.ToString(),
                changes,
                employeeId
            );
        }
    }
}
