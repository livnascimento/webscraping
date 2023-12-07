using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Newtonsoft.Json;
using webscraping.Models;
using System.Globalization;

namespace webscraping.Services
{
  public class Produtos
  {
    public static List<Produto> BuscarProdutosValidos(string url)
    {
      ChromeDriver driver = new();

      List<IWebElement> listaElementos = [];
      List<Produto> listaProdutosValidos = [];

      try
      {
        driver.Navigate().GoToUrl(url);

        var produtos = driver.FindElements(By.CssSelector("div[data-component-type='s-search-result']"));

        listaElementos.AddRange(produtos);

        foreach (IWebElement produto in listaElementos)
        {
          string nomeProduto = produto.FindElement(By.CssSelector("span.a-text-normal")).Text.Trim();
          if (nomeProduto.Contains("10,1kg"))
          {
            IWebElement linkElement = produto.FindElement(By.CssSelector("a.a-link-normal"));

            string linkProduto = linkElement.GetAttribute("href");

            string precoReais = produto.FindElement(By.CssSelector("span.a-price-whole")).Text;
            string precoCents = produto.FindElement(By.CssSelector("span.a-price-fraction")).Text;
            string precoProduto = $"{precoReais}.{precoCents}";

            Produto novoProduto = new(nome: nomeProduto, preco: precoProduto, link: linkProduto);

            listaProdutosValidos.Add(novoProduto);
          }
        }
      }
      catch (WebDriverTimeoutException)
      {
        throw new ArgumentException("Tempo de espera excedido. A página pode não ter carregado completamente.");
      }
      catch (Exception ex)
      {
        throw new ArgumentException($"Ocorreu um erro: {ex.Message}");
      }
      finally
      {
        driver.Quit();
      }
      return listaProdutosValidos;
    }
    public static void SalvarElementosNoArquivo(List<Produto> listaProdutos)
    {
      try
      {
        string jsonProdutos = JsonConvert.SerializeObject(listaProdutos, Formatting.Indented);

        File.WriteAllText(@"racoes.json", jsonProdutos);

      }
      catch (FileNotFoundException)
      {
        throw new Exception("Arquivo não encontrado.");
      }
      catch (Exception ex)
      {
        throw new Exception($"Ocorre um erro ao escrever o arquivo: {ex.Message}");
      }
    }
    public static Produto ProcurarMelhorPreco()
    {
      try
      {
        var racoesEncontradas = File.ReadAllText(@"racoes.json");
        var racoesJson = JsonConvert.DeserializeObject<Produto[]>(racoesEncontradas);

        if (racoesJson != null && racoesJson.Length > 0)
        {
          List<Produto> racoesComPrecoValido = racoesJson.Where(racao => double.TryParse((string)racao.Preco, out double precoDecimal)).ToList();

          if (racoesComPrecoValido.Count > 0)
          {
            Produto produtoMelhorPreco = racoesComPrecoValido.OrderBy(racao =>
            {
              string cleanPrice = new(((string)racao.Preco).Where(c => char.IsDigit(c) || c == '.').ToArray());
              return double.Parse(cleanPrice, CultureInfo.InvariantCulture);
            }).First();

            return produtoMelhorPreco;
          }
          else
          {
            Console.WriteLine("Não há rações com preços válidos na lista.");
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception($"Ocorreu um erro ao procurar a ração mais barata: {ex.Message}");
      }
      return new Produto();
    }
  }
}

