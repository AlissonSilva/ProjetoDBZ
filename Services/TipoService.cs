using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoDBZ.Data;
using ProjetoDBZ.DTO.Tipos;
using ProjetoDBZ.Models;
using ProjetoDBZ.Services.Interfaces;

namespace ProjetoDBZ.Services
{
    public class TipoService : ITipoService
    {

        private readonly AppDbContext _appDbContext;

        public TipoService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<TipoResponseDto> AddTipo(CreateTipoDto dto, int usuarioId)
        {
            var usuario = _appDbContext.Users.FirstOrDefault(u => u.Id == usuarioId);

            if (usuario == null)
                throw new ArgumentException("Usuário inválido.");

            Tipo tipo = new Tipo
            {
                Nome = dto.Nome,
                Ativo = dto.Ativo,
                Publicar = dto.Publicar,
                UsuarioId = usuarioId,
                DataCriacao = DateTime.UtcNow,
                DataAtualizacao = DateTime.UtcNow
            };

            _appDbContext.Tipos.Add(tipo);
            _appDbContext.SaveChanges();

            return new TipoResponseDto
            {
                Id = tipo.Id,
                Nome = tipo.Nome,
                Ativo = tipo.Ativo ?? false,
                Publicar = tipo.Publicar ?? false,
                UsuarioId = tipo.UsuarioId,
                Usuario = tipo.Usuario,
                DataCriacao = tipo.DataCriacao,
                DataAtualizacao = tipo.DataAtualizacao
            };
        }

        public async Task<TipoResponseDto> GetTipoById(int id)
        {
            Tipo tipo = await _appDbContext.Tipos.FindAsync(id);
            if (tipo == null)
            {
                return null;
            }

            return new TipoResponseDto
            {
                Id = tipo.Id,
                Nome = tipo.Nome,
                Ativo = tipo.Ativo ?? false,
                Publicar = tipo.Publicar ?? false,
                UsuarioId = tipo.UsuarioId,
                Usuario = tipo.Usuario,
                DataCriacao = tipo.DataCriacao,
                DataAtualizacao = tipo.DataAtualizacao
            };
        }

        public async Task<TipoResponseDto> GetTipos()
        {
            var tipo = await _appDbContext.Tipos
                .AsNoTracking()
                .Select(t => new TipoResponseDto
                {
                    Id = t.Id,
                    Nome = t.Nome,
                    Ativo = t.Ativo ?? false,
                    Publicar = t.Publicar ?? false,
                    UsuarioId = t.UsuarioId,
                    Usuario = t.Usuario,
                    DataCriacao = t.DataCriacao,
                    DataAtualizacao = t.DataAtualizacao
                })
                .FirstOrDefaultAsync();

            return tipo;
        }

        public async Task<TipoResponseDto> UpdateTipo(int id, PatchTipoDto dto)
        {
            Tipo tipo = await _appDbContext.Tipos.FindAsync(id);
            if (tipo == null)
            {
                return null;
            }

            if (dto.Nome != null)
            {
                tipo.Nome = dto.Nome;
            }
            if (dto.Ativo != null)
            {
                tipo.Ativo = dto.Ativo;
            }
            if (dto.Publicar != null)
            {
                tipo.Publicar = dto.Publicar;
            }

            tipo.DataAtualizacao = DateTime.UtcNow;

            _appDbContext.Tipos.Update(tipo);
            await _appDbContext.SaveChangesAsync();

            return new TipoResponseDto
            {
                Id = tipo.Id,
                Nome = tipo.Nome,
                Ativo = tipo.Ativo ?? false,
                Publicar = tipo.Publicar ?? false,
                UsuarioId = tipo.UsuarioId,
                Usuario = tipo.Usuario,
                DataCriacao = tipo.DataCriacao,
                DataAtualizacao = tipo.DataAtualizacao
            };
        }

        public Task<IActionResult> DeleteTipo(int id)
        {
            Tipo tipo = _appDbContext.Tipos.Find(id);
            if (tipo == null)
            {
                return Task.FromResult<IActionResult>(new NotFoundResult());
            }

            _appDbContext.Tipos.Remove(tipo);
            _appDbContext.SaveChanges();
            return Task.FromResult<IActionResult>(new NoContentResult());
        }
    }
}
