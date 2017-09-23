using Indigo.Client.Core.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Indigo.Client.Core
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			MainPage = new NavigationPage(new UsersPage())
			{
				Title = "Test",			
			};
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