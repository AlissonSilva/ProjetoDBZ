
namespace ProjetoDBZ.Models
{
    public class Favorito
    {
        public int Id { get; set; }
        public int PersonagemId { get; set; }
        public Personagem Personagem { get; set; }

        public int UsuarioId { get; set; }
        public User.User Usuario { get; set; }
    }
}
