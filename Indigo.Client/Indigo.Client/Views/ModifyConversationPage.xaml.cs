using Indigo.Client.ViewModels;
using Indigo.Core.Models;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Indigo.Client.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModifyConversationPage : ContentPage
	{
		ModifyConversationViewModel viewModel;

		public ModifyConversationPage(User existingUser, Conversation existingConversation = null)
		{
			InitializeComponent();

			BindingContext = viewModel = new ModifyConversationViewModel(existingUser, existingConversation);
		}

		async Task Save_ClickedAsync(object sender, EventArgs e)
		{
			await viewModel.SaveConversation();
		}

		async Task ChatType_ChangedAsync(object sender, ToggledEventArgs e)
		{
			if (!e.Value && viewModel.Partners.Count > 1)
			{
				viewModel.ChangeChatType(true);
				await DisplayAlert("Warning", "Can not switch chat type to private chat while chat contains more than 2 users", "ok");
			}
		}

		void AddPartner_Pressed(object sender, EventArgs e)
		{
			viewModel.AddPartner();
		}

		void Partner_Selected(object sender, SelectedItemChangedEventArgs e)
		{
			((ListView)sender).SelectedItem = null;
		}
	}
}