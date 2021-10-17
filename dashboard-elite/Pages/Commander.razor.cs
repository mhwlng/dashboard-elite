using System;
using System.Threading.Tasks;
using dashboard_elite.EliteData;
using dashboard_elite.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace dashboard_elite.Pages
{
    public partial class Commander : IAsyncDisposable
    {
        [Inject] private Data Data { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; }

        private HubConnection hubConnection;


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
