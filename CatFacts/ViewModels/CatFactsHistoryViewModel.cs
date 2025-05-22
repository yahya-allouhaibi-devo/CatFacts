using System.Collections.ObjectModel;
using System.Diagnostics;
using CatFacts.Exceptions;
using CatFacts.Models;
using CatFacts.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CatFacts.ViewModels;

public partial class CatFactsHistoryViewModel: ObservableObject
{
    private readonly ICatDataBaseService _catDataBaseService;

    [ObservableProperty]
    private ObservableCollection<CatFact> _catFacts;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private bool _isEmpty;


    public CatFactsHistoryViewModel(ICatDataBaseService catDataBaseService)
    {
        _catDataBaseService = catDataBaseService;
        _catFacts = new ObservableCollection<CatFact>();
    }

    public async Task OnAppearingAsync()
    {
        await LoadCatFactsAsync();
    }

    [RelayCommand]
    private async Task LoadCatFactsAsync()
    {
        if (IsLoading)
            return;

        IsLoading = true;
        CatFacts.Clear();

        try
        {
            var facts = await _catDataBaseService.GetCatFactsAsync();
            if (facts != null)
            {
                if(facts.Count == 0)
                {
                    IsEmpty = true;
                }

                foreach (var fact in facts)
                {
                    CatFacts.Add(fact);
                }
            }
        }
        catch (DataAccessException ex)
        {
            Debug.WriteLine($"[LoadCatFactsAsync DATA ERROR]: {ex.Message}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[LoadCatFactsAsync GENERAL ERROR]: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }
}
