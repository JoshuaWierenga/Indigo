using Indigo.Client.Core.ViewModels;
using Indigo.Core.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Indigo.Client.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConversationsPage : ContentPage
    {
		ConversationsViewModel viewModel;

        public ConversationsPage(User existingUser)
        {
            InitializeComponent();

			BindingContext = viewModel = new ConversationsViewModel(existingUser);
		}

		async void Create_Pressed(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new ModifyConversationPage(viewModel.User));
		}

		async void Delete_Pressed(object sender, EventArgs e)
		{
			MenuItem menuItem = (MenuItem)sender;
			UserConversation selectedUserConversation = (UserConversation)menuItem.CommandParameter;

			await viewModel.DeleteUserConversationAsync(selectedUserConversation.Conversation);
		}
	}
}