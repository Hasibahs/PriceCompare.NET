using Microsoft.Maui.Controls;
using PriceComparisonApp.Views;

namespace PriceComparisonApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Start the app with the SearchPage wrapped in a NavigationPage
            MainPage = new NavigationPage(new SearchPage());
        }
    }
}
