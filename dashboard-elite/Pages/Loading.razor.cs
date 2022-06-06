using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;



namespace dashboard_elite.Pages
{
    public partial class Loading : IAsyncDisposable
    {
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] private ProtectedSessionStorage protectedSessionStorage { get; set; }

        private HubConnection hubConnection;

        private string LoadingMessage;

        public int Window { get; set; }
        public int WindowCount { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var focusChange = NavigationManager.Uri.Contains("127.0.0.1");

            if (!focusChange)
            {
                NavigationManager.NavigateTo("/information/commander");
            }

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
                NavigationManager.NavigateTo("/information/commander");
            });

            await hubConnection.StartAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var fragment = new Uri(NavigationManager.Uri).Fragment;
                if (!string.IsNullOrEmpty(fragment))
                {
                    var fragments = fragment.Replace("#","").Split('-');
                    if (fragments.Length == 2)
                    {
                        Window = Convert.ToInt32(fragments[0]);
                        WindowCount = Convert.ToInt32(fragments[1]);
                        await protectedSessionStorage.SetAsync("Window", Window);
                        await protectedSessionStorage.SetAsync("WindowCount", WindowCount);
                    }
                }
            }
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
