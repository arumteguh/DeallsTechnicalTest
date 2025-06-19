
using Bogus;
using DeallsTechnicalTest.Data;
using DeallsTechnicalTest.Middleware;
using DeallsTechnicalTest.Models;
using DeallsTechnicalTest.Repositories;
using DeallsTechnicalTest.Services;
using DeallsTechnicalTest.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace DeallsTechnicalTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();

            builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
            builder.Services.AddScoped<IAttendanceService, AttendanceService>();

            builder.Services.AddScoped<IOvertimeRepository, OvertimeRepository>();
            builder.Services.AddScoped<IOvertimeService, OvertimeService>();

            builder.Services.AddScoped<IReimbursementRepository, ReimbursementRepository>();
            builder.Services.AddScoped<IReimbursementService, ReimbursementService>();

            builder.Services.AddScoped<IAttendancePeriodRepository, AttendancePeriodRepository>();
            builder.Services.AddScoped<IAttendancePeriodService, AttendancePeriodService>();

            builder.Services.AddScoped<IPayslipService, PayslipService>();

            builder.Services.AddScoped<JwtTokenGenerator>();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IAuditLogService, AuditLogService>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtSettings = builder.Configuration.GetSection("JwtSettings");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
                };
            });

            builder.Services.AddAuthorization();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(options =>
            {
                // Enable XML comments if you want Swagger doc descriptions (optional)
                // var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                // options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                // Add JWT Authentication to Swagger
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input.\n\nExample: **Bearer eyJhbGciOi...**"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            var app = builder.Build();
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                if (!db.employees.Any(e => e.Id > 1)) // only if fake data not seeded
                {
                    var faker = new Faker<Employee>()
                        .RuleFor(e => e.Id, f => f.IndexFaker + 2)
                        .RuleFor(e => e.Name, f => f.Name.FullName())
                        .RuleFor(e => e.Email, (f, e) => f.Internet.Email(e.Name))
                        .RuleFor(e => e.Salary, f => f.Random.Number(5_000_000, 20_000_000))
                        .RuleFor(e => e.EmployeeId, f => $"EMP{f.Random.Number(1000, 9999)}")
                        .RuleFor(e => e.UserName, f => f.Internet.UserName())
                        .RuleFor(e => e.Password, f => "default123")
                        .RuleFor(e => e.IsAdmin, f => false);

                    var employees = faker.Generate(100);
                    // Optional: set explicit IDs starting from 2
                    //for (int i = 0; i < employees.Count; i++)
                    //{
                    //    employees[i].Id = i + 2;
                    //}

                    db.employees.AddRange(employees);
                    db.SaveChanges();
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<RequestIdMiddleware>();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
