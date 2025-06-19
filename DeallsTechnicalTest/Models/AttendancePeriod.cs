namespace DeallsTechnicalTest.Models
{
    public class AttendancePeriod
    {
        public int Id { get; set; }
        public string PayrollNumber { get; set; }  // e.g., "PR-2024-06"
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }

        public bool IsPayrollRun { get; set; } = false;
        public DateTime? PayrollRunDate { get; set; }
        public string? PayrollRunBy { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public AttendancePeriod()
        {
            PayrollNumber = string.Empty;
            CreatedBy = UpdatedBy = string.Empty;
            CreatedDate = UpdatedDate = DateTime.Now;
        }
    }
}
