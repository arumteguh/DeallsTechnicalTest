using DeallsTechnicalTest.Data;
using DeallsTechnicalTest.Models;

namespace DeallsTechnicalTest.Services
{
    public interface IAuditLogService
    {
        Task LogAsync(string requestId, string action, string tableName, string recordId, string changesJson, string employeeId = null);
    }
    public class AuditLogService : IAuditLogService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;

        public AuditLogService(IHttpContextAccessor httpContextAccessor, AppDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task LogAsync(string requestId, string action, string tableName, string recordId, string changesJson, string employeeId = null)
        {
            var ipAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "unknown";

            var log = new AuditLog
            {
                RequestId = requestId,
                Action = action,
                TableName = tableName,
                RecordId = recordId,
                Changes = changesJson,
                EmployeeId = employeeId,
                IPAddress = ipAddress,
                Timestamp = DateTime.UtcNow
            };

            _context.auditLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
