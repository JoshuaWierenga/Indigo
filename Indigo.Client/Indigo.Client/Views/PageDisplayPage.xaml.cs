using Indigo.Client.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Indigo.Client.Views
{
    /// <inheritdoc />
    /// <summary>
    /// Handles displaying page to user and receiving user input
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageDisplayPage
    {
        /// <summary>
        /// Provides access to view model which allows access to page and functions to get and save pages to the database
        /// </summary>
        private readonly PageViewModel _viewModel;

        /// <summary>
        /// Creates UI and binds to viewModel
        /// </summary>
        public PageDisplayPage()
        {
            //Creates UI
            InitializeComponent();

            //Creates ViewModel and binds UI to it
            BindingContext = _viewModel = new PageViewModel();
        }

        /// <summary>
        /// Runs on page name changing, gets page matching name if it is set otherwises gets home page
        /// </summary>
        /// <param name="sender">Object that called this function</param>
        /// <param name="e">Infomation about text being changed</param>
        private async void PageName_Changed(object sender, TextChangedEventArgs e)
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
        private async void EditSave_Clicked(object sender, EventArgs e)
        {
            //only allow editing or saving if not loading
            if (_viewModel.Loading) return;
            //Get the toolbar button that caused this event
            var editSaveButton = (ToolbarItem)sender;

            //check if edit button was pressed
            if (editSaveButton.Text == "Edit Page")
            {
                //switch to save button
                editSaveButton.Text = "Save Changes";
                editSaveButton.Icon = "ic_save.png";
                //switch from markdown viewer to message editor
                MarkdownViewer.IsVisible = false;
                PageEditor.IsVisible = true;
            }
            else
            {
                //check if message has been changed
                if (_viewModel.PageMessage != _viewModel.Page.Message)
                {
                    //saves message to database and update viewmodel
                    await _viewModel.SavePageAsync();
                    await AttemptGetPage(_viewModel.Page.Name);
                }
                //switch to edit button
                editSaveButton.Text = "Edit Page";
                editSaveButton.Icon = "ic_edit.png";
                //switch from message editor to markdown viewer
                MarkdownViewer.IsVisible = true;
                PageEditor.IsVisible = false;
            }
        }

        /// <summary>
        /// Attempts to get page from server, if there are 10 failures then the user is alerted
        /// </summary>
        /// <param name="pagename">name of page to retrieve</param>
        private async Task AttemptGetPage(string pagename)
        {
            //subscribe to connection issues
            var errorCount = 0;
            MessagingCenter.Subscribe<PageViewModel>(this, "connection error", sender =>
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
            await _viewModel.GetPageAsync(pagename);
            //unsubscribe to connection issues
            MessagingCenter.Unsubscribe<PageViewModel>(this, "connection error");
        }
        
        /// <inheritdoc />
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