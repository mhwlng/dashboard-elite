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
    public partial class PageBar : IAsyncDisposable
    {
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] private IJSRuntime MyJsRuntime { get; set; }

        [Inject] public Data Data { get; set; }

        [Inject] public Ships Ships { get; set; }

        [Inject] public Module Module { get; set; }


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

        private Task<IJSObjectReference> _moduleReference;
        private Task<IJSObjectReference> ModuleReference => _moduleReference ??= MyJsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/util.js").AsTask();

        async Task ScrollTableToTop()
        {
            var module = await ModuleReference;
            await module.InvokeVoidAsync("ScrollTableToTop", "PageTable");
        }

        async Task ScrollTableToBottom()
        {
            var module = await ModuleReference;
            await module.InvokeVoidAsync("ScrollTableToBottom", "PageTable");
        }

        async Task PreviousPage(int currentPage)
        {
            await ScrollTableToTop();

            CurrentPage = PageHelper.DecrementCurrentPage(RouteData.PageType.Name, CurrentPage, Ships,Module);

            var uri = new Uri(NavigationManager.Uri);

            NavigationManager.NavigateTo($"{uri.Segments[1]}{RouteData.PageType.Name}/{CurrentPage}");
        }

        async Task NextPage(int currentPage)
        {
            await ScrollTableToTop();

            CurrentPage = PageHelper.IncrementCurrentPage(RouteData.PageType.Name, CurrentPage, Ships, Module);

            var uri = new Uri(NavigationManager.Uri);

            NavigationManager.NavigateTo($"{uri.Segments[1]}{RouteData.PageType.Name}/{CurrentPage}");
        }

        public async ValueTask DisposeAsync()
        {
            if (_moduleReference != null)
            {
                var module = await _moduleReference;
                await module.DisposeAsync();
            }
        }
    }
}
