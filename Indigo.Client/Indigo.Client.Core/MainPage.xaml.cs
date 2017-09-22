using Indigo.Client.Core.Rest;
using Indigo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Indigo.Client.Core
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
		IIndigoApi Api = Refit.RestService.For<IIndigoApi>("http://localhost:52097/api");
		List<User> Users = new List<User>();

		public MainPage()
		{
			InitializeComponent();

			BindingContext = this;

			Users = Task.Run(() => Api.GetUsers()).Result;
			test.ItemsSource = Users;
		}

	}
}