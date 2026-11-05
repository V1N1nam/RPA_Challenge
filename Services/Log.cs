public static class Log
{
    private const string Arquivo = "execucao.log";

    public static void Info(string mensagem) => Escrever("INFO", mensagem);
    public static void Erro(string mensagem) => Escrever("ERRO", mensagem);

    private static void Escrever(string nivel, string mensagem)
    {
        var linha = $"{DateTime.Now:dd/MM/yyyy HH:mm:ss} [{nivel}] {mensagem}";
        Console.WriteLine(linha);
        File.AppendAllText(Arquivo, linha + Environment.NewLine);
    }
}