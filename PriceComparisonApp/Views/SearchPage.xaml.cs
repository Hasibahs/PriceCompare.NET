using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;
using PriceComparisonApp.Services;   // For BackendService
using PriceComparisonApp.Models;     // For ProductResult
using System.Collections.Generic;

namespace PriceComparisonApp.Views
{
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            InitializeComponent();
        }

        private async void OnSearchButtonClicked(object sender, EventArgs e)
        {
            string productName = searchEntry.Text?.Trim();
            string selectedStore = storePicker.SelectedItem as string;

            // Basic validation
            if (string.IsNullOrWhiteSpace(productName))
            {
                await DisplayAlert("Input Error", "Please enter a valid product name.", "OK");
                return;
            }

            try
            {
                // Call your async method from the BackendService
                List<ProductResult> results = await BackendService.SearchProductsAsync(productName, selectedStore);

                if (results == null || results.Count == 0)
                {
                    await DisplayAlert("No Results", "No matching products found.", "OK");
                    return;
                }

                // Navigate to the ResultsPage, passing the product list
                await Navigation.PushAsync(new ResultsPage(results));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}
