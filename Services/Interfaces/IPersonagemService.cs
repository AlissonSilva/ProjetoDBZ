using ProjetoDBZ.DTO.Common;
using ProjetoDBZ.DTO.Personagens;

namespace ProjetoDBZ.Services.Interfaces
{
    public interface IPersonagemService
    {
        Task<PersonagemResponseDto> AddAsync(PersonagemDto dto);

        Task<PagedResponseDto<PersonagemResponseDto>> GetAllAsync(int page, int pageSize, string nome, int tipoId);

        Task<PersonagemResponseDto> GetByIdAsync(int id);

        Task<PersonagemResponseDto> UpdateAsync(int id, PersonagemDto dto);

        Task<PersonagemResponseDto> PatchAsync(int id, PersonagemDto dto);

        Task<bool> UpdateImageAsync(int id, IFormFile imagem);

        Task<bool> DeleteAsync(int id);
    }
}
