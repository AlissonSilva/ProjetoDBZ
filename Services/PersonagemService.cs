using Microsoft.EntityFrameworkCore;
using ProjetoDBZ.Data;
using ProjetoDBZ.DTO.Common;
using ProjetoDBZ.DTO.Personagens;
using ProjetoDBZ.Models;
using ProjetoDBZ.Services.Interfaces;

namespace ProjetoDBZ.Services
{
    public class PersonagemService : IPersonagemService
    {
        private readonly AppDbContext _context;

        public PersonagemService(AppDbContext context)
        {
            _context = context;
        }
        public Task<PersonagemResponseDto> AddAsync(PersonagemDto dto, int usuarioId)
        {
            var tipo = _context.Tipos.FirstOrDefault(t => t.Id == dto.TipoId);
            var usuario = _context.Users.FirstOrDefault(u => u.Id == usuarioId);
            if (tipo == null)
            {
                throw new ArgumentException("Tipo inválido.");
            }
            if (usuario == null)
            {
                throw new ArgumentException("Usuário inválido.");
            }

            var personagem = new Personagem
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                Tipo = tipo,
                Publicar = dto.Publicar,
                Ativo = dto.Ativo,
                Imagem = dto.Imagem,
                DataCriacao = dto.DataCriacao,
                DataAtualizacao = dto.DataAtualizacao,
                Usuario = usuario
            };

            _context.Personagens.Add(personagem);
            _context.SaveChanges();
            return Task.FromResult(new PersonagemResponseDto
            {
                Id = personagem.Id,
                Nome = personagem.Nome,
                Descricao = personagem.Descricao,
                TipoId = personagem.Tipo.Id,
                Tipo = personagem.Tipo,
                Publicar = personagem.Publicar,
                Ativo = personagem.Ativo,
                PossuiImagem = personagem.Imagem != null && personagem.Imagem.Length > 0,
                DataCriacao = personagem.DataCriacao,
                DataAtualizacao = personagem.DataAtualizacao,
                UsuarioId = personagem.UsuarioId,
                Usuario = personagem.Usuario
            });

        }

        public Task<bool> DeleteAsync(int id)
        {
            var personagem = _context.Personagens.Find(id);
            if (personagem == null)
            {
                return Task.FromResult(false);
            }

            _context.Personagens.Remove(personagem);
            _context.SaveChanges();
            return Task.FromResult(true);
        }


        public Task<bool> LikesAsync(int id_personagem, int id_usuario)
        {
            PersonagemLike p = new PersonagemLike();

            int query = _context.PersonagemLikes.AsNoTracking().Where(p => p.UsuarioId == id_usuario && p.PersonagemId == id_personagem).Count();
            if (query > 0)
                return Task.FromResult(false);

            p.PersonagemId = id_personagem;
            p.UsuarioId = id_usuario;
            p.DataCriacao = DateTime.Now;

            _context.PersonagemLikes.Add(p);
            _context.SaveChanges();
            return Task.FromResult(true);
        }

        public Task<bool> DisLikesAsync(int id_personagem, int id_usuario)
        {
            PersonagemLike pl = _context.PersonagemLikes.AsNoTracking().Where(p => p.UsuarioId == id_usuario && p.PersonagemId == id_personagem).FirstAsync().Result;
            _context.Remove(pl);
            _context.SaveChanges();
            return Task.FromResult(true);
        }

