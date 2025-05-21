using CatFacts.Models;
using SQLite;

namespace CatFacts.Services;

public class CatDataBaseService
{
    SQLiteAsyncConnection _database;

    async Task InitializeAsync()
    {
        if (_database is not null)
            return;

        _database = new SQLiteAsyncConnection(Constants.DataBasePath, Constants.Flags);
        await _database.CreateTableAsync<CatFact>();
        
    }

    public async Task<List<CatFact>> GetCatFactsAsync()
    {
        await InitializeAsync();
        return await _database.Table<CatFact>().OrderByDescending(f => f.CreatedAt).ToListAsync();
    }

    public async Task<CatFact> GetCatFactByIdAsync(int id)
    {
        await InitializeAsync();
        return await _database.Table<CatFact>().Where(f => f.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<CatFact>> GetFavoriteFactsAsync()
    {
        await InitializeAsync();
        return await _database.Table<CatFact>().Where(f => f.IsFavorite).OrderByDescending(f => f.CreatedAt).ToListAsync();
    }

    public async Task<int> SaveFactAsync(CatFact fact)
    {
        await InitializeAsync();
        if (fact.Id != 0)
        {
            return await _database.UpdateAsync(fact);
        }
        else
        {
            return await _database.InsertAsync(fact);
        }
    }

    public async Task<int> DeleteFactAsync(CatFact fact)
    {
        await InitializeAsync();
        return await _database.DeleteAsync(fact);
    }

    public async Task SetCatFactAsFavoriteAsync(int id)
    {
        await InitializeAsync();
        var fact = await GetCatFactByIdAsync(id);
        if (fact != null)
        {
            fact.IsFavorite = !fact.IsFavorite;
            await SaveFactAsync(fact);
        }
    }

}
