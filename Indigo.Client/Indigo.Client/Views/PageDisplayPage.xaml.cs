using Indigo.Client.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using Indigo.Client.Rest;
using System.Threading.Tasks;

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
            await AttemptGetPage(pageName);
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
            //only allow editing or saving if not loading
            if (!viewModel.Loading)
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
                        await AttemptGetPage(viewModel.Page.Name);
                    }
                    //switch to edit button
                    editSaveButton.Text = "Edit Page";
                    editSaveButton.Icon = "ic_edit.png";
                    //switch from message editor to markdown viewer
                    markdownViewer.IsVisible = true;
                    pageEditor.IsVisible = false;
                }
            }           
        }

        /// <summary>
        /// Attempts to get page from server, if there are 10 failures then the user is alerted
        /// </summary>
        /// <param name="pagename">name of page to retrieve</param>
        async Task AttemptGetPage(string pagename)
        {
            //subscribe to connection issues
            int errorCount = 0;
            MessagingCenter.Subscribe<PageViewModel>(this, "connection error", (sender) =>
            {            
                //display error after 10 attempts
                if (errorCount < 10)
                {
                    errorCount++;
                }
                else if (errorCount == 10)
                {
                    errorCount++;
                    //alert user that there was a connection issue
                    DisplayAlert("Warning", "No connection to the server could be established, make sure you are connected to the internet", "ok");
                }               
            });

            //gets home page from database
            await viewModel.GetPageAsync(pagename);
            //unsubscribe to connection issues
            MessagingCenter.Unsubscribe<PageViewModel>(this, "connection error");
        }
        
        /// <summary>
        /// Runs on page appearing, gets home page from database
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await AttemptGetPage("home");        
        }
    }
}