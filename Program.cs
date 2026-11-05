using Microsoft.Playwright;

const string arquivoPlanilha = "challenge.xlsx";

using var playwright = await Playwright.CreateAsync();
await using var browser = await playwright.Chromium.LaunchAsync(new() { Channel = "msedge", Headless = false, SlowMo = 0 });

var pagina = new PaginaDesafio(await browser.NewPageAsync());

try
{
    Log.Info("Iniciando robô");
    await pagina.AbrirAsync();

    var url = await pagina.ObterLinkPlanilhaAsync();
    await Baixador.BaixarAsync(url, arquivoPlanilha);
    Log.Info($"Planilha baixada de {url}");

    var pessoas = new LeitorPlanilha().Ler(arquivoPlanilha);
    Log.Info($"{pessoas.Count} pessoas lidas");

    await pagina.IniciarAsync();
    foreach (var pessoa in pessoas)
    {
        await pagina.PreencherAsync(pessoa);
        Log.Info($"Enviado: {pessoa.Nome} {pessoa.Sobrenome}");
    }

    var resultado = await pagina.LerResultadoAsync();
    Log.Info($"Resultado: {resultado}");
    await pagina.TirarPrintAsync("resultado.png");
}
catch (Exception ex)
{
    Log.Erro(ex.Message);
    await pagina.TirarPrintAsync($"erro_{DateTime.Now:yyyyMMdd_HHmmss}.png");
}

Console.ReadLine();