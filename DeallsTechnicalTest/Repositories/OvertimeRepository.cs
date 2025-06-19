using Microsoft.EntityFrameworkCore;
using DeallsTechnicalTest.Models;
using DeallsTechnicalTest.Data;

namespace DeallsTechnicalTest.Repositories
{
    public interface IOvertimeRepository
    {
        Task AddOvertimeAsync(Overtime overtime);
    }
    public class OvertimeRepository : IOvertimeRepository
    {
        private readonly AppDbContext _context;

        public OvertimeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddOvertimeAsync(Overtime overtime)
        {
            _context.overtimes.Add(overtime);
            await _context.SaveChangesAsync();
        }


    }
}
