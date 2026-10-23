using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoDBZ.Data;
using ProjetoDBZ.Models;

namespace ProjetoDBZ.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public TipoController(AppDbContext context)
        {
            _appDbContext = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddTipo(Tipo tipo)
        {
            _appDbContext.Tipos.Add(tipo);
            await _appDbContext.SaveChangesAsync();
            return Ok(tipo);
        }

        [HttpGet]
        public async Task<IActionResult> GetTipos()
        {
            var tipos = await _appDbContext.Tipos.ToListAsync();
            return Ok(tipos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTipoById(int id)
        {
            var tipo = await _appDbContext.Tipos.FindAsync(id);
            if (tipo == null)
            {
                return NotFound();
            }
            return Ok(tipo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTipo(int id, Tipo updatedTipo)
        {
            var tipo = await _appDbContext.Tipos.FindAsync(id);
            if (tipo == null)
            {
                return NotFound();
            }

            tipo.Nome = updatedTipo.Nome;

            await _appDbContext.SaveChangesAsync();
            return Ok(tipo);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipo(int id)
        {
            var tipo = await _appDbContext.Tipos.FindAsync(id);
            if (tipo == null)
            {
                return NotFound();
            }

            _appDbContext.Tipos.Remove(tipo);
            await _appDbContext.SaveChangesAsync();
            return NoContent();
        }

    }
}