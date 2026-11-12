

using System.ComponentModel.DataAnnotations;

namespace ProjetoDBZ.DTO.Personagens
{
    public class PersonagemDtoPatch
    {


        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        [Range(1, int.MaxValue, ErrorMessage = "Selecione um tipo válido.")]
        public int TipoId { get; set; }
        public bool Publicar { get; set; }
        public bool Ativo { get; set; }
    }
}
