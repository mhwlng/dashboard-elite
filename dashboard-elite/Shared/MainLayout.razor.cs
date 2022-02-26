using System;
using System.Diagnostics;
using System.Threading.Tasks;
using dashboard_elite.EliteData;
using dashboard_elite.Helpers;
using dashboard_elite.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MudBlazor;

namespace dashboard_elite.Shared
{
    public partial class MainLayout
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private Data Data { get; set; }
        [Inject] private SvgCacheService SvgCacheService { get; set; }

        [Inject] ProtectedLocalStorage ProtectedLocalStorage { get; set; }

        private MudTheme _currentTheme = Themes.darkTheme;

        bool _drawerOpen = false;

        public bool HideKeyboard { get; set; } = true;
        public bool HideInformation { get; set; } = true;

        private HubConnection hubConnection;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                HideKeyboard = (await ProtectedLocalStorage.GetAsync<bool>("HideKeyboard")).Value;
                HideInformation = (await ProtectedLocalStorage.GetAsync<bool>("HideInformation")).Value;
                StateHasChanged();
            }
        }

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

        void Close()
        {
            Program.mainWindow.Close();
        }
        
        void Maximize()
        {
            var currentstate = Program.mainWindow.Chromeless;

            Program.mainWindow.SetMaximized(!currentstate);

            CommandTools.AddOrUpdateAppSetting<bool>("Dimensions:FullScreen", !currentstate);

            Process.Start(Process.GetCurrentProcess().MainModule.FileName);

            Program.mainWindow.Close();
        }

        void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        async void KeyboardToggle()
        {
            HideKeyboard = !HideKeyboard;

            await ProtectedLocalStorage.SetAsync("HideKeyboard", HideKeyboard);

            NavigationManager.NavigateTo("/information/commander", forceLoad: true);

        }

        async void InformationToggle()
        {
            HideInformation = !HideInformation;

            await ProtectedLocalStorage.SetAsync("HideInformation", HideInformation);

            NavigationManager.NavigateTo("/information/commander", forceLoad:true);


        }

    }
}
