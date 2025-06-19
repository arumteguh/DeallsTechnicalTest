namespace DeallsTechnicalTest.Models
{
    public class Overtime
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public DateTime OvertimeStart { get; set; }
        public DateTime OvertimeEnd { get; set; }
        public double OvertimeTotal { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public Overtime()
        {
            Id = 0;
            EmployeeId = string.Empty;
            OvertimeStart = DateTime.MinValue;
            OvertimeEnd = DateTime.MinValue;
            OvertimeTotal = 0;
            CreatedDate = DateTime.MinValue;
            UpdatedDate = DateTime.MinValue;
            CreatedBy = string.Empty;
            UpdatedBy = string.Empty;
        }
    }
}
