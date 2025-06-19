using Microsoft.EntityFrameworkCore;
using DeallsTechnicalTest.Models;
using DeallsTechnicalTest.Data;

namespace DeallsTechnicalTest.Repositories
{
    public interface IReimbursementRepository
    {
        Task AddReimbursementAsync(Reimbursement reimbursement);
    }
    public class ReimbursementRepository : IReimbursementRepository
    {
        private readonly AppDbContext _context;

        public ReimbursementRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddReimbursementAsync(Reimbursement reimbursement)
        {
            _context.reimbursements.Add(reimbursement);
            await _context.SaveChangesAsync();
            //return reimbursement.Id;
        }
    }
}
