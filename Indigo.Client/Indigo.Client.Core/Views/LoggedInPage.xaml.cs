using Indigo.Client.Core.ViewModels;
using Indigo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Indigo.Client.Core.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoggedInPage : ContentPage
	{
		TestLoggedIn viewModel;

		public LoggedInPage(User existingUser)
		{
			InitializeComponent();

			BindingContext = viewModel = new TestLoggedIn(existingUser);
		}
	}
}