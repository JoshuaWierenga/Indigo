using Indigo.Client.Core.ViewModels;
using Indigo.Core.Models;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Indigo.Client.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModifyConversationPage : ContentPage
    {
        ModifyConversationViewModel viewModel;

        public static Command RemovePartner_Pressed { get; protected set; }

        public ModifyConversationPage(User existingUser, Conversation existingConversation = null)
        {
            InitializeComponent();

            BindingContext = viewModel = new ModifyConversationViewModel(existingUser, existingConversation);

            RemovePartner_Pressed = new Command<UserConversation>(async uc =>
            {
                string error = "Can not remove users from a private chat";
                if (viewModel.ChatType)
                {
                    if (viewModel.Partners.Count > 1)
                    {
                        viewModel.Partners.Remove(uc);
                        return;
                    }
                    error = "Group chats must contain atleast two users";

                }
                await DisplayAlert("Warning", error, "ok");
            });
        }

        async Task Save_ClickedAsync(object sender, EventArgs e)
        {
            if (viewModel.CheckEntered())
            {
                await viewModel.SaveConversation();
                return;
            }
            await DisplayAlert("Warning", "Please check that everything has been filled in correctly", "ok");
        }

        async Task ChatType_ChangedAsync(object sender, ToggledEventArgs e)
        {
            if (!e.Value && viewModel.Partners.Count > 1)
            {
                viewModel.ChatType = true;
                await DisplayAlert("Warning", "Can not switch chat type to private chat while chat contains more than 2 users", "ok");
            }
        }

        void AddPartner_PressedAsync(object sender, EventArgs e)
        {
            viewModel.AddPartner();
        }

        void Partner_Selected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}