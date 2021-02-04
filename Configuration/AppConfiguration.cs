using System.Collections.Generic;
using TestAliexpressPage.Entities;

namespace TestAliexpressPage.Configuration
{
    public class AppConfiguration
    {
        public AppConfiguration()
        {
            Bookmarks = new List<Bookmark>();
        }

        public IEnumerable<Bookmark> Bookmarks { get; set; }
    }
}
