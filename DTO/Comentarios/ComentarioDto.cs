using System.ComponentModel.DataAnnotations;

namespace ProjetoDBZ.DTO.Comentarios
{
    public class ComentarioDto
    {
        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        public string Descricao { get; set; }
        public int PersonagemId { get; set; }
    }
}
