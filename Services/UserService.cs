using ProjetoDBZ.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ProjetoDBZ.DTO.User;
using ProjetoDBZ.Data;
using Microsoft.AspNetCore.Identity;
using ProjetoDBZ.Models.User;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace ProjetoDBZ.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public UserService( AppDbContext dbContext, IPasswordHasher<User> passwordHasher, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
            _environment = environment;
        }

        public Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var username = request.Username.Trim();

            var user = _dbContext.Users
                .Include(u => u.Roles)
                .SingleOrDefault(u => u.Username == username);

            if (user is null || !user.IsActive)
            {
                return Task.FromResult<IActionResult>(new UnauthorizedObjectResult(new
                {
                    message = "Usuário ou senha inválidos."
                }));
            }

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return Task.FromResult<IActionResult>(new UnauthorizedObjectResult(new
                {
                    message = "Usuário ou senha inválidos."
                }));
            }

            if(result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
                _dbContext.SaveChanges();
            }

            var jwtKey = _configuration["Jwt:Key"]?? throw new InvalidOperationException("JWT key is not configured.");

            var issuer = _configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT issuer is not configured.");

            var audience = _configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT audience is not configured.");

            var expirationMinutes = _configuration.GetValue<int>("Jwt:ExpirationMinutes", 15);

            if (expirationMinutes <= 0)
            {
                throw new InvalidOperationException("JWT expiration time must be greater than zero.");
            }

            var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(ClaimTypes.Role, user.Roles.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            
            return Task.FromResult<IActionResult>(new OkObjectResult(new
            {
                message = "Login bem-sucedido.",
                accessToken = $"Bearer {accessToken}",
                expiresAt = expiresAt,
                user = new
                {
                    id = user.Id,
                    name = user.Name,
                    username = user.Username,
                    email = user.Email,
                    role = user.Roles.Name
                }
            }));
        }

        public Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            if (request.Password.Length < 12)
            {
                return Task.FromResult<IActionResult>(new BadRequestObjectResult(new
                {
                    message = "A senha deve ter pelo menos 12 caracteres."
                }));
            }

            var username = request.Username.Trim();
            var email = request.Email.Trim();

            var existingUser = _dbContext.Users.FirstOrDefault(u => u.Username == username || u.Email == email);
            if (existingUser != null)
            {
                return Task.FromResult<IActionResult>(new BadRequestObjectResult(new
                {
                    message = "Nome de usuário ou e-mail já está em uso."
                }));
            }
            
            var role = _dbContext.Roles.SingleOrDefaultAsync(r => r.Name == "User");

            if (role is null)
            {
                return Task.FromResult<IActionResult>(new BadRequestObjectResult(new
                {
                    message = "Perfil padrão não configurado."
                }));
            }

            var user = new User
            {
                Name = request.Name.Trim(),
                Username = username,
                Email = email,
                RolesId = role.Id,
                IsActive = true
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return Task.FromResult<IActionResult>(new CreatedResult(string.Empty, new
            {
                message = "Usuário registrado com sucesso."
            }));

        }
    }
}
