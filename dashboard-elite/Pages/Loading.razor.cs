using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using PhotinoNET;



namespace dashboard_elite.Pages
{
    public partial class Loading : IAsyncDisposable
    {
        [Inject] private NavigationManager NavigationManager { get; set; }

        private HubConnection hubConnection;

        private string LoadingMessage;

        protected override async Task OnInitializedAsync()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/myhub"))
                .Build();

            hubConnection.On<string>("LoadingMessage", (loadingMessage) =>
            {
                LoadingMessage = loadingMessage;
                StateHasChanged();
            });

            hubConnection.On("LoadingDone", () =>
            {
                NavigationManager.NavigateTo("/index");
            });

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
