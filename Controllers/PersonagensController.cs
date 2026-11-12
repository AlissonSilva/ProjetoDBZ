using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ProjetoDBZ.Data;
using ProjetoDBZ.DTO.Common;
using ProjetoDBZ.DTO.Personagens;
using ProjetoDBZ.Models;
using ProjetoDBZ.Services.Interfaces;

namespace ProjetoDBZ.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonagensController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly IPersonagemService _personagemService;
        public PersonagensController(AppDbContext context, IPersonagemService personagemService)
        {
            _appDbContext = context;
            _personagemService = personagemService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPersonagem([FromBody] PersonagemDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tipo = await _appDbContext.Tipos.FindAsync(dto.TipoId);
            if (tipo == null)
            {
                return BadRequest("Tipo inválido.");
            }

            var personagem = await _personagemService.AddAsync(dto);

            return Ok(personagem);
        }

        [HttpGet]
        public async Task<IActionResult> GetPersonagens(int page = 1, int pageSize = 10, string nome = null, int? tipoId = null)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Page e PageSize devem ser maiores que zero.");

            var personagens = await _personagemService.GetAllAsync(page, pageSize, nome, tipoId ?? 0);

            return Ok(personagens);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonagemById(int id)
        {
            var personagem = await _personagemService.GetByIdAsync(id);

            if (personagem == null)
            {
                return NotFound();
            }
            return Ok(personagem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePersonagem(int id, PersonagemDto updatedPersonagem)
        {
            var personagem = await _personagemService.UpdateAsync(id, updatedPersonagem);
            return Ok(personagem);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchPersonagem(int id, PersonagemDtoPatch updatedPersonagem)
        {
            var personagem = await _personagemService.PatchAsync(id, updatedPersonagem);

            return Ok(personagem);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonagem(int id)
        {
            Boolean personagem = await _personagemService.DeleteAsync(id);
            if (!personagem)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("{id}/imagem")]
        public async Task<IActionResult> UpdateImagem(int id, IFormFile imagem)
        {
            var personagem = await _appDbContext.Personagens.FindAsync(id);

            if (personagem == null)
                return NotFound("Personagem não encontrado.");

            if (imagem == null || imagem.Length == 0)
                return BadRequest("Nenhuma imagem foi enviada.");

            const long tamanhoMaximo = 5 * 1024 * 1024; // 5 MB

            if (imagem.Length > tamanhoMaximo)
                return BadRequest("A imagem deve ter no máximo 5 MB.");

            var tiposPermitidos = new[]
            {
                "image/jpeg",
                "image/png",
                "image/webp"
            };

            if (!tiposPermitidos.Contains(imagem.ContentType))
                return BadRequest("Formato de imagem não permitido.");

            using var memoryStream = new MemoryStream();

            await imagem.CopyToAsync(memoryStream);

            personagem.Imagem = memoryStream.ToArray();

            await _appDbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{id}/imagem")]
        public async Task<IActionResult> AddImagem(int id, IFormFile imagem)
        {
            var personagem = await _appDbContext.Personagens.FindAsync(id);

            if (personagem == null)
                return NotFound("Personagem não encontrado.");

            if (imagem == null || imagem.Length == 0)
                return BadRequest("Nenhuma imagem foi enviada.");

            const long tamanhoMaximo = 5 * 1024 * 1024; // 5 MB

            if (imagem.Length > tamanhoMaximo)
                return BadRequest("A imagem deve ter no máximo 5 MB.");

            var tiposPermitidos = new[]
            {
                "image/jpeg",
                "image/png",
                "image/webp"
            };

            if (!tiposPermitidos.Contains(imagem.ContentType))
                return BadRequest("Formato de imagem não permitido.");

            using var memoryStream = new MemoryStream();

            await imagem.CopyToAsync(memoryStream);

            personagem.Imagem = memoryStream.ToArray();

            await _appDbContext.SaveChangesAsync();

            return NoContent();
        }


    }
}
