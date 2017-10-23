using Indigo.Client.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Indigo.Client
{
    /// <summary>
    /// Handles creating application and displaying UI
    /// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //Creates Page Viewer and displays within a navigation page so that toolbar is visible
            MainPage = new NavigationPage(new PageDisplayPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}