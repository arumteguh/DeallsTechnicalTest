using Microsoft.EntityFrameworkCore;
using DeallsTechnicalTest.Models;
using DeallsTechnicalTest.Data;

namespace DeallsTechnicalTest.Repositories
{
    public interface IAttendanceRepository
    {
        Task<Attendance?> GetAttendanceByDateAsync(string employeeId, DateTime date);
        Task AddAttendanceAsync(Attendance attendance);
    }
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly AppDbContext _context;

        public AttendanceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Attendance?> GetAttendanceByDateAsync(string employeeId, DateTime date)
        {
            return await _context.attendances
                .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.CheckIn.Date == date.Date);
        }

        public async Task AddAttendanceAsync(Attendance attendance)
        {
            _context.attendances.Add(attendance);
            await _context.SaveChangesAsync();
        }
    }
}
