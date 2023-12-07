using DotNetEnv;
using webscraping.Models;

namespace webscraping.Services
{
    public class Mensagem
    {
        public static string FormatarProduto(Produto produtoMaisBarato) {
            string textoFormatado = $"{produtoMaisBarato.Nome} que está custando {produtoMaisBarato.Preco:C}!\nVocê pode acessá-lo clicando no seguinte link:\n{produtoMaisBarato.Link}";

            return textoFormatado;
        }
        public static async Task EnviarMensagem(string melhorProduto)
        {
            Env.Load();
            var token = Environment.GetEnvironmentVariable("TOKEN");
            var chatId = Environment.GetEnvironmentVariable("CHAT_ID");

            var mensagem = $"O melhor produto que encontrei foi: {melhorProduto}";

            string urlString = $"https://api.telegram.org/bot{token}/sendMessage?chat_id={chatId}&text={mensagem}";

            using HttpClient client = new();
            HttpResponseMessage response = await client.GetAsync(urlString);

        }
    }
}