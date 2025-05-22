using CatFacts.Models;

namespace CatFacts.Services;

public interface ICatDataBaseService
{
    Task<List<CatFact>> GetCatFactsAsync();
    Task<CatFact> GetCatFactByIdAsync(int id);
    Task<List<CatFact>> GetFavoriteFactsAsync();
    Task<int> SaveFactAsync(CatFact fact);
    Task<int> DeleteFactAsync(CatFact fact);
    Task SetCatFactAsFavoriteAsync(int id);
}
