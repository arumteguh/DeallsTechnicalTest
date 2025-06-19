using Bogus;
using DeallsTechnicalTest.Models;
using Microsoft.EntityFrameworkCore;

namespace DeallsTechnicalTest.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Attendance> attendances => Set<Attendance>();
        public DbSet<Employee> employees => Set<Employee>();
        public DbSet<Overtime> overtimes => Set<Overtime>();
        public DbSet<Reimbursement> reimbursements => Set<Reimbursement>();

        public DbSet<AttendancePeriod> attendancePeriods => Set<AttendancePeriod>();

        public DbSet<AuditLog> auditLogs => Set<AuditLog>();

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Overtime>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Reimbursement>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<AttendancePeriod>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasKey(e => e.Id);
            });


            //Seeding
            var employees = SeedEmployee();
            modelBuilder.Entity<Employee>().HasData(employees);
        }

        public List<Employee> SeedEmployee()
        {
            var employees = new List<Employee>
            {
                new Employee
                {
                    Id = 1,
                    Name = "Admin User",
                    Email = "admin@yourcompany.com,",
                    Salary = 0,
                    EmployeeId = "000000",
                    UserName = "admin",
                    Password = "admin123", // Plaintext for seed only!
                    IsAdmin = true
                }
            };

            //var faker = new Faker<Employee>()
            //                    .RuleFor(e => e.Id, f => f.IndexFaker + 2) // Start from ID 2
            //                    .RuleFor(e => e.Name, f => f.Name.FullName())
            //                    .RuleFor(e => e.Email, (f, e) => f.Internet.Email(e.Name))
            //                    .RuleFor(e => e.Salary, f => 10_000_000 + (f.IndexFaker * 100_000))
            //                    .RuleFor(e => e.EmployeeId, f => $"EMP{1000 + f.IndexFaker}")
            //                    .RuleFor(e => e.UserName, f => f.Internet.UserName())
            //                    .RuleFor(e => e.Password, f => "default123") // In real use, hash this
            //                    .RuleFor(e => e.IsAdmin, f => false);

            //employees.AddRange(faker.Generate(100));

            return employees;
        }


    }
}
