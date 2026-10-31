using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoDBZ.Data;
using ProjetoDBZ.Models;

namespace ProjetoDBZ.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonagensController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public PersonagensController(AppDbContext context)
        {
            _appDbContext = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddPersonagem([FromBody] Personagem personagem)
        {
            _appDbContext.Personagens.Add(personagem);
            await _appDbContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetPersonagemById),
                new { id = personagem.Id },
                personagem
            );
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

        [HttpGet]
        public async Task<IActionResult> GetPersonagens()
        {
            var personagens = await _appDbContext.Personagens.AsNoTracking().Include(p => p.Tipo).ToListAsync();
            return Ok(personagens);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonagemById(int id)
        {
            var personagem = await _appDbContext.Personagens.AsNoTracking().Include(p => p.Tipo).FirstOrDefaultAsync(p => p.Id == id);
            if (personagem == null)
            {
                return NotFound("Personagem não encontrado.");
            }
            return Ok(personagem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePersonagem(int id, Personagem updatedPersonagem)
        {
            var personagem = await _appDbContext.Personagens.FindAsync(id);
            if (personagem == null)
            {
                return NotFound();
            }

            personagem.Nome = updatedPersonagem.Nome;
            personagem.Tipo = updatedPersonagem.Tipo;
            personagem.Descricao = updatedPersonagem.Descricao;
            personagem.Imagem = updatedPersonagem.Imagem;

            await _appDbContext.SaveChangesAsync();
            return Ok(personagem);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchPersonagem(int id, Personagem updatedPersonagem)
        {
            var personagem = await _appDbContext.Personagens.FindAsync(id);
            if (personagem == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(updatedPersonagem.Nome))
            {
                personagem.Nome = updatedPersonagem.Nome;
            }
            if (updatedPersonagem.Tipo != null)
            {
                personagem.Tipo = updatedPersonagem.Tipo;
            }
            if (!string.IsNullOrEmpty(updatedPersonagem.Descricao))
            {
                personagem.Descricao = updatedPersonagem.Descricao;
            }
            if (updatedPersonagem.Imagem != null && updatedPersonagem.Imagem.Length > 0)
            {
                personagem.Imagem = updatedPersonagem.Imagem;
            }


            await _appDbContext.SaveChangesAsync();
            return Ok(personagem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonagem(int id)
        {
            var personagem = await _appDbContext.Personagens.FindAsync(id);
            if (personagem == null)
            {
                return NotFound();
            }

            _appDbContext.Personagens.Remove(personagem);
            await _appDbContext.SaveChangesAsync();
            return NoContent();
        }





    }
}
