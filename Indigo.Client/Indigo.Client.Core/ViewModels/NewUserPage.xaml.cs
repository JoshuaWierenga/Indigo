using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Indigo.Client.Core.ViewModels
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewUserPage : ContentPage
	{
		public NewUserPage ()
		{
			InitializeComponent ();
		}
	}
}