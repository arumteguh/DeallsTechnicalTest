namespace DeallsTechnicalTest.Models.DTO
{
    public class PayslipDto
    {
        public string EmployeeId { get; set; }
        public string PayrollNumber { get; set; }
        public int DaysPresent { get; set; }
        public double DailySalary { get; set; }
        public double AttendanceEarnings { get; set; }
        public double OvertimeHours { get; set; }
        public double OvertimeEarnings { get; set; }
        public double ReimbursementTotal { get; set; }
        public double TotalTakeHomePay { get; set; }
        public List<DateTime> AttendanceDates { get; set; }
        public List<Reimbursement> Reimbursements { get; set; }
    }
}
