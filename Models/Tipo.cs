using System.ComponentModel.DataAnnotations;

namespace ProjetoDBZ.Models
{
    public class Tipo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 100 caracteres.")]
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public bool Publicar { get; set; }

    }
}
