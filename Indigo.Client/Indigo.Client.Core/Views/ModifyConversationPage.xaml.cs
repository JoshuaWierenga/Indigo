using Indigo.Client.Core.ViewModels;
using Indigo.Core.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Indigo.Client.Core.Views
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

		async void Save_Clicked(object sender, EventArgs e)
		{
			await viewModel.SaveConversation();
		}
	}
}