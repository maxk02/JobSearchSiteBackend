using Newtonsoft.Json;

namespace Infrastructure;

public static class SeedFileHelper
{
    public static async Task<T?> LoadJsonAsync<T>(string relativePath)
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
        if (!File.Exists(path))
            throw new FileNotFoundException($"File {path} not found for seeding.");

        var json = await File.ReadAllTextAsync(path);
        return JsonConvert.DeserializeObject<T>(json);
    }
}