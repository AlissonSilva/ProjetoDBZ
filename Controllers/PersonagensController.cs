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
        public async Task<IActionResult> AddPersonagem(Personagem personagem)
        {
            _appDbContext.Personagens.Add(personagem);
            await _appDbContext.SaveChangesAsync();
            return Ok(personagem);
        }

        [HttpGet]
        public async Task<IActionResult> GetPersonagens()
        {
            var personagens = await _appDbContext.Personagens.Join(
                _appDbContext.Tipos,
                p => p.Tipo.Id,
                t => t.Id,
                (p, t) => new Personagem
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Tipo = t
                }
            ).ToListAsync();
            return Ok(personagens);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonagemById(int id)
        {
            var personagem = await _appDbContext.Personagens.Join(
                _appDbContext.Tipos,
                p => p.Tipo.Id,
                t => t.Id,
                (p, t) => new Personagem
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Tipo = t
                }
            ).FirstOrDefaultAsync(p => p.Id == id);
            if (personagem == null)
            {
                return NotFound();
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