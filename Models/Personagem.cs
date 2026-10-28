namespace ProjetoDBZ.Models
{
    public class Personagem
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string descricao { get; set; }
        public byte[] Imagem { get; set; }
        public Tipo Tipo { get; set; }

    }
}