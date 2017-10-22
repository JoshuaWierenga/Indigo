using Indigo.Client.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;

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

        async void PageName_Changed(object sender, TextChangedEventArgs e)
        {
            string pageName = e.NewTextValue != "" ? e.NewTextValue : "home";
            await viewModel.GetPageAsync(pageName);
        }

        async void EditSave_Clicked(object sender, EventArgs e)
        {
            ToolbarItem editSaveButton = (ToolbarItem)sender;

            if (editSaveButton.Text == "Edit Page")
            {
                editSaveButton.Text = "Save Changes";
                editSaveButton.Icon = "ic_save.png";
                markdownViewer.IsVisible = false;
                pageEditor.IsVisible = true;
            }
            else
            {
                if (viewModel.PageMessage != viewModel.Page.Message)
                {
                    await viewModel.SavePageAsync();
                    await viewModel.GetPageAsync(viewModel.Page.Name);
                }
                
                editSaveButton.Text = "Edit Page";
                editSaveButton.Icon = "ic_edit.png";
                markdownViewer.IsVisible = true;
                pageEditor.IsVisible = false;                
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.GetPageAsync("home");
        }
    }
}