        public async Task<PagedResponseDto<PersonagemResponseDto>> GetAllAsync(int page, int pageSize, string nome, int tipoId)
        {
            var query = _context.Personagens
            .AsNoTracking()
            .AsQueryable();

            if (!string.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(p => p.Nome.Contains(nome));
            }

            if (tipoId > 0)
            {
                query = query.Where(p => p.TipoId == tipoId);
            }

            var totalRecords = await query.CountAsync();

            var personagens = await query
                            .OrderBy(p => p.Nome)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .Select(p => new PersonagemResponseDto
                            {
                                Id = p.Id,
                                Nome = p.Nome,
                                Descricao = p.Descricao,
                                PossuiImagem = p.Imagem != null && p.Imagem.Length > 0,
                                Publicar = p.Publicar,
                                Ativo = p.Ativo,
                                TipoId = p.TipoId,
                                Tipo = p.Tipo,
                                DataCriacao = p.DataCriacao,
                                DataAtualizacao = p.DataAtualizacao,
                                UsuarioId = p.UsuarioId,
                                Usuario = p.Usuario
                            })
                            .ToListAsync();

            return new PagedResponseDto<PersonagemResponseDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(
                    (double)totalRecords / pageSize),
                Data = personagens
            };
        }

        public async Task<PersonagemResponseDto> GetByIdAsync(int id)
        {
            return await _context.Personagens
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new PersonagemResponseDto
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Descricao = p.Descricao,
                    PossuiImagem = p.Imagem != null &&
                                   p.Imagem.Length > 0,
                    Publicar = p.Publicar,
                    Ativo = p.Ativo,
                    TipoId = p.TipoId,
                    Tipo = p.Tipo,
                    DataCriacao = p.DataCriacao,
                    DataAtualizacao = p.DataAtualizacao,
                    UsuarioId = p.UsuarioId,
                    Usuario = p.Usuario
                })
                .FirstOrDefaultAsync();
        }

        public Task<PersonagemResponseDto> PatchAsync(int id, PersonagemDtoPatch dto)
        {
            var personagem = _context.Personagens.Find(id);
            if (personagem == null)
            {
                throw new ArgumentException("Personagem não encontrado.");
            }
            if (!string.IsNullOrWhiteSpace(dto.Nome))
            {
                personagem.Nome = dto.Nome;
            }
            if (!string.IsNullOrWhiteSpace(dto.Descricao))
            {
                personagem.Descricao = dto.Descricao;
            }
            if (dto.TipoId > 0)
            {
                var tipo = _context.Tipos.FirstOrDefault(t => t.Id == dto.TipoId);
                if (tipo == null)
                {
                    throw new ArgumentException("Tipo inválido.");
                }
                personagem.Tipo = tipo;
            }
            if (dto.Publicar != personagem.Publicar)
            {
                personagem.Publicar = dto.Publicar;
            }
            if (dto.Ativo != personagem.Ativo)
            {
                personagem.Ativo = dto.Ativo;
            }

            _context.SaveChanges();
            return Task.FromResult(new PersonagemResponseDto
            {
                Id = personagem.Id,
                Nome = personagem.Nome,
                Descricao = personagem.Descricao,
                PossuiImagem = personagem.Imagem != null && personagem.Imagem.Length > 0,
                Publicar = personagem.Publicar,
                Ativo = personagem.Ativo,
                TipoId = personagem.TipoId,
                Tipo = personagem.Tipo,
                DataCriacao = personagem.DataCriacao,
                DataAtualizacao = personagem.DataAtualizacao,
                UsuarioId = personagem.UsuarioId,
                Usuario = personagem.Usuario
            });
        }

        public Task<PersonagemResponseDto> UpdateAsync(int id, PersonagemDto dto)
        {
            var personagem = _context.Personagens.Find(id);
            if (personagem == null)
            {
                throw new ArgumentException("Personagem não encontrado.");
            }

            var tipo = _context.Tipos.FirstOrDefault(t => t.Id == dto.TipoId);
            if (tipo == null)
            {
                throw new ArgumentException("Tipo inválido.");
            }

            personagem.Nome = dto.Nome;
            personagem.Descricao = dto.Descricao;
            personagem.Tipo = tipo;
            personagem.Publicar = dto.Publicar;
            personagem.Ativo = dto.Ativo;
            personagem.Imagem = dto.Imagem;
            personagem.DataAtualizacao = DateTime.Now;


            _context.SaveChanges();

            return Task.FromResult(new PersonagemResponseDto
            {
                Id = personagem.Id,
                Nome = personagem.Nome,
                Descricao = personagem.Descricao,
                PossuiImagem = personagem.Imagem != null && personagem.Imagem.Length > 0,
                Publicar = personagem.Publicar,
                Ativo = personagem.Ativo,
                TipoId = personagem.TipoId,
                Tipo = personagem.Tipo,
                DataCriacao = personagem.DataCriacao,
                DataAtualizacao = personagem.DataAtualizacao,
                UsuarioId = personagem.UsuarioId,
                Usuario = personagem.Usuario
            });
        }

        public Task<bool> UpdateImageAsync(int id, IFormFile imagem)
        {
            var personagem = _context.Personagens.Find(id);
            if (personagem == null)
            {
                return Task.FromResult(false);
            }

            using (var memoryStream = new MemoryStream())
            {
                imagem.CopyTo(memoryStream);
                personagem.Imagem = memoryStream.ToArray();
            }

            _context.SaveChanges();
            return Task.FromResult(true);
        }
    }
}
