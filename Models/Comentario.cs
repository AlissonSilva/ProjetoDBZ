
using System.ComponentModel.DataAnnotations;

namespace ProjetoDBZ.Models
{
    public class Comentario
    {
        public int Id { get; set; }
        [MaxLength(500)]
        public string Descricao { get; set; }
        public int PersonagemId { get; set; }
        public Personagem personagem { get; set; }
        public int UsuarioId { get; set; }
        public User.User Usuario { get; set; }
        public DateTime DataCriacao { get; set; } = default(DateTime);
        public DateTime DataAtualizacao { get; set; } = default(DateTime);
    }
}
