using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoDBZ.Models;

namespace ProjetoDBZ.DTO.Personagens
{
    public class PersonagemResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int TipoId { get; set; }
        public Tipo tipo { get; set; }
        public bool PossuiImagem { get; set; }
        public bool Publicar { get; set; }
        public bool Ativo { get; set; }

    }
}
