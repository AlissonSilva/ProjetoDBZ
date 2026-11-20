namespace ProjetoDBZ.Models
{
    public class Tipo
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public bool? Ativo { get; set; }
        public bool? Publicar { get; set; }
        public int UsuarioId { get; set; }
        public User.User Usuario { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;

    }
}
