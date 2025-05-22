using CatFacts.Views;
using CatFacts.Services;
using CatFacts.ViewModels;

namespace CatFacts.Configuration;

public static class Configuration
{
    public static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<ICatDataBaseService,CatDataBaseService>();
        mauiAppBuilder.Services.AddSingleton<ICatFactService, CatFactService>();
        mauiAppBuilder.Services.AddSingleton<HttpClient>(sp =>
        {
            var httpClient = new HttpClient();
            return httpClient;
        });

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<CatFactViewModel>();
        mauiAppBuilder.Services.AddTransient<CatFactsHistoryViewModel>();

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<MainPage>();
        mauiAppBuilder.Services.AddTransient<CatFactsHistory>();

        return mauiAppBuilder;
    }


}
