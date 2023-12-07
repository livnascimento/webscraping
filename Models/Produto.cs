namespace webscraping.Models
{
    public class Produto
    {
        public Produto() {}
        public Produto(string nome, string preco, string link)
        {
            Nome = nome;
            Preco = preco;
            Link = link;
        }
        public string Nome { get; set; }
        public string Preco { get; set; }
        public string Link { get; set; } 

    }
}