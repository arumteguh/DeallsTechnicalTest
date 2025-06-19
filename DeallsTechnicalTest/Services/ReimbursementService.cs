using DeallsTechnicalTest.Models;
using DeallsTechnicalTest.Models.DTO;
using DeallsTechnicalTest.Repositories;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace DeallsTechnicalTest.Services
{
    public interface IReimbursementService
    {
        Task SubmitReimbursementAsync(string employeeId, ReimbursementRequestDto dto, string submittedBy);
    }
    public class ReimbursementService : IReimbursementService
    {
        private readonly IReimbursementRepository _repository;
        private readonly IAuditLogService _auditLogService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReimbursementService(IReimbursementRepository repository, IAuditLogService auditLogService, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _auditLogService = auditLogService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SubmitReimbursementAsync(string employeeId, ReimbursementRequestDto dto, string submittedBy)
        {
            if (dto.ReimburseAmount <= 0)
                throw new ArgumentException("Reimbursement amount must be greater than zero.");

            var reimbursement = new Reimbursement
            {
                EmployeeId = employeeId,
                ReimburseAmount = dto.ReimburseAmount,
                Description = dto.Description?.Trim() ?? string.Empty,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedBy = submittedBy,
                UpdatedBy = submittedBy
            };

            await _repository.AddReimbursementAsync(reimbursement);

            var changes = JsonSerializer.Serialize(dto);

            var requestId = _httpContextAccessor.HttpContext?.Items["RequestId"]?.ToString();

            await _auditLogService.LogAsync(
                requestId,
                "Create",
                "Reimbursement",
                reimbursement.Id.ToString(),
                changes,
                employeeId
            );
        }
    }
}
