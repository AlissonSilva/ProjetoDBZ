using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoDBZ.Data;
using ProjetoDBZ.DTO.Tipos;
using ProjetoDBZ.Models;
using ProjetoDBZ.Services.Interfaces;

namespace ProjetoDBZ.Services
{
    public class TipoService : ITipoService
    {
        
        private readonly AppDbContext _appDbContext;

        public TipoService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Task<IActionResult> AddTipo(CreateTipoDto dto)
        {
            Tipo tipo = new Tipo
            {
                Nome = dto.Nome,
                Ativo = dto.Ativo,
                Publicar = dto.Publicar
            };

            _appDbContext.Tipos.Add(tipo);
            _appDbContext.SaveChanges();
            return Task.FromResult<IActionResult>(new OkObjectResult(tipo));
        }

        public Task<IActionResult> DeleteTipo(int id)
        {
            Tipo tipo = _appDbContext.Tipos.Find(id);
            if (tipo == null)
            {
                return Task.FromResult<IActionResult>(new NotFoundResult());
            }

            _appDbContext.Tipos.Remove(tipo);
            _appDbContext.SaveChanges();
            return Task.FromResult<IActionResult>(new NoContentResult());
        }

        public Task<IActionResult> GetTipoById(int id)
        {
            Tipo tipo = _appDbContext.Tipos.Find(id);
            if (tipo == null)
            {
                return Task.FromResult<IActionResult>(new NotFoundResult());
            }
            return Task.FromResult<IActionResult>(new OkObjectResult(tipo));
        }

        public Task<IActionResult> GetTipos()
        {
            Task<List<Tipo>> tipos = _appDbContext.Tipos.ToListAsync();
            return Task.FromResult<IActionResult>(new OkObjectResult(tipos));
        }

        public Task<IActionResult> UpdateTipo(int id, CreateTipoDto dto)
        {
            
            Tipo tipo = _appDbContext.Tipos.FindAsync(id).Result;
            if (tipo == null)
            {
                return Task.FromResult<IActionResult>(new NotFoundResult());
            }
            if(dto.Nome != null)
            {
                tipo.Nome = dto.Nome;
            }else if(dto.Ativo != null)
            {
                tipo.Ativo = dto.Ativo;
            }else if(dto.Publicar != null)
            {
                tipo.Publicar = dto.Publicar;
            }

            _appDbContext.Tipos.Update(tipo);
            _appDbContext.SaveChanges();
            return Task.FromResult<IActionResult>(new OkObjectResult(tipo));
        }
    }
}