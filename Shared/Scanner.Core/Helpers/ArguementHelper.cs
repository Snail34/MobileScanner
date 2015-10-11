using Scanner.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanner.Core.Helpers
{
    public class ArguementHelper
    {
        public class LaunchAppArguements
        {
            public Guid TileId { get; set; }

            public string PageDestinationType { get; set; }
        }

        public static LaunchAppArguements ParseArguementString(string arguements)
        {
            Guid tileId = Guid.Empty;

            var launchArguements = new LaunchAppArguements();

            string[] s = arguements.Split('/');

            if (Guid.TryParse(s[0], out tileId))
            {
                if (tileId != Guid.Empty)
                {
                    launchArguements.TileId = tileId;

                    if (s[1].Equals("Document"))
                        //service.NavigateTo(NavigationSource.DocumentView.ToString(), tileId);
                        launchArguements.PageDestinationType = NavigationSource.DocumentView.ToString();
                    else
                        //service.NavigateTo(NavigationSource.CurrentCategoryView.ToString(), tileId);
                        launchArguements.PageDestinationType = NavigationSource.CurrentCategoryView.ToString();
                }
            }

            return launchArguements;
        }
    }
}
