using ClosedXML.Excel;

public class LeitorPlanilha
{
    public List<Pessoa> Ler(string caminho)
    {
        var pessoas = new List<Pessoa>();

        using var planilha = new XLWorkbook(caminho);
        var aba = planilha.Worksheet(1);

        foreach (var linha in aba.RowsUsed().Skip(1))
        {
            pessoas.Add(new Pessoa
            {
                Nome = Texto(linha, 1),
                Sobrenome = Texto(linha, 2),
                Empresa = Texto(linha, 3),
                Cargo = Texto(linha, 4),
                Endereco = Texto(linha, 5),
                Email = Texto(linha, 6),
                Telefone = Texto(linha, 7),
            });
        }

        return pessoas;
    }
    private static string Texto(IXLRow linha, int coluna)
    {
        return linha.Cell(coluna).GetFormattedString();
    }
}