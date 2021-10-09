using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using dashboard_elite.EliteData;
using Elite;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace dashboard_elite.Pages
{
    public partial class Counter : IAsyncDisposable
    {
        [Inject] private Data Data { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        private HubConnection hubConnection;

        private int currentCount = 0;

        private Timer timer;

        private string Message;


        protected override async Task OnInitializedAsync()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/myhub"))
                .Build();

            hubConnection.On<string, string>("ReceiveTestMessage", (user, message) =>
            {
                var encodedMsg = $"{user}: {message}";
                Message = encodedMsg;
                StateHasChanged();
            });

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


        private void IncrementCount()
        {
            currentCount++;
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                timer = new Timer();
                timer.Interval = 1000;
                timer.Elapsed += OnTimerInterval;
                timer.AutoReset = true;
                // Start the timer
                timer.Enabled = true;
            }
            base.OnAfterRender(firstRender);
        }

        private void OnTimerInterval(object sender, ElapsedEventArgs e)
        {
            IncrementCount();
            InvokeAsync(StateHasChanged);
        }


        public void Dispose()
        {
            // During prerender, this component is rendered without calling OnAfterRender and then immediately disposed
            // this mean timer will be null so we have to check for null or use the Null-conditional operator ? 
            timer?.Dispose();
        }


        private void ZoomIn()
        {
            Program.mainWindow.Zoom += 5;
        }

        private void ZoomOut()
        {
            Program.mainWindow.Zoom -= 5;
        }

        private void Center()
        {
            Program.mainWindow.Center();
        }

        private void Close()
        {
            Program.mainWindow.Close();
        }

        private void Minimize()
        {
            Program.mainWindow.SetMinimized(!Program.mainWindow.Minimized);
        }

        private void Maximize()
        {
            Program.mainWindow.SetMaximized(!Program.mainWindow.Maximized);
        }

        private void SetContextMenuEnabled()
        {
            Program.mainWindow.SetContextMenuEnabled(!Program.mainWindow.ContextMenuEnabled);
        }

        private void SetDevToolsEnabled()
        {
            Program.mainWindow.SetDevToolsEnabled(!Program.mainWindow.DevToolsEnabled);
        }

        private void SetFullScreen()
        {
            Program.mainWindow.SetFullScreen(!Program.mainWindow.FullScreen);
        }

        private void SetGrantBrowserPermissions()
        {
            Program.mainWindow.SetGrantBrowserPermissions(!Program.mainWindow.GrantBrowserPermissions);
        }

        private void SetIconFile()
        {
            var iconFile = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? "wwwroot/img/elite.ico"
                : "wwwroot/img/elite.png";

            Program.mainWindow.SetIconFile(iconFile);
        }

        private void SetPosition()
        {
            Program.mainWindow.SetLeft(Program.mainWindow.Left + 5);
            Program.mainWindow.SetTop(Program.mainWindow.Top + 5);
        }

        private void SetResizable()
        {
            Program.mainWindow.SetResizable(!Program.mainWindow.Resizable);
        }


        private void SetSizeUp()
        {
            Program.mainWindow.SetHeight(Program.mainWindow.Height + 5);
            Program.mainWindow.SetWidth(Program.mainWindow.Width + 5);
        }

        private void SetSizeDown()
        {
            Program.mainWindow.SetHeight(Program.mainWindow.Height - 5);
            Program.mainWindow.SetWidth(Program.mainWindow.Width - 5);
        }

        private void SetTitle()
        {
            Program.mainWindow.SetTitle(Program.mainWindow.Title + "*");
        }

        private void SetTopmost()
        {
            Program.mainWindow.SetTopMost(!Program.mainWindow.Topmost);
        }

        private void ShowState()
        {
            var properties = new StringBuilder();
            properties.AppendLine($"Title: {Program.mainWindow.Title}");
            properties.AppendLine($"Zoom: {Program.mainWindow.Zoom}");
            properties.AppendLine();
            properties.AppendLine($"ContextMenuEnabled: {Program.mainWindow.ContextMenuEnabled}");
            properties.AppendLine($"DevToolsEnabled: {Program.mainWindow.DevToolsEnabled}");
            properties.AppendLine($"GrantBrowserPermissions: {Program.mainWindow.GrantBrowserPermissions}");
            properties.AppendLine();
            properties.AppendLine($"Top: {Program.mainWindow.Top}");
            properties.AppendLine($"Left: {Program.mainWindow.Left}");
            properties.AppendLine($"Height: {Program.mainWindow.Height}");
            properties.AppendLine($"Width: {Program.mainWindow.Width}");
            properties.AppendLine();
            properties.AppendLine($"Resizable: {Program.mainWindow.Resizable}");
            properties.AppendLine($"Screen DPI: {Program.mainWindow.ScreenDpi}");
            properties.AppendLine($"Topmost: {Program.mainWindow.Topmost}");
            properties.AppendLine($"Maximized: {Program.mainWindow.Maximized}");
            properties.AppendLine($"Minimized: {Program.mainWindow.Minimized}");

            Program.mainWindow.OpenAlertWindow("Settings", properties.ToString());
        }

        private void FullScreen()
        {
            CommandTools.AddOrUpdateAppSetting<bool>("Dimensions:FullScreen", true);
        }

    }
}
