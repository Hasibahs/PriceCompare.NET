using Microsoft.Maui.Controls;
using PriceComparisonApp.Models;
using PriceComparisonApp.Services;

namespace PriceComparisonApp.Views
{
    public partial class SummaryPage : ContentPage
    {
        public SummaryPage(List<ProductResult> products)
        {
            InitializeComponent();
            LoadSummary(products);
        }

        private async void LoadSummary(List<ProductResult> products)
        {
            var summary = await ProductAnalyzer.AnalyzeAsync(products);
            SummaryCollectionView.ItemsSource = summary;
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
