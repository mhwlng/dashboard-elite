using System;
using System.Threading.Tasks;
using dashboard_elite.EliteData;
using dashboard_elite.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace dashboard_elite.Shared
{
    public class PageBase : ComponentBase, IAsyncDisposable
    {
        [Parameter]
        public int? CurrentPage { get; set; }

        [Inject] public Data Data { get; set; }

        [Inject] public Galnet Galnet { get; set; }

        [Inject] public Poi Poi { get; set; }

        [Inject] public NavigationManager NavigationManager { get; set; }

        public HubConnection hubConnection;

        public PageHelper.Page PageType { get; set; }


        [CascadingParameter] private RouteData RouteData { get; set; }

        protected override void OnParametersSet()
        {
            Enum.TryParse(RouteData.PageType.Name, true, out PageHelper.Page pageType);

            PageType = pageType;

            if (CurrentPage != null)
            {
                PageHelper.SetCurrentPage(pageType, (int)CurrentPage);
            }
            else
            {
                CurrentPage = PageHelper.GetCurrentPage(pageType);
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

    }
}
