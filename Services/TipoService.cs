using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoDBZ.Data;
using ProjetoDBZ.DTO.Tipos;
using ProjetoDBZ.DTO.User;
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
                Usuario = new UsuarioResponseDto
                {
                    Id = usuario.Id,
                    Name = usuario.Name,
                    Username = usuario.Username,
                    IsActive = usuario.IsActive,
                    DataCriacao = usuario.DataCriacao,
                    Imagem = usuario.Imagem
                },
                DataCriacao = tipo.DataCriacao,
                DataAtualizacao = tipo.DataAtualizacao
            };
        }

        public async Task<TipoResponseDto> GetTipoById(int id)
        {
            return await _appDbContext.Tipos
                .AsNoTracking()
                .Where(t => t.Id == id)
                .Select(t => new TipoResponseDto
                {
                    Id = t.Id,
                    Nome = t.Nome,
                    Ativo = t.Ativo ?? false,
                    Publicar = t.Publicar ?? false,
                    UsuarioId = t.UsuarioId,

                    Usuario = new UsuarioResponseDto
                    {
                        Id = t.Usuario.Id,
                        Name = t.Usuario.Name,
                        Username = t.Usuario.Username,
                        IsActive = t.Usuario.IsActive,
                        DataCriacao = t.Usuario.DataCriacao,
                        Imagem = t.Usuario.Imagem
                    },

                    DataCriacao = t.DataCriacao,
                    DataAtualizacao = t.DataAtualizacao
                }).FirstOrDefaultAsync();
        }

        public async Task<List<TipoResponseDto>> GetTipos()
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
                    Usuario = new UsuarioResponseDto
                    {
                        Id = t.UsuarioId,
                        Name = t.Usuario.Name,
                        Username = t.Usuario.Username,
                        IsActive = t.Usuario.IsActive,
                        DataCriacao = t.Usuario.DataCriacao,
                        Imagem = t.Usuario.Imagem
                    },
                    DataCriacao = t.DataCriacao,
                    DataAtualizacao = t.DataAtualizacao
                }).ToListAsync();

            return tipo;
        }

        public async Task<TipoResponseDto> UpdateTipo(int id, PatchTipoDto dto)
        {
            var tipo = await _appDbContext.Tipos.Include(t => t.Usuario).FirstOrDefaultAsync(t => t.Id == id);

            if (tipo == null)
                return null;

            tipo.Nome = string.IsNullOrWhiteSpace(dto.Nome) ? tipo.Nome : dto.Nome;

            if (dto.Ativo.HasValue)
                tipo.Ativo = dto.Ativo.Value;

            if (dto.Publicar.HasValue)
                tipo.Publicar = dto.Publicar.Value;

            tipo.DataAtualizacao = DateTime.UtcNow;

            await _appDbContext.SaveChangesAsync();

            return new TipoResponseDto
            {
                Id = tipo.Id,
                Nome = tipo.Nome,
                Ativo = tipo.Ativo ?? false,
                Publicar = tipo.Publicar ?? false,
                UsuarioId = tipo.UsuarioId,

                Usuario = tipo.Usuario == null ? null : new UsuarioResponseDto
                {
                    Id = tipo.Usuario.Id,
                    Name = tipo.Usuario.Name,
                    Username = tipo.Usuario.Username,
                    IsActive = tipo.Usuario.IsActive,
                    DataCriacao = tipo.Usuario.DataCriacao,
                    Imagem = tipo.Usuario.Imagem
                },

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
