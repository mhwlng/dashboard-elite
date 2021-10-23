using System;
using System.Threading.Tasks;
using dashboard_elite.EliteData;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace dashboard_elite.Shared
{
    public class PageBase : ComponentBase, IAsyncDisposable
    {
        [Inject] public Data Data { get; set; }

        [Inject] public Elite.Galnet GalnetData { get; set; }

        [Inject] public NavigationManager NavigationManager { get; set; }

        public HubConnection hubConnection;

        protected override async Task OnInitializedAsync()
        {
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
