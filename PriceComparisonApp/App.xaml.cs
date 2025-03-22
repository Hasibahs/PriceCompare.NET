using Microsoft.Maui.Controls;
using PriceComparisonApp.Views;

namespace PriceComparisonApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new SearchPage());
        }
    }
}