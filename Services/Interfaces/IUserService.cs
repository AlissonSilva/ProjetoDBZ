using Microsoft.AspNetCore.Mvc;

namespace ProjetoDBZ.Services.Interfaces
{
    public interface IUserService
    {

        Task<IActionResult> Register([FromBody] RegisterRequestService request);
        Task<IActionResult> Login([FromBody] LoginRequestService request);
    }
}
