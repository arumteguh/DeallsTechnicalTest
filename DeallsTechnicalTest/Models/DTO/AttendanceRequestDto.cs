namespace DeallsTechnicalTest.Models.DTO
{
    public class AttendanceRequestDto
    {
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }

        public AttendanceRequestDto()
        {
            CheckIn = DateTime.MinValue;
            CheckOut = DateTime.MinValue;
        }
    }
}
