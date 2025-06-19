namespace DeallsTechnicalTest.Models
{
    public class Reimbursement
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public double ReimburseAmount { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public Reimbursement() 
        { 
            Id = 0;
            EmployeeId = string.Empty;
            ReimburseAmount = 0;
            Description = string.Empty;
            CreatedDate = DateTime.MinValue;
            UpdatedDate = DateTime.MinValue;
            CreatedBy = string.Empty;
            UpdatedBy = string.Empty;
        }
    }    
}
