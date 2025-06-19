using DeallsTechnicalTest.Data;
using DeallsTechnicalTest.Utils;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using DeallsTechnicalTest.Models.DTO;

namespace DeallsTechnicalTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtTokenGenerator _jwt;

        public AuthController(AppDbContext context, JwtTokenGenerator jwt)
        {
            _context = context;
            _jwt = jwt;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto request)
        {
            var user = _context.employees.FirstOrDefault(x =>
                x.UserName == request.UserName &&
                x.Password == request.Password); // Hash in production!

            if (user == null)
                return Unauthorized();

            var token = _jwt.GenerateToken(user);
            return Ok(new { token });
        }
    }
}
