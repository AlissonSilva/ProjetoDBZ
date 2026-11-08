using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ProjetoDBZ.Data;
using ProjetoDBZ.DTO.Common;
using ProjetoDBZ.DTO.Personagens;
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
        public async Task<IActionResult> AddPersonagem([FromBody] CreatePersonagemDto dto)
        {
            var tipo = await _appDbContext.Tipos.FindAsync(dto.TipoId);
            if (tipo == null)
            {
                return BadRequest("Tipo de personagem inválido.");
            }

            var personagem = new Personagem
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                Tipo = tipo,
                Publicar = dto.Publicar,
                Ativo = dto.Ativo
            };

            _appDbContext.Personagens.Add(personagem);
            await _appDbContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetPersonagemById),
                new { id = personagem.Id },
                new PersonagemResponseDto
                {
                    Id = personagem.Id,
                    Nome = personagem.Nome,
                    Descricao = personagem.Descricao,
                    TipoId = personagem.Tipo.Id,
                    TipoNome = personagem.Tipo.Nome,
                    PossuiImagem = personagem.Imagem != null && personagem.Imagem.Length > 0,
                    Publicar = personagem.Publicar,
                    Ativo = personagem.Ativo
                }
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
        public async Task<IActionResult> GetPersonagens(int page = 1, int pageSize = 10, string? nome = null, int? tipoId = null)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Page e PageSize devem ser maiores que zero.");

            var query = _appDbContext.Personagens.AsNoTracking().Include(p => p.Tipo).AsQueryable();

            if (!string.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(p => p.Nome.Contains(nome));
            }

            // Filtro por tipo
            if (tipoId.HasValue)
            {
                query = query.Where(p => p.Tipo.Id == tipoId.Value);
            }

            var totalRecords = await query.CountAsync();

            var personagens = await query
            .OrderBy(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new PersonagemResponseDto
            {
                Id = p.Id,
                Nome = p.Nome,
                Descricao = p.Descricao,
                TipoId = p.Tipo.Id,
                TipoNome = p.Tipo.Nome,
                PossuiImagem = p.Imagem != null && p.Imagem.Length > 0,
                Publicar = p.Publicar,
                Ativo = p.Ativo
            })
            .ToListAsync();

            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var response = new PagedResponseDto<PersonagemResponseDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = personagens
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonagemById(int id)
        {
            var personagem = await _appDbContext.Personagens
                .AsNoTracking()
                .Include(p => p.Tipo)
                .Select(p => new PersonagemResponseDto
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Descricao = p.Descricao,
                    TipoId = p.Tipo.Id,
                    TipoNome = p.Tipo.Nome,
                    PossuiImagem = p.Imagem != null && p.Imagem.Length > 0,
                    Publicar = p.Publicar,
                    Ativo = p.Ativo
                })
                .FirstOrDefaultAsync(p => p.Id == id);
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
            return Ok(new PersonagemResponseDto
            {
                Id = personagem.Id,
                Nome = personagem.Nome,
                Descricao = personagem.Descricao,
                TipoId = personagem.Tipo.Id,
                TipoNome = personagem.Tipo.Nome,
                PossuiImagem = personagem.Imagem != null && personagem.Imagem.Length > 0,
                Publicar = personagem.Publicar,
                Ativo = personagem.Ativo
            });
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
            if (updatedPersonagem.TipoId != 0)
            {
                personagem.TipoId = updatedPersonagem.TipoId;
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
            return Ok(new PersonagemResponseDto
            {
                Id = personagem.Id,
                Nome = personagem.Nome,
                Descricao = personagem.Descricao,
                TipoId = personagem.TipoId,
                PossuiImagem = personagem.Imagem != null && personagem.Imagem.Length > 0,
                Publicar = personagem.Publicar,
                Ativo = personagem.Ativo
            });
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
