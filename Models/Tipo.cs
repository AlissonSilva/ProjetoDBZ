using System.ComponentModel.DataAnnotations;

namespace ProjetoDBZ.Models
{
    public class Tipo
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public bool? Ativo { get; set; }
        public bool? Publicar { get; set; }

    }
}
