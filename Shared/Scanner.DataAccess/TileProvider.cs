using Scanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.StartScreen;

namespace Scanner.DataAccess
{
    public sealed class TileProvider
    {
        private static readonly Lazy<TileProvider> lazy =
            new Lazy<TileProvider>(() => new TileProvider(), true);

        public TileProvider()
        {

        }

        public static TileProvider Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public async Task<bool> PinDocumentTile(Document document)
        {
            string tileId = document.ID.ToString();

            string arguements = tileId + "/" + "Document";

            string displayName = document.DocumentName;

            bool isPinned = false;

            if (!SecondaryTile.Exists(tileId))
            {
                var secondaryTile = new SecondaryTile();

                secondaryTile.TileId = tileId;

                secondaryTile.ShortName = "2nd tile";

                secondaryTile.DisplayName = displayName;

                secondaryTile.Arguments = arguements;

                secondaryTile.Logo = new Uri("ms-appdata:///local/картинка031.jpg");

                isPinned = await secondaryTile.RequestCreateAsync();

                return isPinned;
            }

            return isPinned;
        }

        public async Task UnPinDocumentTile(string tileId)
        {
            var secondaryTile = new SecondaryTile(tileId);

            await secondaryTile.RequestDeleteAsync();
        }

        public async Task<bool> PinCategoryTile(Category category)
        {
            string tileId = category.ID.ToString();

            string arguements = tileId + "/" + "Category";

            string displayName = category.CategoryName;

            bool isPinned = false;

            if (!SecondaryTile.Exists(tileId))
            {
                var secondaryTile = new SecondaryTile();

                secondaryTile.TileId = tileId;

                secondaryTile.ShortName = "2nd tile";

                secondaryTile.DisplayName = displayName;

                secondaryTile.Arguments = arguements;

                secondaryTile.Logo = new Uri("ms-appdata:///local/картинка031.jpg");

                isPinned = await secondaryTile.RequestCreateAsync();

                return isPinned;
            }

            return isPinned;
        }

        public async Task UnPinCategoryTile(string tileId)
        {
            var secondaryTile = new SecondaryTile(tileId);

            await secondaryTile.RequestDeleteAsync();
        }

        public bool TileExist(string tileId)
        {
            if (SecondaryTile.Exists(tileId))
            {
                return true;
            }

            return false;
        }
    }
}
