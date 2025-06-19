namespace DeallsTechnicalTest.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public DateTime CheckIn {  get; set; }
        public DateTime CheckOut { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        
        public Attendance()
        {
            Id = 0;
            EmployeeId = string.Empty;
            CheckIn = DateTime.MinValue;
            CheckOut = DateTime.MinValue;
            CreatedDate = DateTime.MinValue;
            UpdatedDate = DateTime.MinValue;
            CreatedBy = string.Empty;
            UpdatedBy = string.Empty;
        }
    }
}
