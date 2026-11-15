
using Microsoft.AspNetCore.Mvc;
using ProjetoDBZ.Data;
using ProjetoDBZ.DTO.Favoritos;
using ProjetoDBZ.Models;
using ProjetoDBZ.Services.Interfaces;

namespace ProjetoDBZ.Services
{
    public class FavoritoService : IFavoritoService
    {
        private readonly AppDbContext _appDbContext;

        public FavoritoService(AppDbContext context)
        {
            _appDbContext = context;
        }

        public async Task<IActionResult> AddFavorito(int usuarioId, FavoritoDto favorito)
        {
            Favorito favoritoModel = new Favorito
            {
                UsuarioId = usuarioId,
                PersonagemId = favorito.PersonagemId
            };

            _appDbContext.Favoritos.Add(favoritoModel);
            await _appDbContext.SaveChangesAsync();
            return new OkObjectResult(favorito);
        }


        public async Task<IActionResult> RemoveFavorito(int id)
        {
            var favorito = await _appDbContext.Favoritos.FindAsync(id);
            if (favorito == null)
            {
                return new NotFoundResult();
            }

            _appDbContext.Favoritos.Remove(favorito);
            await _appDbContext.SaveChangesAsync();
            return new OkObjectResult(favorito);
        }
    }
}
