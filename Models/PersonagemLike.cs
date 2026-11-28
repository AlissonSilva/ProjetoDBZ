namespace ProjetoDBZ.Models
{
    public class PersonagemLike
    {
        public int Id { get; set; }

        public int PersonagemId { get; set; }
        public Personagem Personagem { get; set; }

        public int UsuarioId { get; set; }
        public User.User Usuario { get; set; }

        public DateTime DataCriacao { get; set; } = default(DateTime);
    }
}
