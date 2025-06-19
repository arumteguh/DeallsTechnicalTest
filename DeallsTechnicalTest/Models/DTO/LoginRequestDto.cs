namespace DeallsTechnicalTest.Models.DTO
{
    public class LoginRequestDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public LoginRequestDto()
        {
            UserName = string.Empty;
            Password = string.Empty;
        }
    }
}
