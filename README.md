# Web Scraping - Ração de Gato na Amazon

Meu colega [Ralph](https://www.linkedin.com/in/ralph-harada/) me desafiou a criar um webscraping com C# e para dar a minha carinha decidi usar a Amazon para buscar rações de gato, identificar a mais barata e enviar as informações (nome, preço e link) através do Telegram.

## Como usar o aplicativo

### Pré-requisitos

- [.NET Core SDK](https://dotnet.microsoft.com/download) instalado na sua máquina.
- Clone este repositório localmente usando o comando:

  ```bash
  git clone https://github.com/livnascimento/webscraping.git
  ```

### Configuração do Telegram

1. Crie um bot no Telegram usando o **@BotFather**.
2. Obtenha o token do seu bot gerado pelo **@BotFather**.
3. Obtenha o ID do chat ou grupo onde deseja enviar as mensagens.

### Configuração do ambiente

1. Renomeie o arquivo `.env.example` para `.env`.
2. Substitua os marcadores `TOKEN` e `CHAT_ID` no arquivo `.env` pelos valores obtidos do bot do Telegram.

### Executando o aplicativo

1. Navegue até o diretório onde o repositório foi clonado.
2. Execute o aplicativo usando o comando:

   ```bash
   dotnet run
   ```

O aplicativo irá realizar a busca na Amazon por rações de gato, identificar a mais barata e enviar as informações para o chat ou grupo configurado no Telegram.

## Contribuindo

Se você quiser contribuir com melhorias ou correções, sinta-se à vontade para abrir uma **issue** ou enviar um **pull request**.

## Agradecimentos

Muito obrigada, Ralph, pela ideia de projeto. Foi muito divertido pensar em como dar um toque meu a esse app e construí-lo!

---

Obrigada por visitar meu repositório e não esquece de dar uma olhada nos meus outros projetos :)