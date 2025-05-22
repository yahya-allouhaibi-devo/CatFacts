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
            var response = await _client.GetAsync(_apiUrl, ct);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CatFact>(ct);
            
        }
        catch (OperationCanceledException ex)
        {
            Debug.WriteLine($"[GetCatFactAsync: OPERATION WAS CANCELLED]: {ex.Message}");
            throw new OperationCanceledException("Operation was cancelled during fetching cat fact", ex);
        }
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"[GetCatFactAsync: HTTP REQUEST ERROR]:{ex.Message}");
            throw new HttpRequestException("An error occured while fetching cat fact");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[GetCatFactAsync GENERAL ERROR]: {ex.Message}");
            throw new Exception($"An unexpected error occurred while fetching cat fact.", ex);
        }
    }
}
