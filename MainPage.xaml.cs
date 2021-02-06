using Prism.Commands;
using System;
using System.Linq;
using TestAliexpressPage.Entities;
using TestAliexpressPage.Services;
using TestAliexpressPage.Views.Dialogs;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TestAliexpressPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly BookmarkService _bookmarkService = new BookmarkService();

        private readonly string[] _ignorableHosts = new string[] { "play.google.com", "itunes.apple.com" };
        private readonly string[] _ignorableLocalPathes = new string[] { "/download_app_guide.htm" };

        public MainPage()
        {
            this.InitializeComponent();

            WebViewBrowser.NavigationStarting += WebViewBrowser_NavigationStarting;
            WebViewBrowser.NewWindowRequested += WebViewBrowser_NewWindowRequested; ;

            foreach (var bookmark in _bookmarkService.Bookmarks)
            {
                BookmarkMenu.Items.Add(CreateMenuFlyoutItem(bookmark));
            }
        }

        private void WebViewBrowser_NewWindowRequested(WebView sender, WebViewNewWindowRequestedEventArgs args)
        {
            if (!_ignorableHosts.Contains(args.Uri.Host))
            {
                sender.Navigate(args.Uri);
            }
            args.Handled = true;
        }

        private void WebViewBrowser_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            if (_ignorableLocalPathes.Contains(args.Uri.LocalPath))
            {
                args.Cancel = true;
                return;
            }

            if (_bookmarkService.GetBookmarkByUri(args.Uri) != null)
            {
                AddBookmarkFontIcon.Glyph = "\uE735";
                AddBookmarkButton.Click -= Add_Bookmark_Button_Click;
                AddBookmarkButton.Click -= Remove_Bookmark_Button_Click;
                AddBookmarkButton.Click += Remove_Bookmark_Button_Click;
            }
            else
            {
                AddBookmarkFontIcon.Glyph = "\uE734";
                AddBookmarkButton.Click -= Remove_Bookmark_Button_Click;
                AddBookmarkButton.Click -= Add_Bookmark_Button_Click;
                AddBookmarkButton.Click += Add_Bookmark_Button_Click;
            }
        }

        private void Navigation_Back_Button_Click(object sender, RoutedEventArgs e)
        {
            if (WebViewBrowser.CanGoBack)
            {
                WebViewBrowser.GoBack();
            }
        }

        private void Navigation_Forward_Button_Click(object sender, RoutedEventArgs e)
        {
            if (WebViewBrowser.CanGoForward)
            {
                WebViewBrowser.GoForward();
            }
        }

        private void Refresh_Page_Button_Click(object sender, RoutedEventArgs e)
        {
            WebViewBrowser.Refresh();
        }

        private async void Add_Bookmark_Button_Click(object sender, RoutedEventArgs e)
        {
            var newBookmark = new Bookmark()
            {
                Name = WebViewBrowser.DocumentTitle,
                Url = WebViewBrowser.Source.ToString()
            };

            var dialog = new NewBookmarkDialog(newBookmark);
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                UpdateBookmark(newBookmark);

                AddBookmarkFontIcon.Glyph = "\uE735";
                AddBookmarkButton.Click -= Add_Bookmark_Button_Click;
                AddBookmarkButton.Click += Remove_Bookmark_Button_Click;
            }
        }

        private async void Remove_Bookmark_Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new RemoveBookmarkDialog(WebViewBrowser.DocumentTitle);
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                RemoveBookmark(WebViewBrowser.Source.ToString());

                AddBookmarkFontIcon.Glyph = "\uE734";
                AddBookmarkButton.Click -= Remove_Bookmark_Button_Click;
                AddBookmarkButton.Click += Add_Bookmark_Button_Click;
            }
        }

        private void UpdateBookmark(Bookmark newBookmark)
        {
            _bookmarkService.AddBookmark(newBookmark);
            BookmarkMenu.Items.Add(CreateMenuFlyoutItem(newBookmark));
        }

        private void RemoveBookmark(string bookmarkUrl)
        {
            var removedBookmarkIndex = _bookmarkService.RemoveBookmark(bookmarkUrl);
            BookmarkMenu.Items.RemoveAt(removedBookmarkIndex);
        }

        private MenuFlyoutItem CreateMenuFlyoutItem(Bookmark bookmark)
        {
            var menuFlyoutItem = new MenuFlyoutItem()
            {
                Text = bookmark.Name,
                Command = BookmarkClickCommand,
                CommandParameter = bookmark.Id.ToString(),
            };

            var toolTip = new ToolTip
            {
                Content = bookmark.Url
            };
            ToolTipService.SetToolTip(menuFlyoutItem, toolTip);

            return menuFlyoutItem;
        }

        private DelegateCommand<string> _bookmarkClickCommand;
        public DelegateCommand<string> BookmarkClickCommand
        {
            get
            {
                return _bookmarkClickCommand ??
                  (_bookmarkClickCommand = new DelegateCommand<string>(
                      obj => {
                          var bookmarkId = int.Parse(obj);
                          var bookmark = _bookmarkService.GetBookmarkById(bookmarkId);

                          WebViewBrowser.Navigate(new Uri(bookmark.Url));
                      }
                  ));
            }
        }
    }

}
