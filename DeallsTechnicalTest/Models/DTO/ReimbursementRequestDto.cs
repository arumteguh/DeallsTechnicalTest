namespace DeallsTechnicalTest.Models.DTO
{
    public class ReimbursementRequestDto
    {
        public double ReimburseAmount { get; set; }
        public string Description { get; set; }

        public ReimbursementRequestDto()
        {
            ReimburseAmount = 0;
            Description = string.Empty;
        }
    }    
}
