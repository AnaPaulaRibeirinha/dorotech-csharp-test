using DoroTech.Bookstore.Api.Data;
using DoroTech.Bookstore.Api.DTO;
using DoroTech.Bookstore.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoroTech.Bookstore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthController(AppDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Realiza login de administrador e retorna um token JWT Bearer.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (user == null)
                return Unauthorized("Usuário ou senha inválidos.");

            var validPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!validPassword)
                return Unauthorized("Usuário ou senha inválidos.");

            var token = _jwtService.GenerateToken(user);

            return Ok(new
            {
                access_token = token,
                token_type = "Bearer"
            });
        }
    }
}
