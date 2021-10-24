using System;
using System.Threading.Tasks;
using dashboard_elite.EliteData;
using dashboard_elite.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace dashboard_elite.Shared
{
    public class PageBase : ComponentBase, IAsyncDisposable
    {
        [Parameter]
        public int? CurrentPage { get; set; }

        [Inject] public Data Data { get; set; }

        [Inject] public Galnet GalnetData { get; set; }

        [Inject] public NavigationManager NavigationManager { get; set; }

        public HubConnection hubConnection;

        [CascadingParameter] private RouteData RouteData { get; set; }

        protected override async Task OnInitializedAsync()
        {
            CurrentPage ??= PageHelper.GetCurrentPage(RouteData.PageType.Name);

            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/myhub"))
                .Build();


            hubConnection.On("EliteRefresh", StateHasChanged);

            await hubConnection.StartAsync();
        }

        public bool IsConnected =>
            hubConnection.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }

    }
}
