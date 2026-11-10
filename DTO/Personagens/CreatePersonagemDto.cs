using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoDBZ.DTO.Personagens
{
    public class CreatePersonagemDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, MinimumLength = 2,
        ErrorMessage = "O nome deve ter entre 2 e 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(10000, ErrorMessage = "A descrição deve ter no máximo 10000 caracteres.")]
        [MaxLength(10000, ErrorMessage = "A descrição deve ter no máximo 10000 caracteres.")]
        public string Descricao { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Selecione um tipo válido.")]
        public int TipoId { get; set; }
        public bool Publicar { get; set; }
        public bool Ativo { get; set; }


    }
}
