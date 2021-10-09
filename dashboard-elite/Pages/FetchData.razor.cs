using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dashboard_elite;
using dashboard_elite.EliteData;
using dashboard_elite.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace dashboard_elite.Pages
{
    public partial class FetchData : IAsyncDisposable
    {
        [Inject] private Data Data { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        private HubConnection hubConnection;

        [Inject] private WeatherForecastService ForecastService { get; set; }

		private WeatherForecast[] forecasts;

	    protected override async Task OnInitializedAsync()
	    {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/myhub"))
                .Build();

            hubConnection.On("EliteRefresh", StateHasChanged);

            await hubConnection.StartAsync();

            forecasts = await ForecastService.GetForecastAsync(DateTime.Now);
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
