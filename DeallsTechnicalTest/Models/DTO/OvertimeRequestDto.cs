namespace DeallsTechnicalTest.Models.DTO
{
    public class OvertimeRequestDto
    {
        public string EmployeeId { get; set; }
        public DateTime OvertimeStart { get; set; }
        public DateTime OvertimeEnd { get; set; }

        public OvertimeRequestDto()
        {
            EmployeeId = string.Empty;
            OvertimeStart = DateTime.MinValue;
            OvertimeEnd = DateTime.MinValue;
        }
    }
}
