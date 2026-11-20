using Microsoft.AspNetCore.Mvc;
using ProjetoDBZ.Data;
using ProjetoDBZ.DTO.Comentarios;
using ProjetoDBZ.Services.Interfaces;
using ProjetoDBZ.Models;
using ProjetoDBZ.DTO.User;
using Microsoft.EntityFrameworkCore;

namespace ProjetoDBZ.Services
{
    public class ComentarioService : IComentarioService
    {

        private readonly AppDbContext _appDbContext;

        public ComentarioService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ComentarioResponseDto> Add(ComentarioDto dto, int usuarioId)
        {
            var usuario = _appDbContext.Users.FirstOrDefault(u => u.Id == usuarioId);
            if (usuario == null)
                throw new ArgumentException("Usuário inválido.");

            var comentario = new Comentario
            {
                Descricao = dto.Descricao,
                PersonagemId = dto.PersonagemId,
                UsuarioId = usuarioId,
                DataCriacao = DateTime.UtcNow,
                DataAtualizacao = DateTime.UtcNow
            };

            _appDbContext.Comentarios.Add(comentario);
            _appDbContext.SaveChanges();

            return new ComentarioResponseDto
            {
                Id = comentario.Id,
                Descricao = comentario.Descricao,
                UsuarioId = comentario.UsuarioId,
                Usuario = new UsuarioResponseDto
                {
                    Id = usuario.Id,
                    Username = usuario.Username,
                    Name = usuario.Name,
                    Imagem = usuario.Imagem,
                    IsActive = usuario.IsActive,
                    DataCriacao = usuario.DataCriacao
                },
                PersonagemId = comentario.PersonagemId,
                DataCriacao = comentario.DataCriacao,
                DataAtualizacao = comentario.DataAtualizacao
            };
        }

        public async Task<List<ComentarioResponseDto>> Get(int personagemId)
        {
            var comentario = await _appDbContext.Comentarios
            .AsNoTracking()
            .Where(p => p.PersonagemId == personagemId)
            .Select(p => new ComentarioResponseDto
            {
                Id = p.Id,
                Descricao = p.Descricao,
                Usuario = new UsuarioResponseDto
                {
                    Id = p.UsuarioId,
                    Name = p.Usuario.Name,
                    Username = p.Usuario.Username,
                    IsActive = p.Usuario.IsActive,
                    DataCriacao = p.Usuario.DataCriacao,
                    Imagem = p.Usuario.Imagem

                },
                DataCriacao = p.DataCriacao,
                DataAtualizacao = p.DataAtualizacao
            }).ToListAsync();

            return comentario;
        }

        public async Task<List<ComentarioResponseDto>> GetByUsuario(int usuarioId)
        {
            var comentario = await _appDbContext.Comentarios
            .AsNoTracking()
            .Where(p => p.UsuarioId == usuarioId)
            .Select(p => new ComentarioResponseDto
            {
                Id = p.Id,
                Descricao = p.Descricao,
                Usuario = new UsuarioResponseDto
                {
                    Id = p.UsuarioId,
                    Name = p.Usuario.Name,
                    Username = p.Usuario.Username,
                    IsActive = p.Usuario.IsActive,
                    DataCriacao = p.Usuario.DataCriacao,
                    Imagem = p.Usuario.Imagem

                },
                DataCriacao = p.DataCriacao,
                DataAtualizacao = p.DataAtualizacao
            }).ToListAsync();

            return comentario;
        }

        public async Task<ComentarioResponseDto> Update(PatchComentarioDto dto, int id)
        {
            var comentario = await _appDbContext.Comentarios.Include(u => u.Usuario).FirstOrDefaultAsync(t => t.Id == id);

            if (comentario == null)
                return null;

            comentario.Descricao = string.IsNullOrWhiteSpace(dto.Descricao) ? comentario.Descricao : dto.Descricao;

            comentario.DataAtualizacao = DateTime.UtcNow;
            await _appDbContext.SaveChangesAsync();

            return new ComentarioResponseDto
            {
                Id = comentario.Id,
                Descricao = comentario.Descricao,
                Usuario = new UsuarioResponseDto
                {
                    Id = comentario.UsuarioId,
                    Name = comentario.Usuario.Name,
                    Username = comentario.Usuario.Username,
                    IsActive = comentario.Usuario.IsActive,
                    DataCriacao = comentario.Usuario.DataCriacao,
                    Imagem = comentario.Usuario.Imagem

                },
                DataCriacao = comentario.DataCriacao,
                DataAtualizacao = comentario.DataAtualizacao
            };

        }


        public Task<IActionResult> Delete(int id)
        {
            var comentario = _appDbContext.Comentarios.Find(id);
            if (comentario == null)
                return Task.FromResult<IActionResult>(new NotFoundResult());

            _appDbContext.Comentarios.Remove(comentario);
            _appDbContext.SaveChanges();
            return Task.FromResult<IActionResult>(new NoContentResult());

        }

    }
}
