using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjetoDBZ.Data;
using ProjetoDBZ.Models;

namespace ProjetoDBZ.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoritosController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public FavoritosController(AppDbContext context)
        {
            _appDbContext = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddFavorito(Favorito favorito)
        {
            _appDbContext.Favoritos.Add(favorito);
            await _appDbContext.SaveChangesAsync();
            return Ok(favorito);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFavorito(int id)
        {
            var favorito = await _appDbContext.Favoritos.FindAsync(id);
            if (favorito == null)
            {
                return NotFound();
            }

            _appDbContext.Favoritos.Remove(favorito);
            await _appDbContext.SaveChangesAsync();
            return Ok(favorito);
        }
    }
}
