
using System;
using Microsoft.AspNetCore.Components;

namespace dashboard_elite.Shared
{
    public partial class NavMenu
    {
        [CascadingParameter] private RouteData RouteData { get; set; }

        [Parameter]
        public string NavSection { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; }

        protected override void OnParametersSet()
        {
            var uri = new Uri(NavigationManager.Uri);

            NavSection = uri.Segments[1].Replace("/", "");

        }

    }
}

