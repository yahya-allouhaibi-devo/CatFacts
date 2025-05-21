using CatFacts.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CatFacts.ViewModels;

public partial class CatFactViewModel : ObservableObject
{
    private readonly ICatFactService _catFactService;

    [ObservableProperty] private string _catFactText = "";

    public CatFactViewModel(ICatFactService catFactService)
    {
        _catFactService = catFactService;
    }

    [RelayCommand]
    private async Task GetCatFactAsync(CancellationToken ct)
    {
        var catFact =  await _catFactService.GetCatFactAsync(ct);

        if (catFact is null || catFact.Fact is null)
        {
            CatFactText = "Could not fetch cat fact";
        }
        else
        {
           CatFactText = catFact.Fact;
        }
    }
}
