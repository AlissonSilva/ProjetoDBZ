namespace ProjetoDBZ.DTO.User
{
    public class UsuarioResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public byte[] Imagem { get; set; } = Array.Empty<byte>();
        public DateTime DataCriacao { get; set; } = default(DateTime);
    }
}
