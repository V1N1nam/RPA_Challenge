using Microsoft.Playwright;

public class PaginaDesafio
{
    private const string Endereco = "https://rpachallenge.com/";
    private readonly IPage _page;

    public PaginaDesafio(IPage page)
    {
        _page = page;
    }

    public async Task AbrirAsync()
    {
        await _page.GotoAsync(Endereco);
    }

    public async Task<Uri> ObterLinkPlanilhaAsync()
    {
        var href = await _page.Locator("a", new() { HasText = "Download Excel" }).GetAttributeAsync("href")
            ?? throw new Exception("Link da Planilha não encontrado");

        return new Uri(new Uri(_page.Url), href);
    }

    public async Task IniciarAsync()
    {
        await _page.GetByRole(AriaRole.Button, new() { Name = "start" }).ClickAsync();
    }

    public async Task PreencherAsync(Pessoa pessoa)
    {
        await Campo("labelFirstName").FillAsync(pessoa.Nome);
        await Campo("labelLastName").FillAsync(pessoa.Sobrenome);
        await Campo("labelCompanyName").FillAsync(pessoa.Empresa);
        await Campo("labelRole").FillAsync(pessoa.Cargo);
        await Campo("labelAddress").FillAsync(pessoa.Endereco);
        await Campo("labelEmail").FillAsync(pessoa.Email);
        await Campo("labelPhone").FillAsync(pessoa.Telefone);

        await _page.GetByRole(AriaRole.Button, new() { Name = "Submit" }).ClickAsync();
    }

    public async Task<string> LerResultadoAsync()
    {
        return await _page.GetByText("Your success rate").InnerTextAsync();
    }

    public async Task TirarPrintAsync(string caminho)
    {
        await _page.ScreenshotAsync(new() { Path = caminho, FullPage = true });
    }

    private ILocator Campo(string nome)
    {
        return _page.Locator($"input[ng-reflect-name='{nome}']");
    }
}