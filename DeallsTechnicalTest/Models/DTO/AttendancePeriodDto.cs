namespace DeallsTechnicalTest.Models.DTO
{
    public class AttendancePeriodDto
    {
        public string PayrollNumber { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }

        public AttendancePeriodDto()
        {
            PayrollNumber = string.Empty;
            PeriodStart = DateTime.MinValue;
            PeriodEnd = DateTime.MinValue;
        }
    }

    public class RunPayrollRequestDto
    {
        public string PayrollNumber { get; set; }

        public RunPayrollRequestDto()
        {
            PayrollNumber= string.Empty;
        }
    }
}
