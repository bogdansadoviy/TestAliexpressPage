using System;
using System.Collections.Generic;
using System.Linq;
using TestAliexpressPage.Configuration;
using TestAliexpressPage.Entities;

namespace TestAliexpressPage.Services
{
    public class BookmarkService
    {
        private readonly LocalFileConfiguration<AppConfiguration> _localFileConfiguration =
             new LocalFileConfiguration<AppConfiguration>();

        public IEnumerable<Bookmark> Bookmarks { get; private set; }

        public BookmarkService()
        {
            Bookmarks = _localFileConfiguration.Get().Bookmarks;
        }

        public Bookmark GetBookmarkByUri(Uri uri)
        {
            return Bookmarks.FirstOrDefault(_ => _.Url == uri.ToString());
        }

        public Bookmark GetBookmarkById(int id)
        {
            return Bookmarks.FirstOrDefault(_ => _.Id == id);
        }

        public Bookmark AddBookmark(Bookmark newBookmark)
        {
            var currentMaxIndex = 0;
            if (Bookmarks.Any())
            {
                currentMaxIndex = Bookmarks.Max(_ => _.Id);
            }
            newBookmark.Id = currentMaxIndex + 1;

            var newBookmarks = new List<Bookmark>(Bookmarks)
            {
                newBookmark
            };
            Bookmarks = newBookmarks;

            _localFileConfiguration.Save(new AppConfiguration()
            {
                Bookmarks = newBookmarks
            });

            return newBookmark;
        }

        public int RemoveBookmark(string uri)
        {
            var bookmark = Bookmarks.First(_ => _.Url == uri);
            var removedBookmarkIndex = Bookmarks.ToList().IndexOf(bookmark);

            var newBookmarks = new List<Bookmark>(Bookmarks);
            newBookmarks.Remove(bookmark);

            Bookmarks = newBookmarks;

            _localFileConfiguration.Save(new AppConfiguration()
            {
                Bookmarks = newBookmarks
            });

            return removedBookmarkIndex;
        }
    }
}
