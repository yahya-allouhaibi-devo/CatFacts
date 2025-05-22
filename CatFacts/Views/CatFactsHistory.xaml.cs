using CatFacts.ViewModels;

namespace CatFacts.Views;

public partial class CatFactsHistory : ContentPage
{
    private readonly CatFactsHistoryViewModel _viewModel;

    public CatFactsHistory(CatFactsHistoryViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel != null)
        {
           await _viewModel.OnAppearingAsync();
        }
    }
}