using System.Numerics;

namespace DeallsTechnicalTest.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Salary { get; set; } 
        public string EmployeeId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }        

        public Employee()
        {
            Id = 0;
            Name = String.Empty;
            Email = String.Empty;
            EmployeeId = String.Empty;
            UserName = String.Empty;
            Password = String.Empty;
            IsAdmin = false;
        }
    }
}
