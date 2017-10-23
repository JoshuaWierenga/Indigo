using Indigo.Client.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;

namespace Indigo.Client.Views
{
    /// <summary>
    /// Handles displaying page to user and receiving user input
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageDisplayPage : ContentPage
    {
        /// <summary>
        /// Provides access to view model which allows access to page and functions to get and save pages to the database
        /// </summary>
        PageViewModel viewModel;

        /// <summary>
        /// Creates UI and binds to viewModel
        /// </summary>
        public PageDisplayPage()
        {
            //Creates UI
            InitializeComponent();

            //Creates ViewModel and binds UI to it
            BindingContext = viewModel = new PageViewModel();
        }

        /// <summary>
        /// Runs on page name changing, gets page matching name if it is set otherwises gets home page
        /// </summary>
        /// <param name="sender">Object that called this function</param>
        /// <param name="e">Infomation about text being changed</param>
        async void PageName_Changed(object sender, TextChangedEventArgs e)
        {
            //check if page name is blank, if it is use home instead
            string pageName = e.NewTextValue != "" ? e.NewTextValue : "home";
            //gets page from database
            await viewModel.GetPageAsync(pageName);
        }
        
        /// <summary>
        /// Runs on the edit or save buttons being clicked, if edit then switches to save button and switches from
        /// markdown viewer to message editor, if save then switches to edit button, switches from message editor
        /// to markdown viewer and if message has changed then sends to database
        /// </summary>
        /// <param name="sender">Object that called this function</param>
        /// <param name="e">Infomation about button being clicked</param>
        async void EditSave_Clicked(object sender, EventArgs e)
        {
            //Get the toolbar button that caused this event
            ToolbarItem editSaveButton = (ToolbarItem)sender;

            //check if edit button was pressed
            if (editSaveButton.Text == "Edit Page")
            {
                //switch to save button
                editSaveButton.Text = "Save Changes";
                editSaveButton.Icon = "ic_save.png";
                //switch from markdown viewer to message editor
                markdownViewer.IsVisible = false;
                pageEditor.IsVisible = true;
            }
            else
            {
                //check if message has been changed
                if (viewModel.PageMessage != viewModel.Page.Message)
                {
                    //saves message to database and update viewmodel
                    await viewModel.SavePageAsync();
                    await viewModel.GetPageAsync(viewModel.Page.Name);
                }
                //switch to edit button
                editSaveButton.Text = "Edit Page";
                editSaveButton.Icon = "ic_edit.png";
                //switch from message editor to markdown viewer
                markdownViewer.IsVisible = true;
                pageEditor.IsVisible = false;                
            }
        }

        /// <summary>
        /// Runs on page appearing, gets home page from database
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            //gets home page from database
            await viewModel.GetPageAsync("home");
        }
    }
}