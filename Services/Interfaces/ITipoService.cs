using Microsoft.AspNetCore.Mvc;
using ProjetoDBZ.DTO.Tipos;

namespace ProjetoDBZ.Services.Interfaces
{
    public interface ITipoService
    {
        Task<IActionResult> AddTipo(CreateTipoDto dto);
        Task<IActionResult> GetTipos();
        Task<IActionResult> GetTipoById(int id);
        Task<IActionResult> UpdateTipo(int id, CreateTipoDto dto);
        Task<IActionResult> DeleteTipo(int id);
    }
}