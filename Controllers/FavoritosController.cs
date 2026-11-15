using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ProjetoDBZ.DTO.Favoritos;
using ProjetoDBZ.Services.Interfaces;

namespace ProjetoDBZ.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoritosController : ControllerBase
    {

        private readonly IFavoritoService _favoritoService;

        public FavoritosController(IFavoritoService favoritoService)
        {
            _favoritoService = favoritoService;
        }

        [HttpPost]
        public async Task<IActionResult> AddFavorito(FavoritoDto favorito)
        {

            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (usuarioId == null)
                return Unauthorized();

            var result = await _favoritoService.AddFavorito(int.Parse(usuarioId), favorito);
            return result;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFavorito(int id)
        {
            var result = await _favoritoService.RemoveFavorito(id);
            return result;
        }
    }
}
