using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ProjetoDBZ.DTO.Tipos;
using ProjetoDBZ.Services.Interfaces;

namespace ProjetoDBZ.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoController : ControllerBase
    {
        private readonly ITipoService _tipoService;

        public TipoController(ITipoService tipoService)
        {
            _tipoService = tipoService;
        }

        [HttpPost]
        public async Task<IActionResult> AddTipo(CreateTipoDto dto)
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (usuarioId == null)
                return Unauthorized();

            var tipo = await _tipoService.AddTipo(dto, int.Parse(usuarioId));
            return Ok(tipo);
        }

        [HttpGet]
        public async Task<IActionResult> GetTipos()
        {
            var tipos = await _tipoService.GetTipos();
            return Ok(tipos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTipoById(int id)
        {
            var tipo = await _tipoService.GetTipoById(id);
            return Ok(tipo);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateTipo(int id, PatchTipoDto dto)
        {
            var result = await _tipoService.UpdateTipo(id, dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipo(int id)
        {
            return await _tipoService.DeleteTipo(id);
        }

    }
}
