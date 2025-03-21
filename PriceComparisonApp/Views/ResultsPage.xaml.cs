using Microsoft.Maui.Controls;
using System.Collections.Generic;
using PriceComparisonApp.Models; // ProductResult model

namespace PriceComparisonApp.Views
{
    public partial class ResultsPage : ContentPage
    {
        public ResultsPage(List<ProductResult> results)
        {
            InitializeComponent();

            // Bind the incoming results to the CollectionView
            resultsCollectionView.ItemsSource = results;
        }
    }
}
