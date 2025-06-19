using DeallsTechnicalTest.Data;
using DeallsTechnicalTest.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace DeallsTechnicalTest.Services
{
    public interface IPayslipService
    {
        Task<PayslipDto> GeneratePayslipAsync(string employeeId, string payrollNumber);
        Task<PayslipSummaryDto> GenerateSummaryAsync(string payrollNumber);
    }
    public class PayslipService : IPayslipService
    {
        private readonly AppDbContext _context;

        public PayslipService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PayslipDto> GeneratePayslipAsync(string employeeId, string payrollNumber)
        {
            var employee = await _context.employees.FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
            var period = await _context.attendancePeriods.FirstOrDefaultAsync(p => p.PayrollNumber == payrollNumber);

            if (employee == null || period == null)
                throw new Exception("Employee or payroll not found");

            // Get all weekdays in range
            var workingDays = Enumerable.Range(0, (period.PeriodEnd - period.PeriodStart).Days + 1)
                .Select(offset => period.PeriodStart.AddDays(offset))
                .Where(date => date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                .ToList();

            double dailySalary = employee.Salary / (double)workingDays.Count;

            // Get attendance days
            var attendanceDays = await _context.attendances
                .Where(a => a.EmployeeId == employeeId &&
                            a.CheckIn.Date >= period.PeriodStart.Date &&
                            a.CheckIn.Date <= period.PeriodEnd.Date)
                .Select(a => a.CheckIn.Date)
                .Distinct()
                .ToListAsync();

            attendanceDays = attendanceDays
                .Where(d => d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday)
                .ToList();

            double attendanceEarnings = attendanceDays.Count * dailySalary;

            // Overtime
            var overtimes = await _context.overtimes
                .Where(o => o.EmployeeId == employeeId &&
                            o.OvertimeStart.Date >= period.PeriodStart.Date &&
                            o.OvertimeEnd.Date <= period.PeriodEnd.Date)
                .ToListAsync();

            double totalOvertimeHours = overtimes.Sum(o => o.OvertimeTotal);
            double overtimeEarnings = totalOvertimeHours * (dailySalary / 8.0); // assuming 8h workday

            // Reimbursements
            var reimbursements = await _context.reimbursements
                .Where(r => r.EmployeeId == employeeId &&
                            r.CreatedDate.Date >= period.PeriodStart.Date &&
                            r.CreatedDate.Date <= period.PeriodEnd.Date)
                .ToListAsync();

            double reimbursementTotal = reimbursements.Sum(r => r.ReimburseAmount);

            // Final total
            double takeHome = attendanceEarnings + overtimeEarnings + reimbursementTotal;

            return new PayslipDto
            {
                EmployeeId = employeeId,
                PayrollNumber = payrollNumber,
                DaysPresent = attendanceDays.Count,
                DailySalary = Math.Round(dailySalary, 2),
                AttendanceEarnings = Math.Round(attendanceEarnings, 2),
                OvertimeHours = totalOvertimeHours,
                OvertimeEarnings = Math.Round(overtimeEarnings, 2),
                ReimbursementTotal = Math.Round(reimbursementTotal, 2),
                TotalTakeHomePay = Math.Round(takeHome, 2),
                AttendanceDates = attendanceDays,
                Reimbursements = reimbursements
            };
        }

        public async Task<PayslipSummaryDto> GenerateSummaryAsync(string payrollNumber)
        {
            var period = await _context.attendancePeriods
                .FirstOrDefaultAsync(p => p.PayrollNumber == payrollNumber);

            if (period == null)
                throw new Exception("Invalid payroll number.");

            var workingDays = Enumerable.Range(0, (period.PeriodEnd - period.PeriodStart).Days + 1)
                .Select(offset => period.PeriodStart.AddDays(offset))
                .Where(date => date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                .ToList();

            var employees = await _context.employees.ToListAsync();

            var summaryItems = new List<PayslipSummaryItemDto>();
            double total = 0;

            foreach (var emp in employees)
            {
                double dailySalary = emp.Salary / (double)workingDays.Count;

                // Attendance
                var attendanceDays = await _context.attendances
                    .Where(a => a.EmployeeId == emp.EmployeeId &&
                                a.CheckIn.Date >= period.PeriodStart.Date &&
                                a.CheckIn.Date <= period.PeriodEnd.Date)
                    .Select(a => a.CheckIn.Date)
                    .Distinct()
                    .ToListAsync();

                attendanceDays = attendanceDays
                    .Where(d => d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday)
                    .ToList();

                double attendanceEarnings = attendanceDays.Count * dailySalary;

                // Overtime
                var overtimes = await _context.overtimes
                    .Where(o => o.EmployeeId == emp.EmployeeId &&
                                o.OvertimeStart.Date >= period.PeriodStart.Date &&
                                o.OvertimeEnd.Date <= period.PeriodEnd.Date)
                    .ToListAsync();

                double overtimeHours = overtimes.Sum(o => o.OvertimeTotal);
                double overtimeEarnings = overtimeHours * (dailySalary / 8.0);

                // Reimbursements
                var reimbursements = await _context.reimbursements
                    .Where(r => r.EmployeeId == emp.EmployeeId &&
                                r.CreatedDate.Date >= period.PeriodStart.Date &&
                                r.CreatedDate.Date <= period.PeriodEnd.Date)
                    .ToListAsync();

                double reimbursementTotal = reimbursements.Sum(r => r.ReimburseAmount);

                double takeHome = attendanceEarnings + overtimeEarnings + reimbursementTotal;

                summaryItems.Add(new PayslipSummaryItemDto
                {
                    EmployeeId = emp.EmployeeId,
                    Name = emp.Name,
                    TakeHomePay = Math.Round(takeHome, 2)
                });

                total += takeHome;
            }

            return new PayslipSummaryDto
            {
                PayrollNumber = payrollNumber,
                Employees = summaryItems,
                TotalTakeHomePay = Math.Round(total, 2)
            };
        }
    }
}
