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
using System.Windows;
using Microsoft.JSInterop;

namespace dashboard_elite.Shared
{
    public partial class MainLayout : IAsyncDisposable
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private Data Data { get; set; }
        [Inject] private SvgCacheService SvgCacheService { get; set; }
        [Inject] private IJSRuntime MyJsRuntime { get; set; }

        [Inject] private ProtectedLocalStorage ProtectedLocalStorage { get; set; }
        [Inject] private ProtectedSessionStorage ProtectedSessionStorage { get; set; }

        private MudTheme _currentTheme = Themes.darkTheme;

        bool _drawerOpen = false;

        public int Window { get; set; }
        public int WindowCount { get; set; }
        public bool HideKeyboard { get; set; } = true;
        public bool HideInformation { get; set; } = true;

        private HubConnection hubConnection;
        
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                Window = (await ProtectedSessionStorage.GetAsync<int>("Window")).Value;

                WindowCount = (await ProtectedSessionStorage.GetAsync<int>("WindowCount")).Value;

                HideInformation = (await ProtectedLocalStorage.GetAsync<bool>("HideInformation")).Value;

                HideKeyboard = (await ProtectedLocalStorage.GetAsync<bool>("HideKeyboard")).Value;

                if (WindowCount > 1)
                {
                    HideInformation = false;
                    HideKeyboard = true;
                }

                var iframeName = await IframeName();

                if (!string.IsNullOrEmpty(iframeName))
                {
                    Window = Convert.ToInt32(iframeName.Replace("iframe", ""));
                }

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


        void Close()
        {
            Common.MainWindow.Dispatcher.Invoke(() => Common.MainWindow.Close());

        }

        void Maximize()
        {
            Common.MainWindow.Dispatcher.Invoke(() => {

                var currentstate = (Common.MainWindow.WindowStyle == WindowStyle.None);

                Common.MainWindow.WindowState = currentstate ? WindowState.Normal : WindowState.Maximized;

                CommandTools.AddOrUpdateAppSetting<bool>("Dimensions:FullScreen", !currentstate);

                Process.Start(Process.GetCurrentProcess().MainModule.FileName);

                Common.MainWindow.Close();

            });
        }

        void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        async Task KeyboardToggle()
        {
            HideKeyboard = !HideKeyboard;

            await ProtectedLocalStorage.SetAsync("HideKeyboard", HideKeyboard);

            NavigationManager.NavigateTo("/information/commander", forceLoad: true);

        }

        async Task InformationToggle()
        {
            HideInformation = !HideInformation;

            await ProtectedLocalStorage.SetAsync("HideInformation", HideInformation);

            NavigationManager.NavigateTo("/information/commander", forceLoad:true);


        }
        async Task WindowsToggle()
        {
            if (WindowCount < 2)
            {
                WindowCount = 2;
                await ProtectedSessionStorage.SetAsync("Window", 0);
                await ProtectedSessionStorage.SetAsync("WindowCount", WindowCount);
                await ProtectedLocalStorage.SetAsync("WindowCount", WindowCount);

                NavigationManager.NavigateTo($"/iframepage");
            }
            else
            {
                WindowCount = 0;

                await ProtectedSessionStorage.SetAsync("Window", 0);
                await ProtectedSessionStorage.SetAsync("WindowCount", WindowCount);
                await ProtectedLocalStorage.SetAsync("WindowCount", WindowCount);

                await IframeReload();
            }


        }

        private Task<IJSObjectReference> _moduleReference;
        private Task<IJSObjectReference> ModuleReference => _moduleReference ??= MyJsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/util.js").AsTask();

        async Task IframeReload()
        {
            var module = await ModuleReference;
            await module.InvokeVoidAsync("IframeReload");
        }

        async Task<string> IframeName()
        {
            var module = await ModuleReference; 
            return await module.InvokeAsync<string>("IframeName");
        }

        public async ValueTask DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }

            if (_moduleReference != null)
            {
                var module = await _moduleReference;
                await module.DisposeAsync();
            }
        }
    }
}
