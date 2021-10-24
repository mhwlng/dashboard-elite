using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dashboard_elite.EliteData;
using dashboard_elite.Helpers;
using dashboard_elite.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace dashboard_elite.Components
{
    public partial class PageBar
    {
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] private IJSRuntime MyJsRuntime { get; set; }

        public PageHelper.Page PageType { get; set; }

        public int CurrentPage { get; set; }

        [CascadingParameter] private RouteData RouteData { get; set; }

        protected override void OnParametersSet()
        {
             Enum.TryParse(RouteData.PageType.Name, true, out PageHelper.Page pageType);

             PageType = pageType;

            if (RouteData.RouteValues?.ContainsKey("CurrentPage") == true && !string.IsNullOrEmpty(RouteData.RouteValues["CurrentPage"]?.ToString()))
            {
                CurrentPage = Convert.ToInt32(RouteData.RouteValues["CurrentPage"].ToString());

                PageHelper.SetCurrentPage(pageType, CurrentPage);
            }
            else
            {
                CurrentPage = PageHelper.GetCurrentPage(pageType);
            }
        }

        private Task<IJSObjectReference> _module;
        private Task<IJSObjectReference> Module => _module ??= MyJsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/util.js").AsTask();

        async Task ScrollTableToTop()
        {
            var module = await Module;
            await module.InvokeVoidAsync("ScrollTableToTop", "PageTable");
        }

        async Task ScrollTableToBottom()
        {
            var module = await Module;
            await module.InvokeVoidAsync("ScrollTableToBottom", "PageTable");
        }

        private void PreviousPage(int currentPage)
        {
            CurrentPage = PageHelper.DecrementCurrentPage(RouteData.PageType.Name, CurrentPage);

            NavigationManager.NavigateTo($"{RouteData.PageType.Name}/{CurrentPage}");
        }

        private void NextPage(int currentPage)
        {
            CurrentPage = PageHelper.IncrementCurrentPage(RouteData.PageType.Name, CurrentPage);

            NavigationManager.NavigateTo($"{RouteData.PageType.Name}/{CurrentPage}");
        }

        public async ValueTask DisposeAsync()
        {
            if (_module != null)
            {
                var module = await _module;
                await module.DisposeAsync();
            }
        }
    }
}
