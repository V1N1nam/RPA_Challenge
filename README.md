# RPA Challenge com C# e Playwright

Robô que resolve o [RPA Challenge](https://rpachallenge.com/), um desafio clássico de automação: baixar uma planilha com 10 pessoas e preencher um formulário web com os dados de cada uma. A dificuldade é que, a cada envio, os campos do formulário mudam de posição na tela, então o robô não pode depender da ordem dos campos.

## Resultado

**100% de acerto (70 de 70 campos) em cerca de 1,5 segundo.**

## Como funciona

1. Abre o Microsoft Edge e acessa o site do desafio
2. Encontra o link da planilha na página e baixa o arquivo
3. Lê as 10 pessoas da planilha
4. Clica em Start para iniciar o cronômetro
5. Para cada pessoa, preenche os 7 campos e envia o formulário
6. Lê a mensagem de resultado, registra no log e salva um print da tela

## Estrutura do projeto

```
RpaChallenge/
├── Models/
│   └── Pessoa.cs
├── Pages/
│   └── PaginaDesafio.cs
├── Services/
│   ├── Baixador.cs
│   ├── LeitorPlanilha.cs
│   └── Log.cs
├── Program.cs
└── RpaChallenge.csproj
```

O código é separado em classes, cada uma com uma responsabilidade:

| Arquivo | Responsabilidade |
|---|---|
| `Program.cs` | Coordena o fluxo do robô, do início ao fim |
| `Pages/PaginaDesafio.cs` | Concentra toda a interação com o site: abrir, localizar elementos, preencher e enviar (padrão Page Object) |
| `Services/LeitorPlanilha.cs` | Lê o arquivo Excel e transforma cada linha em um objeto `Pessoa` |
| `Services/Baixador.cs` | Faz o download de arquivos via HTTP |
| `Services/Log.cs` | Registra cada etapa no terminal e em arquivo |
| `Models/Pessoa.cs` | Representa os dados de uma pessoa da planilha |

Com essa separação, uma mudança no site afeta apenas a `PaginaDesafio`, e uma mudança no formato da planilha afeta apenas o `LeitorPlanilha`.

## Decisões técnicas

**Localização dos campos por atributo estável.** Os campos do formulário têm `id` e `name` gerados aleatoriamente a cada rodada, e os rótulos não estão associados aos campos no HTML. Inspecionando a página, identifiquei o atributo `ng-reflect-name`, que se mantém fixo mesmo quando os campos mudam de posição. Cada campo é localizado por ele:

```csharp
_page.Locator($"input[ng-reflect-name='{nome}']");
```

**Botões localizados pelo papel e pelo texto.** Os botões Start e Submit são encontrados com `GetByRole`, que busca pelo papel de acessibilidade e pelo texto visível, da mesma forma que uma pessoa identificaria o botão. Isso torna o robô menos sensível a mudanças na estrutura do HTML.

**Uso do navegador instalado.** O download dos navegadores do Playwright foi bloqueado pela inspeção de certificados da rede corporativa. Em vez de alterar configurações de segurança, o robô usa o Microsoft Edge já instalado no Windows (`Channel = "msedge"`).

**Download da planilha via HTTP.** O download pelo clique no navegador também não era concluído no ambiente corporativo. O robô passou a ler o endereço do link na página e baixar o arquivo diretamente com `HttpClient`, o que além de contornar o bloqueio é mais rápido e confiável.

**Log e tratamento de erros.** Cada etapa é registrada com data e hora no terminal e no arquivo `execucao.log`. Ao final, o robô lê a mensagem de resultado do site, registra no log e salva um print (`resultado.png`). Se qualquer etapa falhar, o erro é registrado no log e um print da tela no momento da falha é salvo com data e hora no nome. Esse cenário foi testado forçando um seletor inválido.

## Tecnologias

- C# / .NET 10
- [Playwright](https://playwright.dev/dotnet/) para automação do navegador
- [ClosedXML](https://github.com/ClosedXML/ClosedXML) para leitura da planilha Excel

## Como executar

Requisitos: .NET 10 SDK e Microsoft Edge instalados.

```bash
git clone https://github.com/V1N1nam/RPA_Challenge.git
cd RpaChallenge
dotnet run
```

Como o robô usa o Edge já instalado, não é necessário baixar os navegadores do Playwright.

Para acompanhar o robô preenchendo os campos, aumente o valor de `SlowMo` no `Program.cs` (por exemplo, `SlowMo = 500`).