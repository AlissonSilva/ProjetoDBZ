using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using ProjetoDBZ.DTO.Comentarios;
using ProjetoDBZ.Services.Interfaces;

namespace ProjetoDBZ.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComentarioController : Controller
    {
        private readonly IComentarioService _comentarioService;

        public ComentarioController(IComentarioService comentarioService)
        {
            _comentarioService = comentarioService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ComentarioDto dto)
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (usuarioId == null)
                return Unauthorized();

            var comentario = await _comentarioService.Add(dto, int.Parse(usuarioId));
            return Ok(comentario);
        }

        [HttpGet("personagemId")]
        public async Task<IActionResult> Get(int personagemId)
        {
            var comentario = await _comentarioService.Get(personagemId);

            return Ok(comentario);

        }

        [HttpGet]
        public async Task<IActionResult> GetByUsuario()
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (usuarioId == null)
                return Unauthorized();

            var comentarioByUser = await _comentarioService.GetByUsuario(int.Parse(usuarioId));

            return Ok(comentarioByUser);

        }

        [HttpPatch("id")]
        public async Task<IActionResult> Update([FromBody] PatchComentarioDto dto, int id)
        {
            var result = _comentarioService.Update(dto, id);
            return Ok(result);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _comentarioService.Delete(id);
        }

        [HttpPost("{id}/likes")]
        public async Task<IActionResult> Likes(int id)
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (usuarioId == null)
                return Unauthorized();

            bool cl = await _comentarioService.LikesAsync(id, int.Parse(usuarioId));

            if (cl)
            {
                return Ok();
            }
            else
            {
                return NoContent();
            }

        }

        [HttpPost("{id}/dislikes")]
        public async Task<IActionResult> DisLikes(int id)
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (usuarioId == null)
                return Unauthorized();

            bool pl = await _comentarioService.DisLikesAsync(id, int.Parse(usuarioId));

            if (pl)
            {
                return Ok();
            }
            else
            {
                return NoContent();
            }
        }

    }
}
