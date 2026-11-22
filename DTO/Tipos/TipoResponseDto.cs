using ProjetoDBZ.DTO.User;

namespace ProjetoDBZ.DTO.Tipos
{
    public class TipoResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public bool Publicar { get; set; }
        public int UsuarioId { get; set; }
        public UsuarioResponseDto Usuario { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }

    }
}
