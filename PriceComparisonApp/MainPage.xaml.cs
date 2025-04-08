using PriceComparisonApp.Models;
using PriceComparisonApp.Services;

namespace PriceComparisonApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnSearchButtonPressed(object sender, EventArgs e)
    {
        string query = ProductSearchBar.Text ?? "";
        var results = await BackendService.SearchProductsAsync(query);
        ProductResultsView.ItemsSource = results;
    }

    private async void OnProductTitleTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is string url && !string.IsNullOrWhiteSpace(url))
        {
            try
            {
                await Launcher.Default.OpenAsync(new Uri(url));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Could not open link: {ex.Message}", "OK");
            }
        }
    }
}
