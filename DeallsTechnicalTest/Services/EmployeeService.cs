using DeallsTechnicalTest.Models;
using DeallsTechnicalTest.Repositories;
using System.Text.Json;

namespace DeallsTechnicalTest.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task AddAsync(Employee employee);
        Task<Employee?> UpdateAsync(Employee employee);
        Task<bool> DeleteAsync(int id);
    }
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly IAuditLogService _auditLogService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmployeeService(IEmployeeRepository repository, IAuditLogService auditLogService, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _auditLogService = auditLogService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task AddAsync(Employee employee)
        {
            await _repository.AddAsync(employee);

            var changes = JsonSerializer.Serialize(employee);

            var requestId = _httpContextAccessor.HttpContext?.Items["RequestId"]?.ToString();

            await _auditLogService.LogAsync(
                requestId,
                "Create",
                "Employee",
                employee.Id.ToString(),
                changes,
                employee.EmployeeId
            );
        }

        public Task<bool> DeleteAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }

        public Task<IEnumerable<Employee>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<Employee?> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<Employee?> UpdateAsync(Employee employee)
        {
            return _repository.UpdateAsync(employee);
        }
    }
}
