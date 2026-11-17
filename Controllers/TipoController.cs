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

namespace ProjetoDBZ.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoController : ControllerBase
    {
        private readonly ITipoService _tipoService;

        public TipoController(ITipoService tipoService)
        {
            _tipoService = tipoService;
        }

        [HttpPost]
        public async Task<IActionResult> AddTipo(CreateTipoDto dto)
        {
            return await _tipoService.AddTipo(dto);
        }

        [HttpGet]
        public async Task<IActionResult> GetTipos()
        {
            return await _tipoService.GetTipos();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTipoById(int id)
        {
            return await _tipoService.GetTipoById(id);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateTipo(int id, CreateTipoDto dto)
        {
            return await _tipoService.UpdateTipo(id, dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipo(int id)
        {
            return await _tipoService.DeleteTipo(id);
        }

    }
}