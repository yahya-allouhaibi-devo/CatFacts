using CatFacts.ViewModels;

namespace CatFacts.Views;

public partial class MainPage : ContentPage
{
    public MainPage(CatFactViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
