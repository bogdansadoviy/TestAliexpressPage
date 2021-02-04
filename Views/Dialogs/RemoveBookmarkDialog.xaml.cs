using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TestAliexpressPage.Views.Dialogs
{
    public sealed partial class RemoveBookmarkDialog : ContentDialog
    {
        public RemoveBookmarkDialog(string bookmarkName)
        {
            this.InitializeComponent();
            BookmarkName.Text = $"Remove bookmark '{bookmarkName}'?";
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Hide();
        }
    }
}
