using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Globalization;
using DotNetEnv;

class Program
{
    static async Task Main()
    {
        List<object> produtos = PesquisarPrecos();

        EscreverArquivo(produtos);

        object racaoMaisBarata = ProcurarRacaoMaisBarata();

        await EnviarMensagem(racaoMaisBarata);
    }

    private static List<object> PesquisarPrecos()
    {

        IWebDriver driver = new ChromeDriver();

        string url = "https://www.amazon.com.br/s?k=ra%C3%A7%C3%A3o+para+gatos";
        driver.Navigate().GoToUrl(url);

        List<object> produtosList = new List<object>();

        try
        {
            var produtos = driver.FindElements(By.CssSelector("div[data-component-type='s-search-result']"));

            foreach (var produto in produtos)
            {
                string nomeProduto = produto.FindElement(By.CssSelector("span.a-text-normal")).Text.Trim();

                if (nomeProduto.Contains("10,1kg"))
                {
                    try
                    {
                        IWebElement linkElement = produto.FindElement(By.CssSelector("a.a-link-normal"));

                        string linkProduto = linkElement.GetAttribute("href");

                        string precoReais = produto.FindElement(By.CssSelector("span.a-price-whole")).Text;
                        string precoCents = produto.FindElement(By.CssSelector("span.a-price-fraction")).Text;
                        string precoProduto = $"{precoReais}.{precoCents}";

                        var produtoObj = new
                        {
                            Produto = nomeProduto,
                            Preco = precoProduto,
                            Link = linkProduto
                        };

                        produtosList.Add(produtoObj);
                    }
                    catch (NoSuchElementException)
                    {
                        var produtoObj = new
                        {
                            Produto = nomeProduto,
                            Preco = "Preço ou link não encontrados",
                            Link = "Preço ou link não encontrados"
                        };

                        produtosList.Add(produtoObj);
                    }
                }
            }

            return produtosList;

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
    }
    private static void EscreverArquivo(List<object> produtos)
    {
        string jsonProdutos = JsonConvert.SerializeObject(produtos, Formatting.Indented);

        File.WriteAllText(@"racoes.json", jsonProdutos);
    }

    private static object ProcurarRacaoMaisBarata()
    {
        try
        {
            var racoesEncontradas = File.ReadAllText(@"racoes.json");
            var racoesJson = JsonConvert.DeserializeObject<dynamic[]>(racoesEncontradas);

            if (racoesJson != null && racoesJson.Length > 0)
            {
                var racoesComPrecoValido = racoesJson.Where(racao => double.TryParse((string)racao.Preco, out double precoDecimal)).ToList();

                if (racoesComPrecoValido.Count > 0)
                {
                    var racaoMaisBarata = racoesComPrecoValido.OrderBy(racao =>
                    {
                        string cleanPrice = new(((string)racao.Preco).Where(c => char.IsDigit(c) || c == '.').ToArray());
                        return double.Parse(cleanPrice, CultureInfo.InvariantCulture);
                    }).First();

                    return racaoMaisBarata;

                }
                else
                {
                    Console.WriteLine("Não há rações com preços válidos na lista.");
                }
            }
            else
            {
                Console.WriteLine("A lista de rações está vazia.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu um erro ao procurar a ração mais barata: {ex.Message}");
        }
        return new {};
    }

    private static async Task EnviarMensagem(object racaoMaisBarata)
    {

        Env.Load();
        var token = Environment.GetEnvironmentVariable("TOKEN");
        var chatId = Environment.GetEnvironmentVariable("CHAT_ID");

        var mensagem = $"A ração mais 'no precin' que encontrei foi: {racaoMaisBarata}";

        string urlString = $"https://api.telegram.org/bot{token}/sendMessage?chat_id={chatId}&text={mensagem}";

        using HttpClient client = new();
        HttpResponseMessage response = await client.GetAsync(urlString);

    }

}
