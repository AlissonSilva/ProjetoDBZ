using UserModel = ProjetoDBZ.Models.User.User;
using ProjetoDBZ.Models;

namespace ProjetoDBZ.DTO.Personagens
{
    public class PersonagemResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int TipoId { get; set; }
        public Tipo Tipo { get; set; }
        public bool PossuiImagem { get; set; }
        public bool Publicar { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public int UsuarioId { get; set; }
        public UserModel Usuario { get; set; }
    }
}
