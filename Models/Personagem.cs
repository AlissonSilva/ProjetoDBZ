using UserModel = ProjetoDBZ.Models.User.User;

namespace ProjetoDBZ.Models
{
    public class Personagem
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool Publicar { get; set; }
        public bool Ativo { get; set; }
        public byte[] Imagem { get; set; }
        public Tipo Tipo { get; set; }
        public int TipoId { get; set; }
        public DateTime DataCriacao { get; set; } = default(DateTime);
        public DateTime DataAtualizacao { get; set; } = default(DateTime);
        public int UsuarioId { get; set; }
        public UserModel Usuario { get; set; }

    }
}
