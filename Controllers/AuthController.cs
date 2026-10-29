
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjetoDBZ.Data;
using ProjetoDBZ.Models.User;

namespace ProjetoDBZ.Controllers
{

    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly AppDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;


        public AuthController(IConfiguration configuration, IWebHostEnvironment environment, AppDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            _configuration = configuration;
            _environment = environment;
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterRequest request)
        {
            if (request.Password.Length < 12)
            {
                return BadRequest(new
                {
                    message = "A senha deve possuir pelo menos 12 caracteres."
                });
            }

            var username = request.Username.Trim();
            var email = request.Email.Trim();

            var alreadyExists = await _dbContext.Users.AnyAsync(u =>
                u.Username == username || u.Email == email);

            if (alreadyExists)
            {
                return Conflict(new
                {
                    message = "Não foi possível cadastrar o usuário."
                });
            }

            var role = await _dbContext.Roles.SingleOrDefaultAsync(r => r.Name == "User");

            if (role is null)
            {
                return StatusCode(500, new
                {
                    message = "Perfil padrão não configurado."
                });
            }

            var user = new User
            {
                Username = username,
                Email = email,
                RolesId = role.Id,
                IsActive = true
            };

            user.PasswordHash = _passwordHasher.HashPassword(
                user,
                request.Password);

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return StatusCode(201, new
            {
                message = "Usuário cadastrado com sucesso."
            });
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest request)
        {
            var username = request.Username.Trim();

            var user = await _dbContext.Users
                .Include(u => u.Roles)
                .SingleOrDefaultAsync(u => u.Username == username);

            if (user is null || !user.IsActive)
            {
                return Unauthorized(new
                {
                    message = "Usuário ou senha inválidos."
                });
            }

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new
                {
                    message = "Usuário ou senha inválidos."
                });
            }

            // Atualiza o hash caso o algoritmo ou seus parâmetros
            // precisem ser atualizados pelo PasswordHasher.
            if (result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                user.PasswordHash = _passwordHasher.HashPassword(
                    user,
                    request.Password);

                await _dbContext.SaveChangesAsync();
            }

            var jwtKey = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException(
                    "Chave JWT não configurada.");

            var issuer = _configuration["Jwt:Issuer"]
                ?? throw new InvalidOperationException(
                    "Issuer JWT não configurado.");

            var audience = _configuration["Jwt:Audience"]
                ?? throw new InvalidOperationException(
                    "Audience JWT não configurada.");

            var expirationMinutes =
                _configuration.GetValue<int>("Jwt:ExpirationMinutes", 15);

            if (expirationMinutes <= 0)
            {
                throw new InvalidOperationException(
                    "A duração do JWT deve ser positiva.");
            }

            var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

            var claims = new[]
            {
            new Claim(
                ClaimTypes.NameIdentifier,
                user.Id.ToString()),

            new Claim(ClaimTypes.Name, user.Username),

            new Claim(ClaimTypes.Role, user.Roles.Name),

            new Claim(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials);

            var accessToken = new JwtSecurityTokenHandler()
                .WriteToken(token);

            return Ok(new
            {
                accessToken,
                tokenType = "Bearer",
                expiresAt
            });
        }
    }

    public class RegisterRequest
    {
        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
