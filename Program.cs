using webscraping.Models;
using webscraping.Services;

class Program
{
  static async Task Main()
  {
    string url = "https://www.amazon.com.br/s?k=ra%C3%A7%C3%A3o+para+gatos";

    List<Produto> produtosValidos = Produtos.BuscarProdutosValidos(url);
  
    Produtos.SalvarElementosNoArquivo(produtosValidos);

    Produto melhorProduto = Produtos.ProcurarMelhorPreco();

    string produtoFormatado = Mensagem.FormatarProduto(melhorProduto);

    await Mensagem.EnviarMensagem(produtoFormatado);
  }
  
}
