using System.ComponentModel.DataAnnotations;
using ProjetoDBZ.DTO.User;

namespace ProjetoDBZ.DTO.Comentarios
{
    public class ComentarioResponseDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(500)]
        public string Descricao { get; set; }
        public int UsuarioId { get; set; }
        public int PersonagemId { get; set; }
        public UsuarioResponseDto Usuario { get; set; }
        public DateTime DataCriacao { get; set; } = default(DateTime);
        public DateTime DataAtualizacao { get; set; } = default(DateTime);
    }
}
