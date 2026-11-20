using System.ComponentModel.DataAnnotations;

namespace ProjetoDBZ.DTO.Comentarios
{
    public class ComentarioDto
    {
        public int Id { get; set; }
        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        public string Descricao { get; set; }
        public int PersonagemId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime DataCriacao { get; set; } = default(DateTime);
        public DateTime DataAtualizacao { get; set; } = default(DateTime);
    }
}
