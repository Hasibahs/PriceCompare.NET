using Microsoft.Maui.Controls;
using PriceComparisonApp.Models;
using PriceComparisonApp.Services;

namespace PriceComparisonApp.Views
{
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            InitializeComponent();
        }

        private async void OnCompareClicked(object sender, EventArgs e)
        {
            var items = shoppingListEditor.Text;

            if (string.IsNullOrWhiteSpace(items))
            {
                await DisplayAlert("Missing input", "Please enter your grocery list first.", "OK");
                return;
            }

            await Navigation.PushAsync(new ResultsPage(await BackendService.SearchProductsAsync(items, null)));
        }
    }
}
