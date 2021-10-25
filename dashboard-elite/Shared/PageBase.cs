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

            // fixes problem with optional CurrentPage parameter.
            // get CurrentPage from route table, don't read variable directly
            // otherwise CurrentPage will have the value from the previous url, if CurrentPage is not set in the current url

            if (RouteData.RouteValues?.ContainsKey("CurrentPage") == true && !string.IsNullOrEmpty(RouteData.RouteValues["CurrentPage"]?.ToString()))
            {
                CurrentPage = Convert.ToInt32(RouteData.RouteValues["CurrentPage"].ToString());

                PageHelper.SetCurrentPage(pageType, (int)CurrentPage);
            }
            else
            {
                CurrentPage = PageHelper.GetCurrentPage(pageType);
            }
        }


        protected override async Task OnInitializedAsync()
        {
            OnParametersSet();

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
