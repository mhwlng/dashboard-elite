using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;



namespace dashboard_elite.Pages
{
    public partial class Loading : IAsyncDisposable
    {
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] private ProtectedSessionStorage ProtectedSessionStorage { get; set; }

        [Inject] private ProtectedLocalStorage ProtectedLocalStorage { get; set; }


        private HubConnection hubConnection;

        private string LoadingMessage;

        public int Window { get; set; }
        public int WindowCount { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var focusChange = NavigationManager.Uri.Contains("127.0.0.1");

            if (focusChange)
            {
                hubConnection = new HubConnectionBuilder()
                    .WithUrl(NavigationManager.ToAbsoluteUri("/myhub"))
                    .Build();

                hubConnection.On<string>("LoadingMessage", (loadingMessage) =>
                {
                    LoadingMessage = loadingMessage;
                    //StateHasChanged();
                    InvokeAsync(StateHasChanged);

                });

                hubConnection.On("LoadingDone", () => { NavigationManager.NavigateTo("/information/commander"); });
                
                await hubConnection.StartAsync();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var focusChange = NavigationManager.Uri.Contains("127.0.0.1");

                var fragment = new Uri(NavigationManager.Uri).Fragment;
                if (!string.IsNullOrEmpty(fragment))
                {
                    var fragments = fragment.Replace("#", "").Split('-');
                    if (focusChange)
                    {
                        if (fragments.Length == 2)
                        {
                            Window = Convert.ToInt32(fragments[0]);
                            WindowCount = Convert.ToInt32(fragments[1]);
                            await ProtectedSessionStorage.SetAsync("Window", Window);
                            await ProtectedSessionStorage.SetAsync("WindowCount", WindowCount);
                            await ProtectedLocalStorage.SetAsync("WindowCount", WindowCount);

                        }
                    }
                    else
                    {
                        if (fragments.Length == 1)
                        {
                            WindowCount = Convert.ToInt32(fragments[0]);
                            if (WindowCount < 2)
                            {
                                WindowCount = 0;
                            }
                            await ProtectedSessionStorage.SetAsync("Window", 0);
                            await ProtectedSessionStorage.SetAsync("WindowCount", WindowCount);
                            await ProtectedLocalStorage.SetAsync("WindowCount", WindowCount);
                            if (WindowCount > 1)
                            {
                                NavigationManager.NavigateTo($"/iframepage");
                                return;
                            }
                            
                        }
                        else if (fragments.Length == 2)
                        {
                            Window = Convert.ToInt32(fragments[0]);
                            WindowCount = Convert.ToInt32(fragments[1]);
                            await ProtectedSessionStorage.SetAsync("Window", Window);
                            await ProtectedSessionStorage.SetAsync("WindowCount", WindowCount);
                            await ProtectedLocalStorage.SetAsync("WindowCount", WindowCount);

                        }

                    }

                }
                else
                {
                    WindowCount = (await ProtectedLocalStorage.GetAsync<int>("WindowCount")).Value;
                    if (WindowCount > 1)
                    {
                        await ProtectedSessionStorage.SetAsync("Window", 0);
                        await ProtectedSessionStorage.SetAsync("WindowCount", WindowCount);
 
                        NavigationManager.NavigateTo($"/iframepage");
                        return;

                    }
                }

                if (!focusChange)
                {
                    NavigationManager.NavigateTo("/information/commander");
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
