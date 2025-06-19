namespace DeallsTechnicalTest.Models.DTO
{
    public class PayslipSummaryDto
    {
        public string PayrollNumber { get; set; }
        public List<PayslipSummaryItemDto> Employees { get; set; }
        public double TotalTakeHomePay { get; set; }
    }
}
