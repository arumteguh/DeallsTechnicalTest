using DeallsTechnicalTest.Data;
using DeallsTechnicalTest.Models;
using Microsoft.EntityFrameworkCore;

namespace DeallsTechnicalTest.Repositories
{
    public interface IAttendancePeriodRepository
    {
        Task<bool> PayrollNumberExistsAsync(string payrollNumber);
        Task<AttendancePeriod?> GetByPayrollNumberAsync(string payrollNumber);
        Task AddAsync(AttendancePeriod period);
        Task UpdateAsync(AttendancePeriod period);
    }
    public class AttendancePeriodRepository : IAttendancePeriodRepository
    {
        private readonly AppDbContext _context;

        public AttendancePeriodRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> PayrollNumberExistsAsync(string payrollNumber)
        {
            return await _context.attendancePeriods.AnyAsync(p => p.PayrollNumber == payrollNumber);
        }

        public async Task<AttendancePeriod?> GetByPayrollNumberAsync(string payrollNumber)
        {
            return await _context.attendancePeriods
                .FirstOrDefaultAsync(p => p.PayrollNumber == payrollNumber);
        }

        public async Task AddAsync(AttendancePeriod period)
        {
            _context.attendancePeriods.Add(period);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AttendancePeriod period)
        {
            _context.attendancePeriods.Update(period);
            await _context.SaveChangesAsync();
        }
    }
}
