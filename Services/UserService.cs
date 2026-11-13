using ProjetoDBZ.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ProjetoDBZ.DTO.User;
using ProjetoDBZ.Data;
using Microsoft.AspNetCore.Identity;
using ProjetoDBZ.Models.User;
using Microsoft.EntityFrameworkCore;


namespace ProjetoDBZ.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService( AppDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
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

            return Task.FromResult<IActionResult>(new OkObjectResult(new
            {
                message = "Login bem-sucedido."
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

            var user = new User
            {
                Username = username,
                Email = email,
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
