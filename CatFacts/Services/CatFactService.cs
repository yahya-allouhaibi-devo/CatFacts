using System.Diagnostics;
using System.Net.Http.Json;
using CatFacts.Models;

namespace CatFacts.Services;

public class CatFactService : ICatFactService
{
    private const string _apiUrl = "https://catfact.ninja/fact";
    private readonly HttpClient _client;

    public CatFactService(HttpClient httpClient)
    {
        _client = httpClient;
    }
    public async Task<CatFact?> GetCatFactAsync(CancellationToken ct)
    {
        try
        {
            return await _client.GetFromJsonAsync<CatFact>(_apiUrl, ct);
            
        }
        catch (OperationCanceledException ex)
        {
            Debug.WriteLine($"Opertaion was cancelled : {ex.Message}");
            return null;
        }
        catch 
        {
            return null;
        }
    }
}
