using Indigo.Client.Core.ViewModels;
using Indigo.Core.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Indigo.Client.Core.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConversationPage : ContentPage
	{
		ConversationViewModel viewModel;

		public ConversationPage(User existingUser, Conversation existingConversation = null)
		{
			InitializeComponent();

			BindingContext = viewModel = new ConversationViewModel(existingUser, existingConversation);
		}

		async void Save_Clicked(object sender, EventArgs e)
		{
			
		}
	}
}