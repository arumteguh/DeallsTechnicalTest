using Microsoft.EntityFrameworkCore;
using DeallsTechnicalTest.Models;
using DeallsTechnicalTest.Data;

namespace DeallsTechnicalTest.Repositories
{

    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task<Employee> AddAsync(Employee employee);
        Task<Employee?> UpdateAsync(Employee employee);
        Task<bool> DeleteAsync(int id);
    }

    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> AddAsync(Employee employee)
        {
            _context.employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await _context.employees.FindAsync(id);
            if (employee == null) return false;

            _context.employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            var employees = await _context.employees.ToListAsync();

            return employees;
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            var employee = await _context.employees.FindAsync(id);

            return employee;
        }

        public async Task<Employee?> UpdateAsync(Employee employee)
        {
            var existing = await _context.employees.FindAsync(employee.Id);
            if (existing == null) return null;

            existing.Name = employee.Name;
            existing.Email = employee.Email;
            existing.Salary = employee.Salary;

            await _context.SaveChangesAsync();
            return existing;
        }
    }
}
