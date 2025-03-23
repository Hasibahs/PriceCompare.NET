using Microsoft.Maui.Controls;
using PriceComparisonApp.Models;

namespace PriceComparisonApp.Views
{
    public partial class ResultsPage : ContentPage
    {
        public ResultsPage(List<ProductResult> results)
        {
            InitializeComponent();
            resultsCollectionView.ItemsSource = results;
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}