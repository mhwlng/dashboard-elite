using System;
using System.Threading;
using System.Threading.Tasks;
using dashboard_elite.EliteData;
using dashboard_elite.Services;
using Elite;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace dashboard_elite.Components
{
    public partial class PageBar
    {
        [Inject] private IJSRuntime MyJsRuntime { get; set; }

        public string PageTypeName { get; set; }
        [CascadingParameter] private RouteData RouteData { get; set; }

        protected override void OnParametersSet()
        {
            PageTypeName = RouteData.PageType.Name;
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
