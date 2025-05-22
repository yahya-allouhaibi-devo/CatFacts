using System.Diagnostics;
using CatFacts.Exceptions;
using CatFacts.Models;
using CatFacts.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CatFacts.ViewModels;

public partial class CatFactViewModel : ObservableObject
{
    private readonly ICatFactService _catFactService;
    private readonly ICatDataBaseService _catDataBaseService;

    [ObservableProperty] private string _catFactText = "";

    [ObservableProperty]
    private bool _isBusy = false;

    public CatFactViewModel(ICatFactService catFactService, ICatDataBaseService catDataBaseService)
    {
        _catFactService = catFactService;
        _catDataBaseService = catDataBaseService;
    }

    [RelayCommand]
    private async Task GetCatFactAsync(CancellationToken ct)
    {
        if (IsBusy) return;
        IsBusy = true;

        try
        {
            var catFact = await _catFactService.GetCatFactAsync(ct);

            if (catFact is null || string.IsNullOrWhiteSpace(catFact.Fact))
            {
                CatFactText = "Could not fetch cat fact. Try again";
            }
            else
            {
                CatFactText = catFact.Fact;

                var catFactToBeSaved = new CatFact
                {
                    Fact = catFact.Fact,
                    CreatedAt = DateTime.UtcNow,
                    Length = catFact.Length,
                    IsFavorite = false
                };

                await _catDataBaseService.SaveFactAsync(catFactToBeSaved);
                Debug.WriteLine("Fact saved to database");
            }
        }
        catch (HttpRequestException)
        {
            CatFactText = "Network error. Could not fetch new cat fact.";
        }
        catch (DataAccessException)
        {
            CatFactText = "Database error. Could not save the cat fact.";
        }
        catch (InvalidOperationException)
        {
            CatFactText = "Invalid operation error. Please try again.";
        }
        catch (Exception)
        {
            CatFactText = "An unexpected error occurred. Please try again.";
        }
        finally
        {
            IsBusy = false;
        }

    }
}
