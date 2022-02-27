
using System;
using System.Security.Policy;
using dashboard_elite.EliteData;
using Microsoft.AspNetCore.Components;

namespace dashboard_elite.Shared
{
    public partial class NavMenu
    {
        [CascadingParameter] private RouteData RouteData { get; set; }

        [Parameter]
        public string NavSection { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] public Engineer Engineer { get; set; }

        protected override void OnParametersSet()
        {
            var uri = new Uri(NavigationManager.Uri);

            if (uri.Segments.Length > 1)
            {
                NavSection = uri.Segments[1].Replace("/", "");

                if (!string.IsNullOrEmpty(Engineer.CommanderName) && uri.AbsolutePath.ToLower().Contains("edengineer"))
                {
                    Engineer.GetShoppingList();
                }
            }
        }

    }
}

