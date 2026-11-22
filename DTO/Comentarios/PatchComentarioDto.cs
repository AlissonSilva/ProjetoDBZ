using System.ComponentModel.DataAnnotations;

namespace ProjetoDBZ.DTO.Comentarios
{
    public class PatchComentarioDto
    {
        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        public string Descricao { get; set; }
        public DateTime DataAtualizacao { get; set; } = default(DateTime);
    }
}
