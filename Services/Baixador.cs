public static class Baixador
{
    private static readonly HttpClient _http = new();

    public static async Task BaixarAsync(Uri url, string caminho)
    {
        var bytes = await _http.GetByteArrayAsync(url);
        await File.WriteAllBytesAsync(caminho, bytes);
    }
}