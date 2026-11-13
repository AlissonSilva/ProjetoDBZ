using Microsoft.AspNetCore.Mvc;
using ProjetoDBZ.DTO.User;

namespace ProjetoDBZ.Services.Interfaces
{
    public interface IUserService
    {

        Task<IActionResult> Register([FromBody] RegisterRequestDto request);
        Task<IActionResult> Login([FromBody] LoginRequestDto request);
    }
}
