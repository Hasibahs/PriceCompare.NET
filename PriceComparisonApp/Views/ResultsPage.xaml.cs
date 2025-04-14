using PriceComparisonApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;

namespace PriceComparisonApp.Views
{
    public partial class ResultsPage : ContentPage
    {
        // Separate lists for each store's products.
        private List<ProductResult> jumboResults;
        private List<ProductResult> aldiResults;
        private List<ProductResult> ahResults;

        private int currentPage = 0;
        private const int PageSize = 5;

        public ResultsPage(List<ProductResult> results)
        {
            InitializeComponent();

            // Filter results by store name (case‑insensitive).
            jumboResults = results.Where(r => r.Store.Equals("Jumbo", StringComparison.OrdinalIgnoreCase)).ToList();
            aldiResults = results.Where(r => r.Store.Equals("Aldi", StringComparison.OrdinalIgnoreCase)).ToList();
            ahResults = results.Where(r => r.Store.Equals("ah", StringComparison.OrdinalIgnoreCase)).ToList();

            UpdatePage();
        }

        // Update each column's CollectionView with paginated data.
        private void UpdatePage()
        {
            var jumboPage = jumboResults.Skip(currentPage * PageSize).Take(PageSize).ToList();
            var aldiPage = aldiResults.Skip(currentPage * PageSize).Take(PageSize).ToList();
            var ahPage = ahResults.Skip(currentPage * PageSize).Take(PageSize).ToList();

            JumboCollectionView.ItemsSource = jumboPage;
            AldiCollectionView.ItemsSource = aldiPage;
            AHCollectionView.ItemsSource = ahPage;

            // Calculate total pages for each store and take the maximum.
            int jumboPages = (int)Math.Ceiling(jumboResults.Count / (double)PageSize);
            int aldiPages = (int)Math.Ceiling(aldiResults.Count / (double)PageSize);
            int ahPages = (int)Math.Ceiling(ahResults.Count / (double)PageSize);
            int totalPages = Math.Max(jumboPages, Math.Max(aldiPages, ahPages));

            pageLabel.Text = $"Page {currentPage + 1} of {totalPages}";

            previousButton.IsEnabled = currentPage > 0;
            nextButton.IsEnabled = (currentPage + 1) < totalPages;
        }

        private void OnNextClicked(object sender, EventArgs e)
        {
            currentPage++;
            UpdatePage();
        }

        private void OnPreviousClicked(object sender, EventArgs e)
        {
            if (currentPage > 0)
            {
                currentPage--;
                UpdatePage();
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        // ✅ New: Click handler for product title
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
                    await DisplayAlert("Error", $"Unable to open link: {ex.Message}", "OK");
                }
            }
            else
            {
                await DisplayAlert("Missing Link", "This product has no valid link.", "OK");
            }
        }

        private async void OnViewSummaryClicked(object sender, EventArgs e)
        {
            var allProducts = jumboResults.Concat(aldiResults).Concat(ahResults).ToList();
            await Navigation.PushAsync(new SummaryPage(allProducts));
        }

    }
}
