
using Microsoft.AspNetCore.Mvc;
using ProjetoDBZ.DTO.Favoritos;

namespace ProjetoDBZ.Services.Interfaces
{
    public interface IFavoritoService
    {
        Task<IActionResult> AddFavorito(int usuarioId, FavoritoDto favorito);

        Task<IActionResult> RemoveFavorito(int id);
    }
}
