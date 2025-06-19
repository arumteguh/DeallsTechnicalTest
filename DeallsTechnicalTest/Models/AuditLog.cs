namespace DeallsTechnicalTest.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string RequestId { get; set; }
        public string EmployeeId { get; set; }  // Optional, can be null
        public string Action { get; set; }
        public string TableName { get; set; }
        public string RecordId { get; set; }    // E.g., "EMP1234"
        public string Changes { get; set; }     // JSON string of what changed
        public string IPAddress { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
