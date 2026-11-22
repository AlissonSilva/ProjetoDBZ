using Microsoft.AspNetCore.Mvc;
using ProjetoDBZ.DTO.Comentarios;

namespace ProjetoDBZ.Services.Interfaces
{
    public interface IComentarioService
    {
        Task<ComentarioResponseDto> Add(ComentarioDto dto, int usuarioId);
        Task<List<ComentarioResponseDto>> Get(int personagemId);
        Task<List<ComentarioResponseDto>> GetByUsuario(int usuarioId);
        Task<ComentarioResponseDto> Update(PatchComentarioDto dto, int id);
        Task<IActionResult> Delete(int id);
    }
}
