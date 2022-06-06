using System;
using System.Diagnostics;
using System.Threading.Tasks;
using dashboard_elite.Components;
using dashboard_elite.EliteData;
using dashboard_elite.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace dashboard_elite.Shared
{
    public partial class PageLayout
    {

        [Inject] private Data Data { get; set; }

        [Inject] private ProtectedLocalStorage ProtectedLocalStorage { get; set; }
        [Inject] private ProtectedSessionStorage ProtectedSessionStorage { get; set; }

        public int Window { get; set; }
        public int WindowCount { get; set; }
        public bool HideKeyboard { get; set; } = true;
        public bool HideInformation { get; set; } = true;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                Window = (await ProtectedSessionStorage.GetAsync<int>("Window")).Value;
                WindowCount = (await ProtectedSessionStorage.GetAsync<int>("WindowCount")).Value;

                HideInformation = (await ProtectedLocalStorage.GetAsync<bool>("HideInformation")).Value;

                HideKeyboard = (await ProtectedLocalStorage.GetAsync<bool>("HideKeyboard")).Value || WindowCount > 1;
                StateHasChanged();
            }
        }


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
