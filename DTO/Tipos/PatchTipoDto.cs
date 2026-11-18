namespace ProjetoDBZ.DTO.Tipos
{
    public class PatchTipoDto
    {
        public string Nome { get; set; } = string.Empty;
        public bool? Ativo { get; set; }
        public bool? Publicar { get; set; }
    }
}
