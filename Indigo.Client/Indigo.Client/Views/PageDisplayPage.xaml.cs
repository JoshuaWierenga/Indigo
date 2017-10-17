using Indigo.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Indigo.Client.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PageDisplayPage : ContentPage
	{
        PageViewModel viewModel;

		public PageDisplayPage()
		{
			InitializeComponent();

            BindingContext = viewModel = new PageViewModel();
		}
	}
}