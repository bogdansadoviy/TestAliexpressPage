using System.Collections.Generic;
using TestAliexpressPage.Entities;

namespace TestAliexpressPage.Configuration
{
    public interface IAppConfiguration
    {
        IEnumerable<Bookmark> Bookmarks { get; set; }
    }
    public class AppConfiguration : IAppConfiguration
    {
        public AppConfiguration()
        {
            Bookmarks = new List<Bookmark>();
        }

        public IEnumerable<Bookmark> Bookmarks { get; set; }
    }
}
