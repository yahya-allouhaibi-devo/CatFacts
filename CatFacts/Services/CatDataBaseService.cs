using System.Diagnostics;
using CatFacts.Exceptions;
using CatFacts.Models;
using SQLite;

namespace CatFacts.Services;

public class CatDataBaseService : ICatDataBaseService
{
    SQLiteAsyncConnection? _database;

    async Task InitializeAsync()
    {
        if (_database is not null)
            return;

        try
        {
            Debug.WriteLine($"[InitializeAsync] Database Path: {Constants.DataBasePath}");
            _database = new SQLiteAsyncConnection(Constants.DataBasePath, Constants.Flags);
            await _database.CreateTableAsync<CatFact>();
            Debug.WriteLine("Database initialized successfully.");
        }
        catch (SQLiteException ex)
        {
            Debug.WriteLine($"[InitializeAsync SQLITE ERROR]: {ex.Message}");
            _database = null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[InitializeAsync GENERAL ERROR]: {ex.Message}");
            _database = null;
            throw;
        }

    }

    public async Task<List<CatFact>> GetCatFactsAsync()
    {
        try
        {
            await InitializeAsync();
            if (_database == null)
            {
                Debug.WriteLine($"[GetCatFactsAsync ERROR]: Database not initialized");
                throw new InvalidOperationException("Database service is not properly initialized.");
            }

            return await _database.Table<CatFact>().OrderByDescending(f => f.CreatedAt).ToListAsync();
        }
        catch (SQLiteException ex)
        {
            Debug.WriteLine($"[GetCatFactsAsync SQLITE ERROR]: {ex.Message}");
            throw new DataAccessException("A database error occurred while retrieving facts.", ex);
        }
        catch (InvalidOperationException)
        {
            throw; 
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[GetCatFactsAsync GENERAL ERROR]: {ex.Message}");
            throw new DataAccessException($"An unexpected error occurred while retrieving facts.", ex);
        }
    }

    public async Task<CatFact> GetCatFactByIdAsync(int id)
    {
        try
        {
            await InitializeAsync();
            if (_database == null)
            {
                Debug.WriteLine($"[GetCatFactByIdAsync ERROR]: Database not initialized (ID: {id})");
                throw new InvalidOperationException("Database service is not properly initialized.");
            }

            return await _database.Table<CatFact>().Where(f => f.Id == id).FirstOrDefaultAsync();
        }
        catch (SQLiteException ex)
        {
            Debug.WriteLine($"[GetCatFactByIdAsync SQLITE ERROR]: {ex.Message} (ID: {id})");
            throw new DataAccessException($"A database error occurred while retrieving fact with ID {id}.", ex);
        }
        catch (InvalidOperationException) 
        {
            throw;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[GetCatFactByIdAsync GENERAL ERROR]: {ex.Message} (ID: {id})");
            throw new DataAccessException($"An unexpected error occurred while retrieving fact with ID {id}.", ex);
        }
    }

    public async Task<List<CatFact>> GetFavoriteFactsAsync()
    {
        try
        {
            await InitializeAsync();
            if (_database == null)
            {
                Debug.WriteLine($"[GetFavoriteFactsAsync ERROR]: Database not initialized");
                throw new InvalidOperationException("Database service is not properly initialized.");
            }

            return await _database.Table<CatFact>().Where(f => f.IsFavorite).OrderByDescending(f => f.CreatedAt).ToListAsync();
        }
        catch (SQLiteException ex)
        {
            Debug.WriteLine($"[GetFavoriteFactsAsync SQLITE ERROR]: {ex.Message}");
            throw new DataAccessException("A database error occurred while retrieving facts.", ex);
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[GetFavoriteFactsAsync GENERAL ERROR]: {ex.Message}");
            throw new DataAccessException($"An unexpected error occurred while retrieving facts.", ex);
        }
    }

    public async Task<int> SaveFactAsync(CatFact fact)
    {
        ArgumentNullException.ThrowIfNull(fact);

        try
        {
            await InitializeAsync();
            if (_database == null)
            {
                Debug.WriteLine($"[SaveFactAsync ERROR]: Database not initialized");
                throw new InvalidOperationException("Database service is not properly initialized.");
            }

            if (fact.Id != 0)
            {
                Debug.WriteLine($"[SaveFactAsync UPDATING]: ID {fact.Id}");
                return await _database.UpdateAsync(fact);
            }
            else
            {
                Debug.WriteLine($"[SaveFactAsync INSERTING]: Content '{fact.Fact}'"); 
                return await _database.InsertAsync(fact);
            }
        }
        catch (SQLiteException ex)
        {
            Debug.WriteLine($"[SaveFactAsync SQLITE ERROR]: {ex.Message} (Fact ID: {fact.Id}, Content: {fact.Fact})");
            throw new DataAccessException($"Failed to save fact due to a database error. ID: {fact.Id}", ex);
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[SaveFactAsync GENERAL ERROR]: {ex.Message} (Fact ID: {fact.Id}, Content: {fact.Fact})");
            throw new DataAccessException($"An unexpected error occurred while saving fact. ID: {fact.Id}", ex);
        }
    }

    public async Task<int> DeleteFactAsync(CatFact fact)
    {
        ArgumentNullException.ThrowIfNull(fact);

        try
        {
            await InitializeAsync();
            if (_database == null)
            {
                Debug.WriteLine($"[DeleteFactAsync ERROR]: Database not initialized");
                throw new InvalidOperationException("Database service is not properly initialized.");
            }

            Debug.WriteLine($"[DeleteFactAsync]: ID {fact.Id}");
            return await _database.DeleteAsync(fact);
        }
        catch (SQLiteException ex)
        {
            Debug.WriteLine($"[DeleteFactAsync SQLITE ERROR]: {ex.Message} (Fact ID: {fact.Id})");
            throw new DataAccessException($"Failed to delete fact due to a database error. ID: {fact.Id}", ex);
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[DeleteFactAsync GENERAL ERROR]: {ex.Message} (Fact ID: {fact.Id})");
            throw new DataAccessException($"An unexpected error occurred while deleting fact. ID: {fact.Id}", ex);
        }
    }

    public async Task SetCatFactAsFavoriteAsync(int id)
    {
        try
        {
            var fact = await GetCatFactByIdAsync(id); 
            if (fact != null)
            {
                fact.IsFavorite = !fact.IsFavorite;
                Debug.WriteLine($"[SetCatFactAsFavoriteAsync]: ID {fact.Id}, New IsFavorite: {fact.IsFavorite}");
                await SaveFactAsync(fact); 
            }
            else
            {
                Debug.WriteLine($"[SetCatFactAsFavoriteAsync WARNING]: Fact with ID {id} not found or failed to load.");
            }
        }
        catch (DataAccessException ex) 
        {
            Debug.WriteLine($"[SetCatFactAsFavoriteAsync DATA ACCESS ERROR]: {ex.Message} (ID: {id})");
            throw;
        }
        catch (ArgumentNullException ex)
        {
            Debug.WriteLine($"[SetCatFactAsFavoriteAsync ARGUMENT ERROR]: {ex.Message} (ID: {id})");
            throw new DataAccessException($"An error occurred while setting favorite status for ID {id}.", ex);
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex) 
        {
            Debug.WriteLine($"[SetCatFactAsFavoriteAsync GENERAL ERROR]: {ex.Message} (ID: {id})");
            throw new DataAccessException($"An unexpected error occurred while setting favorite status for ID {id}.", ex);
        }
    }

}
