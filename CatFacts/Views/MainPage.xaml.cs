using CatFacts.ViewModels;

namespace CatFacts.Views;

public partial class MainPage : ContentPage
{
    public MainPage(CatFactViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }


    //private void OnCounterClicked(object? sender, EventArgs e)
    //{
    //    count++;

    //    if (count == 1)
    //        CounterBtn.Text = $"Clicked {count} time";
    //    else
    //        CounterBtn.Text = $"Clicked {count} times";

    //    SemanticScreenReader.Announce(CounterBtn.Text);
    //}
}
