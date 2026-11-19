using Microsoft.AspNetCore.Mvc;
using ProjetoDBZ.DTO.Tipos;

namespace ProjetoDBZ.Services.Interfaces
{
    public interface ITipoService
    {
        Task<TipoResponseDto> AddTipo(CreateTipoDto dto, int usuarioId);
        Task<List<TipoResponseDto>> GetTipos();
        Task<TipoResponseDto> GetTipoById(int id);
        Task<TipoResponseDto> UpdateTipo(int id, PatchTipoDto dto);
        Task<IActionResult> DeleteTipo(int id);
    }
}
