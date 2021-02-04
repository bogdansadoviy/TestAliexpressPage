using TestAliexpressPage.Entities;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TestAliexpressPage.Views.Dialogs
{
    public sealed partial class NewBookmarkDialog : ContentDialog
    {
        private readonly Bookmark _bookmark;

        public NewBookmarkDialog(Bookmark bookmark)
        {
            this.InitializeComponent();

            _bookmark = bookmark;
            BookmarkName.Text = bookmark.Name;
            BookmarkUrl.Text = bookmark.Url;
        }

        private void SaveButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            _bookmark.Name = BookmarkName.Text;
        }

        private void CancelButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Hide();
        }
    }
}
