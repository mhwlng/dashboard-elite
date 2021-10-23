using System;
using System.Diagnostics;
using System.Threading.Tasks;
using dashboard_elite.Components;
using dashboard_elite.EliteData;
using dashboard_elite.Services;
using Elite;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace dashboard_elite.Shared
{
    public partial class PageLayout
    {

        [Inject] private Data Data { get; set; }


        /*
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private SvgCacheService SvgCacheService { get; set; }


        private MudTheme _currentTheme = Themes.darkTheme;

        bool _drawerOpen = false;

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
        }*/


    }
}
